using BackEnd.Output;

using Dtos.Attributes;

using Exceptions;
using Exceptions.Common;
using Exceptions.DataAccess;
using Exceptions.BusinessLogic;

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Linq;
using System.Web.Http.Controllers;
using Dtos;
using Newtonsoft.Json.Linq;

namespace BackEnd.Controllers
{
    public abstract class ControllerBase : ApiController
    {
        private static Dictionary<Type, HttpStatusCode> ErrorStatusCodes = new Dictionary<Type, HttpStatusCode>()
        {
            { typeof(ValidationException), HttpStatusCode.BadRequest },
            { typeof(NotFoundException), HttpStatusCode.NotFound },
            { typeof(LoginDuplicationException), HttpStatusCode.Conflict },
            { typeof(InvalidPasswordException), HttpStatusCode.BadRequest },
            { typeof(InvalidKeyException), HttpStatusCode.Unauthorized },
            { typeof(SessionNotFoundException), HttpStatusCode.Unauthorized },
            { typeof(WrongSessionTokenException), HttpStatusCode.Unauthorized },
            { typeof(SessionExpiredException), HttpStatusCode.Unauthorized },
            { typeof(AccountNotFoundException), HttpStatusCode.NotFound },
            { typeof(WrongPasswordException), HttpStatusCode.Unauthorized },
            { typeof(NotAppropriateRoleException), HttpStatusCode.Forbidden },
            { typeof(WrongEncryptionException), HttpStatusCode.BadRequest },
            { typeof(MembershipDuplicationException), HttpStatusCode.Conflict },
            { typeof(MembershipNotFoundException), HttpStatusCode.NotFound },
            { typeof(AlreadyInsideException), HttpStatusCode.Conflict },
            { typeof(NotInsideException), HttpStatusCode.NotFound },
            { typeof(PointNotFoundException), HttpStatusCode.NotFound },
            { typeof(ForbiddenActionException), HttpStatusCode.Forbidden },
            { typeof(ExpressionParseException), HttpStatusCode.NotAcceptable }
        };

        public async Task<HttpResponseMessage> Execute<Tin>(Action<Tin> executor, Tin parameter)
        {
            Func<object> func = new Func<object>(() =>
            {
                executor(parameter);
                return null;
            });

            Func<Task<object>> task = new Func<Task<object>>(
                () => Task.Run(func)
            );

            return await ProtectedExecute(task, parameter, true);
        }

        public async Task<HttpResponseMessage> Execute<Tin>(Func<Tin, Task> executor, Tin parameter)
        {
            Func<Task<object>> task = new Func<Task<object>>(
                async () =>
                {
                    await executor(parameter);
                    return null;
                }
            );

            return await ProtectedExecute(task, parameter, true);
        }

        public async Task<HttpResponseMessage> Execute<Tout>(Func<Tout> executor)
        {
            return await ProtectedExecute(() => Task.Run(executor), null, false);
        }

        public async Task<HttpResponseMessage> Execute(Func<Task> executor)
        {
            Func<Task<object>> task = new Func<Task<object>>(
                async () =>
                {
                    await executor();
                    return null;
                }
            );

            return await ProtectedExecute(task, null, false);
        }

        public async Task<HttpResponseMessage> Execute<Tout>(Func<Task<Tout>> executor)
        {
            return await ProtectedExecute(executor, null, false);
        }

        public async Task<HttpResponseMessage> Execute<Tin, Tout>(Func<Tin, Tout> executor, Tin parameter)
        {
            Func<Task<Tout>> task = new Func<Task<Tout>>(
                () => Task.Run(
                    () => executor(parameter)
                )
            );

            return await ProtectedExecute(task, parameter, true);
        }

        public async Task<HttpResponseMessage> Execute<Tin, Tout>(Func<Tin, Task<Tout>> executor, Tin parameter)
        {
            return await ProtectedExecute(() => executor(parameter), parameter, true);
        }

        protected string GetHeaderOrCookie(string name, bool throwIfNotPresented)
        {
            string header = null;
            if (!Request.Headers.TryGetValues(name, out IEnumerable<string> tokens))
            {
                Collection<CookieHeaderValue> cookies = Request.Headers.GetCookies();
                if (cookies != null && cookies.Count > 0 && cookies[0][name] != null)
                    header = cookies[0][name].Value;
            }
            else
                header = tokens.FirstOrDefault();

            if(header == null && throwIfNotPresented)
                throw new ValidationException($"{name} was not presented");

            return header;
        }

        protected virtual void ValidateModel(object parameter, bool mustHaveParameter = true)
        {
            if (mustHaveParameter && parameter == null)
                throw new ValidationException("no data passed");

            ICollection<string> errors = new List<string>();
            foreach (KeyValuePair<string, ModelState> fieldState in ModelState)
                foreach (ModelError error in fieldState.Value.Errors)
                    errors.Add(error.ErrorMessage);

            Collection<HttpParameterDescriptor> parameterDescriptors = Request.GetActionDescriptor().GetParameters();
            if(parameterDescriptors != null)
            {
                foreach (HttpParameterDescriptor parameterDescriptor in parameterDescriptors)
                {
                    if (parameterDescriptor.GetCustomAttributes<IdentifiedAttribute>().Count == 0)
                        continue;

                    object parameterValue = ActionContext.ActionArguments[parameterDescriptor.ParameterName];

                    if (parameterValue == null || !CheckIdentified(parameterValue))
                    {
                        errors.Add("All IdDto inheritors must specify id field in indentified-market requests");
                        break;
                    }
                }
            }

            if (errors.Count != 0)
                throw new ValidationException(errors);
        }

        private bool CheckIdentified(object obj)
        {
            if (obj is IdDto idObj && !idObj.Id.HasValue)
                return false;

            if (obj is JObject)
                return true;

            Type type = obj.GetType();
            if (obj is IEnumerable<object> collection)
            {
                if (!CheckTypeContainsIdentified(type.GetGenericArguments()[0]))
                    return true;

                return collection.All(item => CheckIdentified(item));
            }

            PropertyInfo[] properties = type.GetProperties().Where(
                property => !property.PropertyType.IsValueType && !property.PropertyType.Equals(typeof(string))
            ).ToArray();

            foreach(PropertyInfo property in properties)
            {
                object value = property.GetValue(obj);

                if (value == null)
                {
                    if (property.PropertyType.IsGenericType)
                        continue;

                    if (CheckTypeContainsIdentified(property.PropertyType))
                        return false;
                }
                else if (!CheckIdentified(value))
                    return false;
            }

            return true;
        }

        private bool CheckTypeContainsIdentified(Type type)
        {
            if (typeof(IdDto).IsAssignableFrom(type))
                return true;

            return type.GetProperties().Where(
                property => !property.PropertyType.IsValueType && !property.PropertyType.Equals(typeof(string))
            ).Any(
                property => CheckTypeContainsIdentified(property.PropertyType)
            );
        }

        private HttpResponseMessage CreateErrorResponse(ServerException ex)
        {
            Type type = ex.GetType();

            if (ErrorStatusCodes.ContainsKey(type))
            {
                Response response = new Response() { Error = ex };
                return Request.CreateResponse(ErrorStatusCodes[type], response); 
            }

            //log it
            throw ex;
        }

        private void WireHeaders(object obj)
        {
            if (obj == null || obj.GetType().IsPrimitive)
                return;

            PropertyInfo[] properties = obj.GetType().GetProperties()
                .Where(property => property.GetCustomAttribute<HeaderAutoWiredAttribute>() != null)
                .ToArray();

            foreach (PropertyInfo property in properties)
            {
                HeaderAutoWiredAttribute attr = property.GetCustomAttribute<HeaderAutoWiredAttribute>();

                if (property.PropertyType.IsPrimitive || property.PropertyType.Equals(typeof(string)))
                {
                    string header = GetHeaderOrCookie(attr.HeaderName, attr.ThrowIfNotPresented);
                    try
                    {
                        object value = Convert.ChangeType(header, property.PropertyType);
                        property.SetValue(obj, value);
                    }
                    catch { throw new ValidationException($"Header {attr.HeaderName} is invalid"); }
                }
                else
                {
                    object value = property.GetValue(obj);

                    if(value == null)
                    {
                        value = property.PropertyType.GetConstructor(new Type[] { })
                            .Invoke(new object[] { });

                        property.SetValue(obj, value);
                    }

                    WireHeaders(value);
                }   
            }
        }

        private HttpResponseMessage CreateResponse(object data)
        {
            Response response = new Response() { Data = data };
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        private async Task<HttpResponseMessage> ProtectedExecute<Tout>(Func<Task<Tout>> task, object parameter, bool mustHaveParameter)
        {
            try
            {
                ValidateModel(parameter, mustHaveParameter);
                WireHeaders(parameter);
                return CreateResponse(await task());
            }
            catch (ServerException ex) { return CreateErrorResponse(ex); }
        }
    }
}

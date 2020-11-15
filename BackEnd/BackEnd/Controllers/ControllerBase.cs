using BackEnd.Output;

using Dtos;

using Exceptions;
using Exceptions.BackEnd;
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
using Dtos.Attributes;

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
        };

        public async Task<HttpResponseMessage> Execute(Action<object> executor, object parameter)
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

        public async Task<HttpResponseMessage> Execute(Func<object> executor)
        {
            Func<Task<object>> task = new Func<Task<object>>(
                () => Task.Run(executor)
            );

            return await ProtectedExecute(task, null, false);
        }

        public async Task<HttpResponseMessage> Execute<Tin, Tout>(Func<Tin, Task<Tout>> executor, Tin parameter)
        {
            Func<Task<Tout>> task = new Func<Task<Tout>>(
                () => executor(parameter)
            );

            return await ProtectedExecute(task, parameter, true);
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

            if (errors.Count != 0)
                throw new ValidationException(errors);
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
                .Where(property => property.GetCustomAttribute<HeaderAutoWired>() != null)
                .ToArray();

            foreach (PropertyInfo property in properties)
            {
                HeaderAutoWired attr = property.GetCustomAttribute<HeaderAutoWired>();

                if (property.PropertyType.IsPrimitive || property.PropertyType.Equals(typeof(string)))
                {
                    string header = GetHeaderOrCookie(attr.HeaderName, attr.ThrowIfNotPresented);
                    object value = Convert.ChangeType(header, property.PropertyType);
                    property.SetValue(obj, value);
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

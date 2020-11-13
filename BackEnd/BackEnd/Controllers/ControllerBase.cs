using BackEnd.Dto.Output;

using Exceptions;
using Exceptions.BackEnd;
using Exceptions.DataAccess;

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace BackEnd.Controllers
{
    public class ControllerBase : ApiController
    {
        private static Dictionary<Type, HttpStatusCode> ErrorStatusCodes = new Dictionary<Type, HttpStatusCode>()
        {
            { typeof(ValidationException), HttpStatusCode.BadRequest },
            { typeof(UnauthorizedAccessException), HttpStatusCode.Unauthorized },
            { typeof(NotFoundException), HttpStatusCode.NotFound }
        };

        public async Task<HttpResponseMessage> Execute(Action<object> executor, object parameter)
        {
            return await ProtectedExecute(
                new Task<object>(() =>
                {
                    executor(parameter);
                    return null;
                }), parameter, true
            );
        }

        public async Task<HttpResponseMessage> Execute<Tout>(Task<Tout> executor)
        {
            return await ProtectedExecute(executor, null, false);
        }

        public async Task<HttpResponseMessage> Execute<Tout>(Func<object, Task<Tout>> executor, object parameter)
        {
            return await ProtectedExecute(executor(parameter), parameter, true);
        }

        protected virtual void ValidateModel(object parameter, bool mustHaveParameter = true)
        {
            if (mustHaveParameter && parameter == null)
                throw new ValidationException("no data passed");

            ICollection<string> errors = new List<string>();
            foreach (KeyValuePair<string, ModelState> fieldState in ModelState)
                foreach (ModelError error in fieldState.Value.Errors)
                    errors.Add(error.ErrorMessage ?? error.Exception?.Message);

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

            throw ex;
        }

        private HttpResponseMessage CreateResponse(object data)
        {
            Response response = new Response() { Data = data };
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        private async Task<HttpResponseMessage> ProtectedExecute<Tout>(Task<Tout> task, object parameter, bool mustHaveParameter)
        {
            try
            {
                ValidateModel(parameter, mustHaveParameter);
                return CreateResponse(await task);
            }
            catch (ServerException ex) { return CreateErrorResponse(ex); }
        }
    }
}

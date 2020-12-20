using Autofac;

using BusinessLogic;
using BusinessLogic.ServiceContracts;

using Dtos;

using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace BackEnd.Controllers
{
    [EnableCors("*", "*", "*")]
    public class AuthController : ControllerBase
    {
        private static IAuthService AuthService = BusinessLogicDependencyHolder.Dependencies.Resolve<IAuthService>();

        [HttpPost]
        public async Task<HttpResponseMessage> SignUp([FromBody] SignUpDto signUpDto)
        {
            return await Execute(
                dto => AuthService.SignUp(dto),
                signUpDto
            );
        }

        [HttpPost]
        public async Task<HttpResponseMessage> LogIn(LogInDto logInDto)
        {
            return await Execute(
                dto => AuthService.LogIn(dto),
                logInDto
            );
        }
    }
}

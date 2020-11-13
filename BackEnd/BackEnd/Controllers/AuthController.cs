using Autofac;

using BusinessLogic;
using BusinessLogic.Models;
using BusinessLogic.ServiceContracts;

using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace BackEnd.Controllers
{
    public class AuthController : ControllerBase
    {
        private static IAuthService AuthService = BusinessLogicDependencyHolder.Dependencies.Resolve<IAuthService>();

        public async Task<HttpResponseMessage> CreateAccount([FromBody] AccountModel accountModel)
        {
            return await Execute(
                AuthService.Create(accountModel)
            );
        }
    }
}

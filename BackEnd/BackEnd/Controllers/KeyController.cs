using Autofac;

using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using BusinessLogic.ServiceContracts;
using BusinessLogic;

namespace BackEnd.Controllers
{
    public class KeyController : ControllerBase
    {
        private static IAsymmetricEncryptionService AsymmetricEncryptionService = 
            BusinessLogicDependencyHolder.Dependencies.Resolve<IAsymmetricEncryptionService>();

        [HttpGet]
        public async Task<HttpResponseMessage> GetPublicAsymmetricKey()
        {
            return await Execute(
                () => AsymmetricEncryptionService.GetNewPublicKey()
            );
        }
    }
}

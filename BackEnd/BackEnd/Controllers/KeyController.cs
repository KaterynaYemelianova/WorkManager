using Autofac;

using BusinessLogic.ServiceContracts;
using BusinessLogic;

using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace BackEnd.Controllers
{
    [EnableCors("*", "*", "*")]
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

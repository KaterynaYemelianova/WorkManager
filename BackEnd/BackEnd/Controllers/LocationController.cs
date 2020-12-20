using Autofac;

using BusinessLogic;
using BusinessLogic.ServiceContracts;

using Dtos;
using Dtos.Attributes;

using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace BackEnd.Controllers
{
    [EnableCors("*", "*", "*")]
    public class LocationController : ControllerBase
    {
        private IMemberLocationService MemberLocationService = BusinessLogicDependencyHolder.Dependencies.Resolve<IMemberLocationService>();

        [HttpPost]
        public async Task<HttpResponseMessage> GetLocationData([FromBody, Identified] AuthorizedDto<IdDto> dto)
        {
            return await Execute(d => MemberLocationService.GetLocationData(d), dto);
        }
    }
}
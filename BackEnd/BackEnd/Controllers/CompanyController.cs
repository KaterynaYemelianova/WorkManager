using Dtos;
using Dtos.Attributes;

using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using BusinessLogic;
using BusinessLogic.ServiceContracts;

using Autofac;

namespace BackEnd.Controllers
{
    public class CompanyController : ControllerBase
    {
        private ICompanyService CompanyService = BusinessLogicDependencyHolder.Dependencies.Resolve<ICompanyService>();

        [HttpGet]
        public async Task<HttpResponseMessage> Get()
        {
            return await Execute(CompanyService.GetCompaniesList);
        }

        [HttpGet]
        public async Task<HttpResponseMessage> Get(int id)
        {
            return await Execute(() => CompanyService.GetCompanyById(id));
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Register([FromBody] AuthorizedDto<CompanyDto> dto)
        {
            return await Execute(d => CompanyService.RegisterCompany(d), dto);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Update([FromBody, Identified] AuthorizedDto<CompanyDto> dto)
        {
            return await Execute(d => CompanyService.UpdateCompany(d), dto);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Delete([FromBody, Identified] AuthorizedDto<IdDto> dto)
        {
            return await Execute(d => CompanyService.DeleteCompany(d), dto);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> AddRoom([FromBody] AuthorizedDto<RoomDto> dto)
        {
            return await Execute(d => CompanyService.AddRoom(d), dto);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> UpdateRoom([FromBody, Identified] AuthorizedDto<RoomDto> dto)
        {
            return await Execute(d => CompanyService.UpdateRoom(d), dto);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> DeleteRoom([FromBody, Identified] AuthorizedDto<IdDto> dto)
        {
            return await Execute(d => CompanyService.DeleteRoom(d), dto);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> AddMember([FromBody] AuthorizedDto<AccountCompanyRoleDto> dto)
        {
            return await Execute(d => CompanyService.AddMember(d), dto);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> UpdateMember([FromBody, Identified] AuthorizedDto<AccountCompanyRoleDto> dto)
        {
            return await Execute(d => CompanyService.UpdateMember(d), dto);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> DeleteMember([FromBody, Identified] AuthorizedDto<IdDto> dto)
        {
            return await Execute(d => CompanyService.DeleteMember(d), dto);
        }
    }
}

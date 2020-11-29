  using BusinessLogic.Models;

using Dtos;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.ServiceContracts
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyModel>> GetCompaniesList();
        Task<CompanyModel> GetCompanyById(int id);

        Task<CompanyModel> RegisterCompany(AuthorizedDto<CompanyDto> dto);
        Task<CompanyModel> UpdateCompany(AuthorizedDto<CompanyDto> dto);
        Task DeleteCompany(AuthorizedDto<IdDto> dto);

        Task AddMember(AuthorizedDto<AccountCompanyRoleDto> dto);
        Task UpdateMember(AuthorizedDto<AccountCompanyRoleDto> dto);
        Task DeleteMember(AuthorizedDto<IdDto> dto);

        Task<CompanyModel> AddRoom(AuthorizedDto<RoomDto> dto);
        Task<RoomModel> UpdateRoom(AuthorizedDto<RoomDto> dto);
        Task DeleteRoom(AuthorizedDto<IdDto> dto);
    }
}

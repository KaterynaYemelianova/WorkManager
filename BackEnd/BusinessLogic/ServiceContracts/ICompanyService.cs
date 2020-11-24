using BusinessLogic.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.ServiceContracts
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyModel>> GetCompaniesList();

        Task<CompanyModel> RegisterCompany();
    }
}

using BusinessLogic.Models;

using Dtos;

using System.Threading.Tasks;

namespace BusinessLogic.ServiceContracts
{
    public interface IAuthService
    {
        Task<AccountModel> SignUp(SignUpDto signUpDto);
        Task<SessionModel> LogIn(LogInDto logInDto);
    }
}

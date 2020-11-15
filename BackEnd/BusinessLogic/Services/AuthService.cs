using Autofac;

using BusinessLogic.Models;
using BusinessLogic.ServiceContracts;

using DataAccess;
using DataAccess.Entities;
using DataAccess.RepoContracts;

using Dtos;
using Exceptions.BusinessLogic;

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace BusinessLogic.Services
{
    internal class AuthService : IAuthService
    {
        private static IAccountRepo AccountRepo = DataAccessDependencyHolder.Dependencies.Resolve<IAccountRepo>();
        private static IAsymmetricEncryptionService EncryptionService = BusinessLogicDependencyHolder.Dependencies.Resolve<IAsymmetricEncryptionService>();
        private static IHashingService HashingService = BusinessLogicDependencyHolder.Dependencies.Resolve<IHashingService>();

        private static Regex PasswordPattern = new Regex("^[A-Za-z0-9]{8,32}$");

        public async Task<AccountModel> SignUp(SignUpDto signUpDto)
        {
            if (await AccountRepo.GetByLogin(signUpDto.Login) != null)
                throw new LoginDuplicationException();

            PublicKeyModel key = DtoModelMapper.Mapper.Map<PublicKeyModel>(signUpDto.PublicKey);

            string password = null;

            try { password = EncryptionService.Decrypt(signUpDto.PasswordEncrypted, key); }
            catch(KeyNotFoundException ex) { throw new InvalidKeyException(); }

            EncryptionService.DestroyKeyPair(key);

            if (!PasswordPattern.IsMatch(password))
                throw new InvalidPasswordException();

            string passwordHash = HashingService.GetHashUTF8(password);

            AccountModel account = DtoModelMapper.Mapper.Map<AccountModel>(signUpDto);
            account.PasswordHash = passwordHash;

            AccountEntity accountEntity = EntityModelMapper.Mapper.Map<AccountEntity>(account);
            AccountEntity inserted = await AccountRepo.Insert(accountEntity);

            return EntityModelMapper.Mapper.Map<AccountModel>(inserted);
        }

        public Task<SessionModel> LogIn(LogInDto logInDto)
        {
            return null;
        }
    }
}

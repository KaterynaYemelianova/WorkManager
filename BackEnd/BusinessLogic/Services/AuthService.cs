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
using System;
using Exceptions.Common;
using System.Security.Cryptography;

namespace BusinessLogic.Services
{
    internal class AuthService : IAuthService
    {
        private static IAccountRepo AccountRepo = 
            DataAccessDependencyHolder.Dependencies.Resolve<IAccountRepo>();

        private static IAsymmetricEncryptionService EncryptionService = 
            BusinessLogicDependencyHolder.Dependencies.Resolve<IAsymmetricEncryptionService>();

        private static IHashingService HashingService = 
            BusinessLogicDependencyHolder.Dependencies.Resolve<IHashingService>();

        private static ISessionService SessionService =
            BusinessLogicDependencyHolder.Dependencies.Resolve<ISessionService>();

        private static Regex PasswordPattern = new Regex("^[A-Za-z0-9]{8,32}$");

        public async Task<AccountModel> SignUp(SignUpDto signUpDto)
        {
            if (await AccountRepo.GetByLogin(signUpDto.Login) != null)
                throw new LoginDuplicationException();

            PublicKeyModel key = DtoModelMapper.Mapper.Map<PublicKeyModel>(signUpDto.PublicKey);

            string password = null;

            try { password = EncryptionService.Decrypt(signUpDto.PasswordEncrypted, key); }
            catch(KeyNotFoundException) { throw new InvalidKeyException(); } 
            catch(FormatException) { throw new ValidationException("Password format was not match Base64"); }
            catch(CryptographicException) { throw new WrongEncryptionException("Password"); }

            EncryptionService.DestroyKeyPair(key);

            if (!PasswordPattern.IsMatch(password))
                throw new InvalidPasswordException();

            string passwordHash = HashingService.GetHashHex(password);

            AccountModel account = DtoModelMapper.Mapper.Map<AccountModel>(signUpDto);
            account.PasswordHash = passwordHash;

            AccountEntity accountEntity = EntityModelMapper.Mapper.Map<AccountEntity>(account);
            AccountEntity inserted = await AccountRepo.Insert(accountEntity);

            return EntityModelMapper.Mapper.Map<AccountModel>(inserted);
        }

        public async Task<SessionModel> LogIn(LogInDto logInDto)
        {
            AccountEntity account = await AccountRepo.FirstOrDefault(acc => acc.Login, logInDto.Login);

            if (account == null)
                throw new AccountNotFoundException();

            string saltedPassword = account.Password + logInDto.Salt;
            string saltedPasswordHash = HashingService.GetHashHex(saltedPassword);

            if (logInDto.PasswordSalted.ToUpper() != saltedPasswordHash.ToUpper())
                throw new WrongPasswordException();

            return SessionService.CreateSessionFor(account.Id);
        }
    }
}

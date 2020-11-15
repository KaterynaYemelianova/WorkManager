using Autofac;

using BusinessLogic.Models;
using BusinessLogic.ServiceContracts;

using Dtos;

using Exceptions.BusinessLogic;

using System;
using System.Collections.Generic;

namespace BusinessLogic.Services
{
    internal class SessionService : ISessionService
    {
        private const int SESSION_DURATION = 3600;

        private static IDictionary<int, SessionModel> Sessions = new Dictionary<int, SessionModel>();
        private static IHashingService Hasher = BusinessLogicDependencyHolder.Dependencies.Resolve<IHashingService>();

        private string GenerateToken()
        {
            string seed = Guid.NewGuid().ToString();
            return Hasher.GetHashUTF8(seed);
        }

        public SessionModel CreateSessionFor(int accountId)
        {
            if (Sessions.ContainsKey(accountId))
                Sessions.Remove(accountId);

            string token = GenerateToken();
            DateTime expires = DateTime.Now.AddSeconds(SESSION_DURATION);

            SessionModel session = new SessionModel()
            {
                Token = token,
                ExpiredAt = expires
            };

            Sessions.Add(accountId, session);
            return session;
        }

        public void CheckSession(SessionDto sessionDto)
        {
            int userId = sessionDto.UserId;
            if (!Sessions.ContainsKey(userId))
                throw new SessionNotFoundException();

            SessionModel session = Sessions[userId];

            string originalTokenSalted = Hasher.GetHashUTF8(session.Token + sessionDto.Salt);
            if (originalTokenSalted.ToUpper() != sessionDto.SessionTokenSalted.ToUpper())
                throw new WrongSessionTokenException();

            if (session.ExpiredAt < DateTime.Now)
            {
                Sessions.Remove(userId);
                throw new SessionExpiredException();
            }

            double secondsLeft = (session.ExpiredAt - DateTime.Now).TotalSeconds;

            if (secondsLeft < SESSION_DURATION / 2)
                session.ExpiredAt = session.ExpiredAt.AddSeconds(SESSION_DURATION - secondsLeft);
        }

        public void TerminateSession(SessionDto sessionDto)
        {
            CheckSession(sessionDto);
            Sessions.Remove(sessionDto.UserId);
        }
    }
}

using BusinessLogic.Models;

using Dtos;

namespace BusinessLogic.ServiceContracts
{
    internal interface ISessionService
    {
        SessionModel CreateSessionFor(int accountId);
        void CheckSession(SessionDto sessionDto);
        void TerminateSession(SessionDto sessionDto);
    }
}

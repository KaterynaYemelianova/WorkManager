using Autofac;

using BusinessLogic.Models.Data;
using BusinessLogic.ServiceContracts;
using BusinessLogic.ServiceContracts.PointServiceContracts;
using System;

namespace BusinessLogic.Services
{
    internal class ConditionParseService : IConditionParseService
    {
        protected MemberLocationService MemberLocationService =
            BusinessLogicDependencyHolder.Dependencies.Resolve<MemberLocationService>();

        private IEnterPointService EnterPointService = 
            BusinessLogicDependencyHolder.Dependencies.Resolve<IEnterPointService>();

        private ICheckPointService CheckPointService =
            BusinessLogicDependencyHolder.Dependencies.Resolve<ICheckPointService>();

        private IInteractionPointService InteractionPointService =
            BusinessLogicDependencyHolder.Dependencies.Resolve<IInteractionPointService>();

        private IControlPointService ControlPointService =
            BusinessLogicDependencyHolder.Dependencies.Resolve<IControlPointService>();

        public bool ParseCondition(string condition, ActionParseParameters parameters)
        {
            
            //TODO
            return true;
        }
    }
}

using BusinessLogic.Models;
using BusinessLogic.Models.Data;

using System;

namespace BusinessLogic.ServiceContracts
{
    internal interface IConditionParseService
    {
        bool ParseCondition(string condition, ActionParseParameters parameters);
    }
}

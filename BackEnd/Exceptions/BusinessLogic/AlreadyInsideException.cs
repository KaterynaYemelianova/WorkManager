using System;
using System.Collections.Generic;
using System.Text;

namespace Exceptions.BusinessLogic
{
    public class AlreadyInsideException : ServerException
    {
        public override int Code => 16;

        public override string Message => "Member is already inside";
    }
}

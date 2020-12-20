using System;
using System.Collections.Generic;
using System.Text;

namespace Exceptions.BusinessLogic
{
    public class NotInsideException : ServerException
    {
        public override int Code => 17;

        public override string Message => "Member was not inside";
    }
}

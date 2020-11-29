using System;
using System.Collections.Generic;
using System.Text;

namespace Exceptions.BusinessLogic
{
    public class PointNotFoundException : ServerException
    {
        public override int Code => 15;

        public override string Message => "Point was not found";
    }
}

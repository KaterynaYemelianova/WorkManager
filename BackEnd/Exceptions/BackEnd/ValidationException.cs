using System;
using System.Collections.Generic;

namespace Exceptions.BackEnd
{
    public class ValidationException : ServerException
    {
        public ValidationException(IEnumerable<string> messages) :
            base(string.Join(";\n", messages)) { }

        public ValidationException(params string[] messages) :
            this(messages as IEnumerable<string>) { }

        public override int Code => 1;
    }
}
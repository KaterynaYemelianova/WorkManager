using System;

namespace Exceptions.DataAccess
{
    public class NotFoundException : ServerException
    {
        public string What { get; private set; }

        public NotFoundException(string what) : base(what) { }

        public override string Message => $"{What} not found";

        public override int Code => 2;
    }
}

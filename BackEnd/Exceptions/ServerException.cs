using System;

namespace Exceptions
{
    public abstract class ServerException : Exception
    {
        public abstract int Code { get; }

        public ServerException(string message) : base(message) { }
    }
}

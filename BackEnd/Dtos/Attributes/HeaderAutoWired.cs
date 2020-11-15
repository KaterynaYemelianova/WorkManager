using System;

namespace Dtos.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class HeaderAutoWired : Attribute
    {
        public string HeaderName { get; private set; }
        public bool ThrowIfNotPresented { get; private set; }

        public HeaderAutoWired(string headerName = null, bool throwIfNotPresented = true)
        {
            HeaderName = headerName;
            ThrowIfNotPresented = throwIfNotPresented;
        }
    }
}
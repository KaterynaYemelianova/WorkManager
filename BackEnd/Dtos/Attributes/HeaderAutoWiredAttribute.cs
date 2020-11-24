using System;

namespace Dtos.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class HeaderAutoWiredAttribute : Attribute
    {
        public string HeaderName { get; private set; }
        public bool ThrowIfNotPresented { get; private set; }

        public HeaderAutoWiredAttribute(string headerName = null, bool throwIfNotPresented = true)
        {
            HeaderName = headerName;
            ThrowIfNotPresented = throwIfNotPresented;
        }
    }
}
using System;

namespace DataAccess.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    internal class TableAttribute : Attribute
    {
        public string Name { get; private set; }

        public TableAttribute(string name)
        {
            Name = name;
        }
    }
}
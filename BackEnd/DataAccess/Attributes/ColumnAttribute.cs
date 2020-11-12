using System;

namespace DataAccess.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class ColumnAttribute : Attribute
    {
        public string Name { get; private set; }

        public ColumnAttribute(string name)
        {
            Name = name;
        }
    }
}

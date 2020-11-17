using System;

namespace DataAccess.Attributes
{
    public class FValueAttribute : Attribute
    {
        public Type ReferencingType { get; private set; }
        public int? ReferenceNumber { get; private set; }

        public FValueAttribute(Type referencingType, int? referenceNumber = null)
        {
            ReferencingType = referencingType;
            ReferenceNumber = referenceNumber;
        }
    }
}

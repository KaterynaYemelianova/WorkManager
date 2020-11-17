using System;

namespace DataAccess.Attributes
{
    public class FKeyAttribute : Attribute
    {
        public Type ReferencedType { get; private set; }
        public int? ReferenceNumber { get; private set; }

        public FKeyAttribute(Type referencedType, int? referenceNumber = null)
        {
            ReferencedType = referencedType;
            ReferenceNumber = refrenceNumber;
        }
    }
}

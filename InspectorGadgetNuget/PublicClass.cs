using System;

namespace InspectorGadget
{
    [Serializable]
    public class PublicClass : TypeBase
    {  
        public PublicClass(){}

        public PublicClass(Type type) : base(type)
        {}
    }
}
using System;

namespace InspectorGadget
{
    [Serializable]
    public class PublicInterface : TypeBase
    {
        public PublicInterface(){}

        public PublicInterface(Type type) : base(type)
        {}
    }
}
using System;
using System.Collections.Generic;

namespace InspectorGadget
{
    [Serializable]
    public class PublicSurfaceArea
    {
        public Dictionary<string, PublicInterface> PublicInterfaces = new Dictionary<string, PublicInterface>();
        public Dictionary<string, PublicClass> PublicClasses = new Dictionary<string, PublicClass>();
    }
}
using System;

namespace InspectorGadget
{
    [Serializable]
    public class TypesToTrack
    {
        public PublicSurfaceArea PreviousPublicClasses = new PublicSurfaceArea();
        public PublicSurfaceArea CurrentPublicClasses = new PublicSurfaceArea();
    }
}
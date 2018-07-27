using System;
using System.IO;
using System.Text;

namespace InspectorGadget
{
    public static class Inspect
    {

        const string BasePath = @"../InspectorGadget.json";
              
        public static void InspectAll(Type type)
        {
            var added = new StringBuilder();
            var removed = new StringBuilder();
            var typesToTrack = new TypesToTrack
            {
                CurrentPublicClasses = new PublicSurfaceArea
                {
                    PublicInterfaces = InspectingTools.GetPublicInterfaces(type),
                    PublicClasses = InspectingTools.GetPublicClasses(type)                    
                }
            };

            if (!File.Exists(BasePath))
            {
                InspectingTools.WriteToJsonFile(BasePath, typesToTrack);
            }

            var oldFile = InspectingTools.ReadFromJsonFile(BasePath);

            typesToTrack.PreviousPublicClasses = oldFile.CurrentPublicClasses;

            InspectingTools.DetectChangedInterfaceMethodsAndProperties(typesToTrack, added, removed);

            InspectingTools.DetectChangedClassPropertiesAndMethods(typesToTrack, added, removed);

            if (!string.IsNullOrWhiteSpace(added.ToString()))
                Console.WriteLine(added);
            
            if (!string.IsNullOrWhiteSpace(removed.ToString()))
            {
                Console.WriteLine(removed);
            }
                
            
            InspectingTools.WriteToJsonFile(BasePath, typesToTrack);
        }     
    }
}
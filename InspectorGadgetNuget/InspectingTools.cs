using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace InspectorGadget
{
    public static class InspectingTools
    {       
        public static Dictionary<string, PublicClass> GetPublicClasses(Type type)
        {
            var assembly = type.Assembly;

            var publicClasses = assembly.GetExportedTypes();
            
            return publicClasses
                .ToDictionary(p => p.FullName, 
                    p => new PublicClass(p));
        }

        public static Dictionary<string, PublicInterface> GetPublicInterfaces(Type type)
        {
            var assembly = type.Assembly;                       
            
            var interfaces = assembly.GetExportedTypes()
                .Where(o => o.IsInterface);

            return interfaces
                .ToDictionary(o => o.FullName,
                    o => new PublicInterface(o));
        }

        public static void DetectChangedInterfaceMethodsAndProperties(TypesToTrack workingType, StringBuilder added, StringBuilder removed)
        {
            var currentInterfaces = workingType.CurrentPublicClasses.PublicInterfaces;
            var previousInterfaces = workingType.PreviousPublicClasses.PublicInterfaces;

            foreach (var c in currentInterfaces)
            {
                added.Append(DetectChangedInterfacesHelper(previousInterfaces, c, " was added"));
            }
            foreach (var p in previousInterfaces)
            {
                removed.Append(DetectChangedInterfacesHelper(currentInterfaces, p, " was removed"));
            }
        }

        public static void DetectChangedClassPropertiesAndMethods(TypesToTrack workingType, StringBuilder added, StringBuilder removed)
        {
            var currentClasses = workingType.CurrentPublicClasses.PublicClasses;
            var previousClasses = workingType.PreviousPublicClasses.PublicClasses;

            foreach (var c in currentClasses)
            {
                added.Append(DetectChangedClassesHelper(previousClasses, c, " was added"));   
            }
            foreach (var p in previousClasses)
            {
                removed.Append(DetectChangedClassesHelper(currentClasses, p, " was removed"));
            }
        }

        private static string DetectChangedInterfacesHelper(Dictionary<string, PublicInterface> comparer, 
                                                            KeyValuePair<string, PublicInterface> iterator, string method)
        {
            var sb = new StringBuilder();
            if (!comparer.ContainsKey(iterator.Key))
            {
                sb.Append(iterator.Value);
                sb.Append(" ");
                sb.Append(iterator.Key);
                sb.Append(method);
                sb.AppendLine();
                return sb.ToString();
            }
            var allMethodsAndProperties = iterator.Value.PublicMethods
                .Concat(iterator.Value.PublicProperties)
                .ToDictionary(o => o.Key, o => o.Value);
            var previousDic = comparer
                .First(o =>
                   o.Key.Equals(iterator.Key));
            var allPreviousMethodsAndProperties = previousDic.Value.PublicMethods
                .Concat(previousDic.Value.PublicProperties)
                .ToDictionary(o => o.Key, o => o.Value);
            foreach (var kvp in allMethodsAndProperties)
            {
                if (!allPreviousMethodsAndProperties.ContainsKey(kvp.Key))
                {
                    sb.Append(kvp.Value);
                    sb.Append(" ");
                    sb.Append(kvp.Key);
                    sb.Append(method);
                    sb.AppendLine();
                }
            }
            return sb.ToString();
        }

        private static string DetectChangedClassesHelper(Dictionary<string, PublicClass> comparer, 
                                                         KeyValuePair<string, PublicClass> iterator, string method)
        {
            var sb = new StringBuilder();
            if (!comparer.ContainsKey(iterator.Key))
            {
                sb.Append(iterator.Value);
                sb.Append(" ");
                sb.Append(iterator.Key);
                sb.Append(method);
                sb.AppendLine();
                return sb.ToString();
            }
            var allMethodsAndProperties = iterator.Value.PublicMethods
                .Concat(iterator.Value.PublicProperties)
                .ToDictionary(o => o.Key, o => o.Value);
            var previousDic = comparer
                .First(o =>
                   o.Key.Equals(iterator.Key));
            var allPreviousMethodsAndProperties = previousDic.Value.PublicMethods
                .Concat(previousDic.Value.PublicProperties)
                .ToDictionary(o => o.Key, o => o.Value);
            foreach (var kvp in allMethodsAndProperties)
            {
                if (!allPreviousMethodsAndProperties.ContainsKey(kvp.Key))
                {
                    sb.Append(kvp.Value);
                    sb.Append(" ");
                    sb.Append(kvp.Key);
                    sb.Append(method);
                    sb.AppendLine();
                }
            }
            return sb.ToString();
        }



        public static void WriteToJsonFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            TextWriter writer = null;
            try
            {
                writer = new StreamWriter(filePath, append);
                var x = JsonConvert.SerializeObject(objectToWrite, Formatting.Indented);
                File.WriteAllText(filePath, x);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Dispose();
                    writer.Close();
                }
            }
        }        
        
        public static TypesToTrack ReadFromJsonFile(string filePath) 
        {
            return JsonConvert.DeserializeObject<TypesToTrack>(File.ReadAllText(filePath));
        }

    }
}
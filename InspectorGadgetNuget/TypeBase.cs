using System;
using System.Collections.Generic;
using System.Reflection;

namespace InspectorGadget
{
    [Serializable]
    public abstract class TypeBase
    {
        public Dictionary<string, string> PublicProperties = new Dictionary<string, string>();
        public Dictionary<string, string> PublicMethods = new Dictionary<string, string>();
        public int test;

        public TypeBase(){}

        public TypeBase(Type type)
        {
            if (type == null) return;

            
            var properties = type.GetFields(BindingFlags.DeclaredOnly |
                                            BindingFlags.Instance     |
                                            BindingFlags.Static       |
                                            BindingFlags.Public);
        
            foreach (var p in properties)
            {
                PublicProperties.Add(p.Name, p.DeclaringType.FullName.Replace("Temp.", ""));
            }
            var methods = type.GetMethods(BindingFlags.DeclaredOnly |
                                          BindingFlags.Instance     |
                                          BindingFlags.Static       |
                                          BindingFlags.Public);
            foreach (var m in methods)
            {
                PublicMethods.Add(m.MethodSignature(), m.DeclaringType.FullName.Replace("Temp.", ""));
            }   
        } 
    }
}
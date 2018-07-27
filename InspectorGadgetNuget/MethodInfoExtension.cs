using System.Linq;
using System.Reflection;

namespace InspectorGadget
{
    public static class MethodInfoExtension
    {
        public static string MethodSignature(this MethodInfo mi)
        {
            var param = mi.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}").ToArray();
            var signature = $"{mi.ReturnType.Name} {mi.Name}({string.Join(",", param)})";
            return signature;
        }
    }
}
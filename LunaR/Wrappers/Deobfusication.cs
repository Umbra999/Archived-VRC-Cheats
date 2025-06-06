using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnhollowerBaseLib.Attributes;

namespace LunaR.Wrappers
{
    internal class Deobfusication
    {
        internal static readonly Dictionary<string, Type> DeobfuscatedTypes = new();

        internal static readonly Dictionary<string, string> reverseDeobCache = new();

        public static void Load()
        {
            BuildDeobfuscationCache();
        }

        private static void BuildDeobfuscationCache()
        {
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in TryGetTypes(asm))
                {
                    TryCacheDeobfuscatedType(type);
                }
            }
        }

        private static void TryCacheDeobfuscatedType(Type type)
        {
            try
            {
                if (type.CustomAttributes.Any())
                {
                    foreach (CustomAttributeData customAttributeData in type.CustomAttributes)
                    {
                        bool flag2 = customAttributeData.AttributeType == typeof(ObfuscatedNameAttribute);
                        if (flag2)
                        {
                            string text = customAttributeData.ConstructorArguments[0].Value.ToString();
                            DeobfuscatedTypes.Add(text, type);
                            reverseDeobCache.Add(type.FullName, text);
                        }
                    }
                }
            }
            catch
            {
            }
        }
        public static IEnumerable<Type> TryGetTypes(Assembly asm)
        {
            IEnumerable<Type> result;
            try
            {
                result = asm.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                try
                {
                    result = asm.GetExportedTypes();
                }
                catch
                {
                    result = from t in ex.Types where t != null select t;
                }
            }
            catch
            {
                result = Enumerable.Empty<Type>();
            }
            return result;
        }
        public static Type GetUnhollowedType(Il2CppSystem.Type cppType)
        {
            if (DeobfuscatedTypes.Count == 0)
            {
                BuildDeobfuscationCache();
            }

            if (DeobfuscatedTypes.TryGetValue(cppType.FullName, out var deob)) return deob;

            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections;

namespace RFClassLibrary
{
    /// <summary>
    /// Class using reflection and recursion to examine an object and return information about it.
    /// </summary>
    public static class ObjectProperties
    {
        /// <summary>
        /// Returns a list of properties for the given type.
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static IEnumerable<string> IteratePropertiesRecursively(string prefix, Type t)
        {
            if (!string.IsNullOrEmpty(prefix) && !prefix.EndsWith("."))
                prefix += ".";

            prefix += t.Name + ".";

            var flags = BindingFlags.Public | BindingFlags.Instance;
            // enumerate the properties of the type
            foreach (PropertyInfo p in t.GetProperties(flags))
            {
                Type pt = p.PropertyType;

                if (p.PropertyType.Module.ScopeName != "CommonLanguageRuntimeLibrary")
                {
                    foreach (var propName in GetPropertiesRecursive(p.PropertyType, prefix))
                    {
                        yield return propName;
                    }
                }
                else
                {
                    if (pt.Name == "List`1")
                    {
                        Type genericType = pt.GetGenericArguments()[0];
                        // then enumerate the generic subtype
                        yield return prefix + p.Name + "|" + genericType.Name;
                        foreach (string propertyName in GetPropertiesRecursive(genericType, prefix))
                        {
                            yield return propertyName;
                        }
                    }
                    else
                    {
                        yield return prefix + p.Name;
                    }
                }
            }
        }

        /// <summary>
        /// Returns a list of properties for the given type. Does not drill into List&lt;object&gt; instances.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="propName"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetPropertiesRecursive(Type t, string propName = "", string prefix = "")
        {
            if (!string.IsNullOrEmpty(prefix) && !prefix.EndsWith("."))
                prefix += ".";

            prefix += (propName ?? t.Name) + ".";

            var flags = BindingFlags.Public | BindingFlags.Instance;
            // enumerate the properties of the type
            foreach (PropertyInfo p in t.GetProperties(flags))
            {
                Type pt = p.PropertyType;

                if (p.PropertyType.Module.ScopeName != "CommonLanguageRuntimeLibrary")
                {
                    foreach (var pName in GetPropertiesRecursive(p.PropertyType, prefix))
                    {
                        yield return pName;
                    }
                }
                else
                {
                    if (pt.Name == "List`1")
                    {
                        Type genericType = pt.GetGenericArguments()[0];
                        // then enumerate the generic subtype
                        yield return prefix + p.Name + "|" + genericType.Name;
                        /*            	foreach (string propertyName in GetPropertiesRecursive(genericType, prefix))
                                        {
                                            yield return propertyName;
                                        } */
                    }
                    else
                    {
                        yield return prefix + p.Name;
                    }
                }
            }
        }

        /// <summary>
        /// Returns a list of properties for the given type. Does not drill into List&lt;object&gt; instances.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetProperties(Type t)
        {
            var flags = BindingFlags.Public | BindingFlags.Instance;
            // enumerate the properties of the type
            foreach (PropertyInfo p in t.GetProperties(flags))
            {
                Type pt = p.PropertyType;

                if (p.PropertyType.Module.ScopeName != "CommonLanguageRuntimeLibrary")
                {
                    foreach (var propName in GetPropertiesRecursive(p.PropertyType, p.Name))
                    {
                        yield return propName;
                    }
                }
                else
                {
                    if (pt.Name == "List`1")
                    {
                        Type genericType = pt.GetGenericArguments()[0];
                        // then enumerate the generic subtype
                        yield return p.Name; // + "|" + genericType.Name;
                    }
                    else
                    {
                        yield return p.Name;
                    }
                }
            }
        }


    }
}

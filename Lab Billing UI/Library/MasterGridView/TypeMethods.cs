using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.MasterGridView
{
    public class TypeMethods
    {
        public static string GetDescriptionFromMemberInfo(MemberInfo mi)
        {
            var descriptions = (DescriptionAttribute[])mi.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (descriptions.Length == 0)
            {
                return null;
            }
            return descriptions[0].Description;
        }

        public static string GetDescriptionFromFieldInfo(FieldInfo fi)
        {
            var descriptions = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (descriptions.Length == 0)
            {
                return null;
            }
            return descriptions[0].Description;
        }

        public static string GetDescriptionFromPropertyInfo(PropertyInfo pi)
        {
            var descriptions = (DescriptionAttribute[])pi.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (descriptions.Length == 0)
            {
                return null;
            }
            return descriptions[0].Description;
        }


        public static string GetDescriptionFromType(Type type)
        {
            var descriptions = (DescriptionAttribute[])type.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (descriptions.Length == 0)
            {
                return null;
            }
            return descriptions[0].Description;
        }

        /// <summary>
        /// Determine the type of a list
        /// </summary>
        /// <param name="myList"></param>
        /// <returns></returns>
        public static Type HeuristicallyDetermineType(IEnumerable myList)
        {
            var enumerable_type =
                myList.GetType()
                .GetInterfaces()
                .Where(i => i.IsGenericType && i.GenericTypeArguments.Length == 1)
                .FirstOrDefault(i => i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

            if (enumerable_type != null)
                return enumerable_type.GenericTypeArguments[0];

            IEnumerator enumerator = myList.GetEnumerator();
            if (enumerator.MoveNext() == false)
                return null;

            return enumerator.Current.GetType();
        }
    }
}

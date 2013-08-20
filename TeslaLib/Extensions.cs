using System;
using System.Linq;
using System.Runtime.Serialization;

namespace TeslaLib
{
    public static class Extensions
    {
        public static string GetEnumValue(this Enum enumValue)
        {
            //Look for DescriptionAttributes on the enum field
            var attr = enumValue.GetType().GetField(enumValue.ToString())
                .GetCustomAttributes(typeof(EnumMemberAttribute), false);

            if (attr.Length > 0) // a DescriptionAttribute exists; use it
                return ((EnumMemberAttribute)attr[0]).Value;
           
            var result = enumValue.ToString();

            return result;
        }

        public static T ToEnum<T>(string str)
        {
            var enumType = typeof(T);
            foreach (var name in Enum.GetNames(enumType))
            {
                var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
                if (enumMemberAttribute.Value == str) return (T)Enum.Parse(enumType, name);
            }
            

            return default(T);
        }

    }
}

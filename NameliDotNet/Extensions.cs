using System;
using System.Reflection;
using System.ComponentModel;

namespace NameliDotNet
{
    internal static class Extensions
    {
        #region Enum extension methods
        /// <summary>
        /// Retrieves the string value associated with an enum.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name == null) return null;

            FieldInfo field = type.GetField(name);
            if (field == null) return null;

            DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            return (attribute != null) ? attribute.Description : null;
        }
        #endregion

        #region String extension methods
        /// <summary>
        /// Escapes single apostrophes from a string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static string EscapeApostrophes(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return value;
            if (!value.Contains("'")) return value;
            return value.Replace("'", "''");
        }

        /// <summary>
        /// Remove escaped apostrophes from a string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static string UnescapeApostrophes(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return value;
            if (!value.Contains("''")) return value;
            return value.Replace("''", "'");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static string FirstCharToUpper(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return value;
            return char.ToUpper(value[0]) + value.Substring(1);
        }
        #endregion
    }
}

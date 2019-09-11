using System;
using System.ComponentModel;

namespace Resistance
{
    public static class EnumHelper
    {
        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            return GetDescription(value as object);
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string ToCommand(this Enum value)
        {
            return "/" + value.ToString().ToLower();
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private static string GetDescription(object value)
        {
            if (value == null)
                return "";

            var fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo == null)
                return "";

            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

    }
}
using System;
using System.ComponentModel;
using Resistance.Enums;
using Resistance.Helpers.Attributes;

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
        public static CommandType GetCommandType(this Enum value)
        {
            return GetCommandType(value as object);
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

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private static CommandType GetCommandType(object value)
        {
            if (value == null)
                return CommandType.Any;

            var fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo == null)
                return CommandType.Any;

            var attributes = (PublicityAttribute[])fieldInfo.GetCustomAttributes(typeof(PublicityAttribute), false);

            return attributes.Length > 0 ? attributes[0].Type : CommandType.Any;
        }
    }
}
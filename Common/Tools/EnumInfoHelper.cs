using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Reflection;

namespace Common.Tools
{
    /// <summary>
    /// enum class information service 
    /// </summary>
    public static class EnumInfoHelper
    {
        /// <summary>
        /// Get enum description 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDescription(object value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        /// <summary>
        /// Get enum display
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDisplayValue(object value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            if (fieldInfo == null) return string.Empty;

            var descriptionAttributes = fieldInfo.GetCustomAttributes(
                typeof(DisplayAttribute), false) as DisplayAttribute[];

            if (descriptionAttributes == null) return string.Empty;
            return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Name : value.ToString();
        }
    }
}
using System;
using System.ComponentModel;
using System.Reflection;

#if !NET20 && !NET35 && !NET40

using System.ComponentModel.DataAnnotations;

#endif

namespace SharpRambo.ExtensionsLib.EnumExtensions {

    public static class EnumExtensions {

        /// <summary>Gets an attribute on an enum field value</summary>
        /// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
        /// <param name="enumVal">The enum value</param>
        /// <returns>The attribute of type T that exists on the enum value</returns>
        /// <example><![CDATA[string desc = myEnumVariable.GetAttributeOfType<DescriptionAttribute>().Description;]]></example>
        public static T GetAttributeOfType<T>(this Enum enumVal) where T : Attribute {
            Type enumT = enumVal.GetType();
            string memberName = Enum.GetName(enumT, enumVal);

            if (memberName.IsNull())
                return default;

            MemberInfo[] memberInfo = enumT.GetMember(memberName);

            if (memberInfo.Length == 0)
                return null;

            object[] attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);

            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }

        /// <summary>Gets the description attribute on an enum field value</summary>
        /// <param name="enumVal">The enum value</param>
        /// <returns>The value of the description attribute if exists on the enum value otherwise the name of the enum value</returns>
        /// <example><![CDATA[string desc = myEnumVariable.GetDescription();]]></example>
        public static string GetDescription(this Enum enumVal) {
            DescriptionAttribute attr = enumVal.GetAttributeOfType<DescriptionAttribute>();
            return attr == null ? Enum.GetName(enumVal.GetType(), enumVal) : attr.Description;
        }

        /// <summary>Gets the category attribute on an enum field value</summary>
        /// <param name="enumVal">The enum value</param>
        /// <returns>The value of the category attribute if exists on the enum value otherwise an empty string</returns>
        /// <example><![CDATA[string desc = myEnumVariable.GetDescription();]]></example>
        public static string GetCategory(this Enum enumVal) {
            CategoryAttribute attr = enumVal.GetAttributeOfType<CategoryAttribute>();
            return attr == null ? string.Empty : attr.Category;
        }

#if !NET20 && !NET35 && !NET40

        /// <summary>Gets the display attribute on an enum field value</summary>
        /// <param name="enumVal">The enum value</param>
        /// <returns>The display attribute if exists on the enum value otherwise an empty string</returns>
        /// <example><![CDATA[string desc = myEnumVariable.GetDisplay();]]></example>
        public static DisplayAttribute GetDisplay(this Enum enumVal) {
            DisplayAttribute attr = enumVal.GetAttributeOfType<DisplayAttribute>();
            return attr ?? null;
        }

#endif

        public static string GetName(this Enum enumVal)
            => Enum.GetName(enumVal.GetType(), enumVal);

        public static EnumFieldEntry GetFieldEntry(this Enum enumVal)
#if NET5_0_OR_GREATER
            => new(enumVal);

#else
            => new EnumFieldEntry(enumVal);
#endif
    }
}

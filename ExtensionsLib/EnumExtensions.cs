using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

#if !NET20
using System.Threading.Tasks;
#endif

#if !NET20 && !NET35 && !NET40
using System.ComponentModel.DataAnnotations;
#endif

namespace SharpRambo.ExtensionsLib
{
    public static class EnumExtensions
    {
        /// <summary>Gets an attribute on an enum field value</summary>
        /// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
        /// <param name="enumVal">The enum value</param>
        /// <returns>The attribute of type T that exists on the enum value</returns>
        /// <example><![CDATA[string desc = myEnumVariable.GetAttributeOfType<DescriptionAttribute>().Description;]]></example>
        public static T GetAttributeOfType<T>(this Enum enumVal) where T:Attribute
        {
            Type enumT = enumVal.GetType();
            MemberInfo[] memberInfo = enumT.GetMember(Enum.GetName(enumT, enumVal));
            object[] attributes = memberInfo.First().GetCustomAttributes(typeof(T), false);

            return (attributes.Length > 0) ? (T)attributes.First() : null;
        }

        /// <summary>Gets the description attribute on an enum field value</summary>
        /// <param name="enumVal">The enum value</param>
        /// <returns>The value of the description attribute if exists on the enum value otherwise the name of the enum value</returns>
        /// <example><![CDATA[string desc = myEnumVariable.GetDescription();]]></example>
        public static string GetDescription(this Enum enumVal)
        {
            DescriptionAttribute attr = enumVal.GetAttributeOfType<DescriptionAttribute>();
            return attr == null ? Enum.GetName(enumVal.GetType(), enumVal) : attr.Description;
        }

        /// <summary>Gets the category attribute on an enum field value</summary>
        /// <param name="enumVal">The enum value</param>
        /// <returns>The value of the category attribute if exists on the enum value otherwise an empty string</returns>
        /// <example><![CDATA[string desc = myEnumVariable.GetDescription();]]></example>
        public static string GetCategory(this Enum enumVal)
        {
            CategoryAttribute attr = enumVal.GetAttributeOfType<CategoryAttribute>();
            return attr == null ? string.Empty : attr.Category;
        }

#if !NET20 && !NET35 && !NET40
        /// <summary>Gets the display attribute on an enum field value</summary>
        /// <param name="enumVal">The enum value</param>
        /// <returns>The display attribute if exists on the enum value otherwise an empty string</returns>
        /// <example><![CDATA[string desc = myEnumVariable.GetDescription();]]></example>
        public static DisplayAttribute GetDisplay(this Enum enumVal)
        {
            DisplayAttribute attr = enumVal.GetAttributeOfType<DisplayAttribute>();
            return attr ?? null;
        }
#endif

        public static string GetName(this Enum enumVal)
            => Enum.GetName(enumVal.GetType(), enumVal);

        public static EnumX.EnumFieldEntry GetFieldEntry(this Enum enumVal)
#if NET5_0_OR_GREATER
            => new(enumVal);
#else
            => new EnumX.EnumFieldEntry(enumVal);
#endif
    }

    public static class EnumX
    {
        public class EnumCollection : List<Enum>
        {
            public EnumCollection() : base() { }
            public EnumCollection(int capacity) : base(capacity) { }
            public EnumCollection(IEnumerable<Enum> collection) : base(collection) { }
        }

        public class ParsedEnumCollection : Dictionary<Type, EnumFieldEntryCollection>
        {
            public ParsedEnumCollection() : base() { }
            public ParsedEnumCollection(int capacity) : base(capacity) { }
            public ParsedEnumCollection(IEqualityComparer<Type> comparer) : base(comparer) { }
            public ParsedEnumCollection(IDictionary<Type, EnumFieldEntryCollection> dictionary) : base(dictionary) { }
            public ParsedEnumCollection(int capacity, IEqualityComparer<Type> comparer) : base(capacity, comparer) { }
            public ParsedEnumCollection(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
            public ParsedEnumCollection(IDictionary<Type, EnumFieldEntryCollection> dictionary, IEqualityComparer<Type> comparer) : base(dictionary, comparer) { }

            public void Add(Enum @enum) => Add(@enum.GetType());
            public void Add<TEnum>() where TEnum : Enum => Add(typeof(TEnum));
            public void Add(Type enumType)
            {
                EnumFieldEntryCollection eFEC = GetFieldEntries(enumType);

                if (this.ContainsKey(enumType))
                    this[enumType] = eFEC;
                else
                    this.Add(enumType, eFEC);
            }

#if !NET20
            public async Task AddRangeAsync(params Enum[] enums) => await AddRangeAsync(new EnumCollection(enums));
            public async Task AddRangeAsync(EnumCollection enumCollection)
            {
                if (enumCollection.IsNull())
                    throw new ArgumentNullException(nameof(enumCollection));

                await enumCollection.ForEachAsync(e => {
                    Add(e);
#if NET35 || NET40
                    return GenericExtensions.GetCompletedTask();
#elif NET45
                    return Task.FromResult(0);
#else
                    return Task.CompletedTask;
#endif
                });
            }

            public async Task AddRangeAsync(params Type[] enumTypes) => await AddRangeAsync(enumTypes?.ToList());
            public async Task AddRangeAsync(IEnumerable<Type> enumTypeCollection)
            {
                if (enumTypeCollection.IsNull())
                    throw new ArgumentNullException(nameof(enumTypeCollection));

                await enumTypeCollection.ForEachAsync(eT => {
                    Add(eT);

#if NET35 || NET40
                    return GenericExtensions.GetCompletedTask();
#elif NET45
                    return Task.FromResult(0);
#else
                    return Task.CompletedTask;
#endif
                });
            }
#endif

            public void AddRange(params Enum[] enums) => AddRange(new EnumCollection(enums));
            public void AddRange(EnumCollection enumCollection)
            {
                foreach (Enum e in enumCollection)
                    Add(e);
            }

            public void AddRange(params Type[] enumTypes) => AddRange(enumTypes?.ToList());
            public void AddRange(IEnumerable<Type> enumTypeCollection)
            {
                foreach (Type eT in enumTypeCollection)
                    Add(eT);
            }
        }

        public class EnumFieldEntry
        {
            public string Name { get; private set; }
            public Enum EnumObject { get; private set; }
            public string Description { get; private set; }
            public string Category { get; private set; }
            public string DisplayName { get; set; }

#if !NET20 && !NET35 && !NET40
            public DisplayAttribute Display { get; private set; }
#endif

            public EnumFieldEntry(Enum enumVal) : this(enumVal.GetName(), enumVal.GetDescription(), enumVal) { }
            public EnumFieldEntry(string name, string description, Enum enumVal)
            {
                if (name.IsNull())
                    throw new ArgumentNullException(nameof(name));

                EnumObject = enumVal ?? throw new ArgumentNullException(nameof(enumVal));

                Name = name;
                Description = description;
                DisplayName = name;
                Category = enumVal.GetCategory();

#if !NET20 && !NET35 && !NET40
                Display = enumVal.GetDisplay();

                if (Display != null && !Display.Description.IsNull() && description.IsNull())
                    Description = Display.Description;
#endif
            }
        }

#if NET20
        public class EnumFieldEntryCollection : List<EnumFieldEntry>
#else
        public class EnumFieldEntryCollection : ObservableCollection<EnumFieldEntry>
#endif
        {
            public EnumFieldEntryCollection() : base() { }
#if NET20
            public EnumFieldEntryCollection(int capacity) : base(capacity) { }
#else
            public EnumFieldEntryCollection(List<EnumFieldEntry> list) : base(list) { }
#endif
            public EnumFieldEntryCollection(IEnumerable<EnumFieldEntry> collection) : base(collection) { }
        }

        public static EnumFieldEntryCollection GetFieldEntries<T>() where T:Enum => GetFieldEntries(typeof(T));
        public static EnumFieldEntryCollection GetFieldEntries(Enum @enum) => GetFieldEntries(@enum.GetType());
        public static EnumFieldEntryCollection GetFieldEntries(Type enumType)
        {
            if (enumType == null)
                throw new ArgumentNullException(nameof(enumType));

            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum", nameof(enumType));

            EnumFieldEntryCollection enumFieldEntries =
#if NET5_0_OR_GREATER
                new();
#else
                new EnumFieldEntryCollection();
#endif

#if NET20 || NET35
            IEnumerable<object> vals = Enum.GetValues(enumType).Cast<object>();
#else
            IEnumerable<object> vals = enumType.GetEnumValues().Cast<object>();
#endif
            IOrderedEnumerable<object> orderedVals = vals.OrderBy(v => int.TryParse(Convert.ToString(v), out int o) ? o : int.MaxValue);

            foreach (object enumVal in orderedVals)
                enumFieldEntries.Add(new EnumFieldEntry((Enum)enumVal));

            return enumFieldEntries;
        }
    }
}

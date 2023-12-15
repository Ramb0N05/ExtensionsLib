using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpRambo.ExtensionsLib.EnumExtensions {

    public static class EnumX {

        public static EnumFieldEntryCollection GetFieldEntries<T>() where T : Enum => GetFieldEntries(typeof(T));

        public static EnumFieldEntryCollection GetFieldEntries(Enum @enum) => GetFieldEntries(@enum.GetType());

        public static EnumFieldEntryCollection GetFieldEntries(Type enumType) {
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

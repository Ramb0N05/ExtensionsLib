using System.Collections.Generic;

#if !NET20

using System.Collections.ObjectModel;

#endif

namespace SharpRambo.ExtensionsLib.EnumExtensions {
#if NET20
    public class EnumFieldEntryCollection : List<EnumFieldEntry>
#else

    public class EnumFieldEntryCollection : ObservableCollection<EnumFieldEntry>
#endif
    {
        public EnumFieldEntryCollection() {
        }

#if NET20
        public EnumFieldEntryCollection(int capacity) : base(capacity) { }
#else

        public EnumFieldEntryCollection(List<EnumFieldEntry> list) : base(list) {
        }

#endif

        public EnumFieldEntryCollection(IEnumerable<EnumFieldEntry> collection) : base(collection) {
        }
    }
}

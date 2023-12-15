using System;
using System.Collections.Generic;

namespace SharpRambo.ExtensionsLib.EnumExtensions {

    public class EnumCollection : List<Enum> {

        public EnumCollection() : base() {
        }

        public EnumCollection(int capacity) : base(capacity) {
        }

        public EnumCollection(IEnumerable<Enum> collection) : base(collection) {
        }
    }
}

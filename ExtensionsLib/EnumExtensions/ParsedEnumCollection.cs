using System;
using System.Collections.Generic;
using System.Linq;

#if !NET20

using System.Threading.Tasks;

#endif

namespace SharpRambo.ExtensionsLib.EnumExtensions {
#if !NET8_0_OR_GREATER
    [Serializable]
#endif

    public class ParsedEnumCollection : Dictionary<Type, EnumFieldEntryCollection> {

        #region Public Constructors

        public ParsedEnumCollection() : base() {
        }

        public ParsedEnumCollection(int capacity) : base(capacity) {
        }

        public ParsedEnumCollection(IEqualityComparer<Type> comparer) : base(comparer) {
        }

        public ParsedEnumCollection(IDictionary<Type, EnumFieldEntryCollection> dictionary) : base(dictionary) {
        }

        public ParsedEnumCollection(int capacity, IEqualityComparer<Type> comparer) : base(capacity, comparer) {
        }

#if !NET8_0_OR_GREATER
        protected ParsedEnumCollection(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) {
        }
#endif

        public ParsedEnumCollection(IDictionary<Type, EnumFieldEntryCollection> dictionary, IEqualityComparer<Type> comparer) : base(dictionary, comparer) {
        }

        #endregion Public Constructors

        #region Public Methods

        public void Add(Enum @enum) => Add(@enum.GetType());

        public void Add<TEnum>() where TEnum : Enum => Add(typeof(TEnum));

        public void Add(Type enumType) {
            EnumFieldEntryCollection eFEC = EnumX.GetFieldEntries(enumType);

            if (ContainsKey(enumType))
                this[enumType] = eFEC;
            else
                Add(enumType, eFEC);
        }

        public void AddRange(params Enum[] enums) => AddRange(new EnumCollection(enums));

        public void AddRange(EnumCollection enumCollection) {
            foreach (Enum e in enumCollection)
                Add(e);
        }

        public void AddRange(params Type[] enumTypes) => AddRange(enumTypes?.ToList());

        public void AddRange(IEnumerable<Type> enumTypeCollection) {
            foreach (Type eT in enumTypeCollection)
                Add(eT);
        }

#if !NET20

        public async Task AddRangeAsync(params Enum[] enums) => await AddRangeAsync(new EnumCollection(enums));

        public async Task AddRangeAsync(EnumCollection enumCollection) {
            if (enumCollection.IsNull())
                throw new ArgumentNullException(nameof(enumCollection));

            await enumCollection.ForEachAsync(async e => {
                Add(e);
                await AsyncExtensions.CompletedTask;
            });
        }

        public async Task AddRangeAsync(params Type[] enumTypes) => await AddRangeAsync(enumTypes?.ToList());

        public async Task AddRangeAsync(IEnumerable<Type> enumTypeCollection) {
            if (enumTypeCollection.IsNull())
                throw new ArgumentNullException(nameof(enumTypeCollection));

            await enumTypeCollection.ForEachAsync(async eT => {
                Add(eT);
                await AsyncExtensions.CompletedTask;
            });
        }

#endif
        #endregion Public Methods
    }
}

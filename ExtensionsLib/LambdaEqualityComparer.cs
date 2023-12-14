using System;
using System.Collections.Generic;

namespace SharpRambo.ExtensionsLib {

    /// <summary>The LambdaEqualityComparer class.</summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TComparable">The type of the comparable.</typeparam>
    /// <seealso cref="System.Collections.Generic.IEqualityComparer&lt;TSource&gt;" />
    public class LambdaEqualityComparer<TSource, TComparable> : IEqualityComparer<TSource> {
        private readonly Func<TSource, TComparable> _keyGetter;

        /// <summary>Initializes a new instance of the <see cref="LambdaEqualityComparer{TSource, TComparable}"/> class.</summary>
        /// <param name="keyGetter">The key getter.</param>
        public LambdaEqualityComparer(Func<TSource, TComparable> keyGetter) {
            _keyGetter = keyGetter;
        }

        /// <summary>Determines whether the specified objects are equal.</summary>
        /// <param name="x">The first object of type <typeparamref name="TSource"/> to compare.</param>
        /// <param name="y">The second object of type <typeparamref name="TSource" /> to compare.</param>
        /// <returns><see langword="true" /> if the specified objects are equal; otherwise, <see langword="false" />.</returns>
        public bool Equals(TSource x, TSource y)
            => x == null || y == null ? x == null && y == null : Equals(_keyGetter(x), _keyGetter(y));

        /// <summary>Returns a hash code for this instance.</summary>
        /// <param name="obj">The object.</param>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public int GetHashCode(TSource obj) {
            TComparable k = _keyGetter(obj);

            return obj == null
                ? int.MinValue
                : (k == null
                    ? int.MaxValue
                    : k.GetHashCode());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

#if !NET20

using System.Threading.Tasks;

#endif

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER

using System.Diagnostics.CodeAnalysis;

#endif

namespace SharpRambo.ExtensionsLib {

    /// <summary>
    /// The NullCheckExtensions class.
    /// </summary>
    public static class NullCheckExtensions {

        #region String null check

        /// <summary>Determines whether this instance is null or an empty string.</summary>
        /// <param name="value">The value.</param>
        /// <returns><see langword="True"/> if the specified value is null, otherwise <see langword="False"/>.</returns>
        public static bool IsNull(
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
            [NotNullWhen(false)]
#endif
        this string value)

#if NETCOREAPP1_0_OR_GREATER || NETSTANDARD1_0_OR_GREATER || NET40_OR_GREATER

            => string.IsNullOrWhiteSpace(value);

#else

            => string.IsNullOrEmpty(value) || value.Trim().Length == 0;

#endif
        #endregion String null check

        #region Collections null check

        /// <summary>Determines whether this Array is null or empty.</summary>
        /// <param name="array">The array.</param>
        /// <param name="recursive">Specifies whether the array should be checked recursively.</param>
        /// <returns><see langword="True"/> if the specified value is null, otherwise <see langword="False"/>.</returns>
        public static bool IsNull(
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
            [NotNullWhen(false)]
#endif
        this Array array, bool recursive = true) {
            bool onlyEmptyItems = true;

            if (recursive && array?.Length > 0)
                foreach (object item in array)
                    if (item != null)
                        onlyEmptyItems = false;

            return array == null || array.Length == 0 || onlyEmptyItems;
        }

        /// <summary>Determines whether this Enumerable is null or empty.</summary>
        /// <param name="collection">The collection.</param>
        /// <param name="recursive">Specifies whether the collection should be checked recursively.</param>
        /// <returns><see langword="True"/> if the specified value is null, otherwise <see langword="False"/>.</returns>
        public static bool IsNull<TSource>(
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
            [NotNullWhen(false)]
#endif
        this IEnumerable<TSource> collection, bool recursive = true) {
            bool onlyEmptyItems = true;

            if (recursive && collection?.Any() == true)
                foreach (TSource item in collection)
                    if (item != null)
                        onlyEmptyItems = false;

            return collection?.Any() != true || onlyEmptyItems;
        }

        #endregion Collections null check

#if !NET20

        #region Async collections null check

        /// <summary><inheritdoc cref="IsNull(Array, bool)"/></summary>
        /// <param name="array">The array.</param>
        /// <param name="recursive">Specifies whether the array should be checked recursively.</param>
        /// <returns><inheritdoc cref="IsNull(Array, bool)"/></returns>
        public static async Task<bool> IsNullAsync(
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
            [NotNullWhen(false)]
#endif
        this Array array, bool recursive = true) {
            bool onlyEmptyItems = true;

            if (recursive && array?.Length > 0)
                await array.ForEachAsync(async item => {
                    if (item != null)
                        onlyEmptyItems = false;

                    await AsyncExtensions.CompletedTask;
                });

            return array == null || array.Length == 0 || onlyEmptyItems;
        }

        /// <summary><inheritdoc cref="IsNull{TSource}(IEnumerable{TSource}, bool)"/></summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="recursive">if set to <c>true</c> [recursive].</param>
        /// <returns><inheritdoc cref="IsNull{TSource}(IEnumerable{TSource}, bool)"/></returns>
        public static async Task<bool> IsNullAsync<TSource>(
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
            [NotNullWhen(false)]
#endif
        this IEnumerable<TSource> collection, bool recursive = true) {
            bool onlyEmptyItems = true;

            if (recursive && collection?.Any() == true)
                await collection.ForEachAsync(async item => {
                    if (item != null)
                        onlyEmptyItems = false;

                    await AsyncExtensions.CompletedTask;
                });

            return collection?.Any() != true || onlyEmptyItems;
        }

        #endregion Async collections null check

#endif
    }
}

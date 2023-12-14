using System;
using System.Collections.Generic;
using System.Linq;

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER

using System.Diagnostics.CodeAnalysis;

#endif

namespace SharpRambo.ExtensionsLib {

    /// <summary>The NullCheckExtensions class.</summary>
    public static class NullCheckExtensions {

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

        /// <summary>Determines whether this Enumerable is null or empty.</summary>
        /// <param name="value">The value.</param>
        /// <param name="recursive">Specifies whether the collection should be checked recursively.</param>
        /// <returns><see langword="True"/> if the specified value is null, otherwise <see langword="False"/>.</returns>
        public static bool IsNull<TSource>(
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
            [NotNullWhen(false)]
#endif
        this IEnumerable<TSource> value, bool recursive = true) {
            bool onlyEmptyItems = true;

            if (recursive && value?.Any() == true)
                foreach (TSource item in value)
                    if (item != null)
                        onlyEmptyItems = false;

            return value?.Any() != true || onlyEmptyItems;
        }

        /// <summary>Determines whether this Array is null or empty.</summary>
        /// <param name="value">The value.</param>
        /// <param name="recursive">Specifies whether the array should be checked recursively.</param>
        /// <returns><see langword="True"/> if the specified value is null, otherwise <see langword="False"/>.</returns>
        public static bool IsNull(
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
            [NotNullWhen(false)]
#endif
        this Array value, bool recursive = true) {
            bool onlyEmptyItems = true;

            if (recursive && value?.Length > 0)
                foreach (object item in value)
                    if (item != null)
                        onlyEmptyItems = false;

            return value == null || value.Length == 0 || onlyEmptyItems;
        }
    }
}

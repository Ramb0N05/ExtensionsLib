using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpRambo.ExtensionsLib
{
    /// <summary>The LambdaEqualityComparerExtensions class.</summary>
    public static class LambdaEqualityComparerExtensions
    {
        /// <summary><inheritdoc cref="Enumerable.Except{TSource}(IEnumerable{TSource}, IEnumerable{TSource}, IEqualityComparer{TSource}?)" path="/summary/node()"/></summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TComparable">The type of the comparable.</typeparam>
        /// <param name="source1">The source1.</param>
        /// <param name="source2">The source2.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <returns><inheritdoc cref="Enumerable.Except{TSource}(IEnumerable{TSource}, IEnumerable{TSource}, IEqualityComparer{TSource}?)" path="/returns/node()"/></returns>
        public static IEnumerable<TSource> Except<TSource, TComparable>(
            this IEnumerable<TSource> source1,
            IEnumerable<TSource> source2,
            Func<TSource, TComparable> keySelector)
            => source1.Except(source2, new LambdaEqualityComparer<TSource, TComparable>(keySelector));

        /// <summary><inheritdoc cref="Enumerable.Union{TSource}(IEnumerable{TSource}, IEnumerable{TSource}, IEqualityComparer{TSource}?)" path="/summary/node()"/></summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TComparable">The type of the comparable.</typeparam>
        /// <param name="source1">The source1.</param>
        /// <param name="source2">The source2.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <returns><inheritdoc cref="Enumerable.Union{TSource}(IEnumerable{TSource}, IEnumerable{TSource}, IEqualityComparer{TSource}?)" path="/returns/node()"/></returns>
        public static IEnumerable<TSource> Union<TSource, TComparable>(
            this IEnumerable<TSource> source1,
            IEnumerable<TSource> source2,
            Func<TSource, TComparable> keySelector)
            => source1.Union(source2, new LambdaEqualityComparer<TSource, TComparable>(keySelector));
    }
}

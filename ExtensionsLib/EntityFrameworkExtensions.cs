﻿#if !NET20

using System;
using System.Linq;
using System.Linq.Expressions;

#if !NET35 && !NET40 && !NET5_0 && !NETCOREAPP2_1 && !NETSTANDARD2_0
#if NET6_0_OR_GREATER

using Microsoft.EntityFrameworkCore;

#else

using System.Data.Entity;

#endif
#endif

namespace SharpRambo.ExtensionsLib {

    /// <summary>
    /// The EntityFrameworkExtensions class.
    /// </summary>
    public static class EntityFrameworkExtensions {

        public static int MaxOrDefault<T>(this IQueryable<T> source, Expression<Func<T, int?>> selector, int nullValue = 0)
            => source.Max(selector) ?? nullValue;

#if !NET20 && !NET35 && !NET40 && !NET5_0 && !NETCOREAPP2_1 && !NETSTANDARD2_0

        public static async System.Threading.Tasks.Task<int> MaxOrDefaultAsync<T>(this IQueryable<T> source, Expression<Func<T, int?>> selector, int nullValue = 0)
            => (await source.MaxAsync(selector)) ?? nullValue;

#endif
    }
}

#endif

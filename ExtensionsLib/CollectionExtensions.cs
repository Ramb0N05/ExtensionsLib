using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#if !NET20 && !NET35 && !NET40
using System.Threading.Tasks;
#endif

namespace SharpRambo.ExtensionsLib
{
    /// <summary>The CollectionExtensions class.</summary>
    public static class CollectionExtensions
    {
#if !NET20 && !NET35 && !NET40
        public static async Task ForEachAsync<T>(this IEnumerable<T> list, Func<T, Task> func)
        {
            foreach (T value in list)
                await func(value).ConfigureAwait(false);
        }

        public static async Task<TResult> ForEachAsync<T, TResult>(this IEnumerable<T> list, Func<T, Task<T>> func)
            where TResult : IList<T>, new()
        {
            TResult result =
#if NET5_0_OR_GREATER
                new();
#else
                new TResult();
#endif

            foreach (T value in list)
                result.Add(await func(value).ConfigureAwait(false));

            return result;
        }
#endif

        /// <summary>Adds each list item to a <see langword="string"/> and seperates it with commata and whitespaces by default. Behaviour is configurable.</summary>
        /// <param name="list">The list that will be converted.</param>
        /// <param name="seperator">The seperator char to use. Default is <c>','</c>.</param>
        /// <param name="addTrailingWhitespaces">Determines whether to add whitespaces after each seperator. Default is <see langword="True"/>.</param>
        /// <returns>The seperated string of the list.</returns>
        public static string ToSeparatedString(this List<string> list, char seperator = ',', bool addTrailingWhitespaces = true)
        {
            string result = string.Empty;

            if (!list.IsNull())
                foreach (string item in list)
                    result += item + (
#if NETCOREAPP3_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
                        item != list[^1]
#else
                        item != list[list.Count - 1]
#endif
                            ? seperator + (addTrailingWhitespaces
                                ? " "
                                : string.Empty)
                            : string.Empty);

            return result;
        }

        /// <summary>
        /// Adds each list item to a <see langword="string"/> (<c><![CDATA['<ValueName>=<Value>']]></c>) and seperates it with commata and whitespaces by default. Behaviour is configurable.<br/>
        /// The objects are Parsed to a <see langword="string"/> with <see cref="GenericExtensions.ParseToString(object, Type, bool)"/>.
        /// </summary>
        /// <param name="dict">The dictionary that will be converted.</param>
        /// <param name="seperator">The seperator char to use. Default is <c>','</c>.</param>
        /// <param name="addTrailingWhitespaces">Determines whether to add whitespaces after each seperator. Default is <see langword="True"/>.</param>
        /// <returns>The seperated string of the dictionary.</returns>
        public static string ToSeparatedString(this Dictionary<string, KeyValuePair<object, Type>> dict, char seperator = ',', bool addTrailingWhitespaces = true)
        {
            string result = string.Empty;

            if (!dict.IsNull()) {
                string[] myKeys = new string[dict.Count];
                dict.Keys.CopyTo(myKeys, 0);

                string lastKey =
#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
                    myKeys[^1];
#else
                    myKeys[myKeys.Length - 1];
#endif

                foreach (KeyValuePair<string, KeyValuePair<object, Type>> item in dict)
                    result += item.Key + "=" + item.Value.Key.ParseToString(item.Value.Value) +
                                (item.Key != lastKey ? seperator + (addTrailingWhitespaces ? " " : string.Empty) : string.Empty);
            }

            return result;
        }

        /// <summary>Adds each key item to a <see langword="string"/> and seperates it with commata and whitespaces by default. Behaviour is configurable.</summary>
        /// <param name="dict">The dictionary from which the keys are converted.</param>
        /// <param name="seperator">The seperator char to use. Default is <c>','</c>.</param>
        /// <param name="addTrailingWhitespaces">Determines whether to add whitespaces after each seperator. Default is <see langword="True"/>.</param>
        /// <returns></returns>
        public static string KeysToSeparatedString(this Dictionary<string, KeyValuePair<object, Type>> dict, char seperator = ',', bool addTrailingWhitespaces = true)
            => !dict.IsNull() ? new List<string>(dict.Keys).ToSeparatedString(seperator, addTrailingWhitespaces) : string.Empty;

        /// <summary>Adds each value item to a <see langword="string"/> and seperates it with commata and whitespaces by default. Behaviour is configurable.</summary>
        /// <param name="dict">The dictionary from which the values are converted.</param>
        /// <param name="seperator">The seperator char to use. Default is <c>','</c>.</param>
        /// <param name="addTrailingWhitespaces">Determines whether to add whitespaces after each seperator. Default is <see langword="True"/>.</param>
        /// <returns></returns>
        public static string ValuesToSeparatedString(this Dictionary<string, KeyValuePair<object, Type>> dict, char seperator = ',', bool addTrailingWhitespaces = true)
        {
            if (!dict.IsNull()) {
                List<string> values =
#if NET5_0_OR_GREATER
                    new();
#else
                    new List<string>();
#endif

                foreach (KeyValuePair<object, Type> value in dict.Values)
                    values.Add(value.Key.ParseToString(value.Value));

                return values.ToSeparatedString(seperator, addTrailingWhitespaces);
            } else
                return string.Empty;
        }

        /// <summary><inheritdoc cref="Dictionary{TKey, TValue}.Add(TKey, TValue)" path="/summary/node()"/></summary>
        /// <param name="dict">The dictionary to which key-value pairs are added.</param>
        /// <param name="key">The name of the value.</param>
        /// <param name="value">The value.</param>
        /// <param name="valueType">The <see cref="Type"/> of the value. If null it defaults to <c>typeof(<see cref="string"/>)</c>.</param>
        public static void Add(this Dictionary<string, KeyValuePair<object, Type>> dict, string key, object value = null, Type valueType = null)
            => dict.Add(key, new KeyValuePair<object, Type>(value, valueType ?? typeof(string)));

        /// <summary>Adds the specified key and <see cref="KeyValuePair"/> to the dictionary.</summary>
        /// <typeparam name="TDictKey">The type of the dictionary key.</typeparam>
        /// <typeparam name="TKVPKey">The type of the <see cref="KeyValuePair"/> key.</typeparam>
        /// <typeparam name="TKVPValue">The type of the <see cref="KeyValuePair"/> value.</typeparam>
        /// <param name="dictKVP">The dictionary.</param>
        /// <param name="key">The key of the dictionary.</param>
        /// <param name="valueKey">The key of the <see cref="KeyValuePair"/>.</param>
        /// <param name="value">The value of the <see cref="KeyValuePair"/>.</param>
        public static void Add<TDictKey, TKVPKey, TKVPValue>(this Dictionary<TDictKey, KeyValuePair<TKVPKey, TKVPValue>> dictKVP, TDictKey key, TKVPKey valueKey, TKVPValue value)
            => dictKVP.Add(key, new KeyValuePair<TKVPKey, TKVPValue>(valueKey, value));

#if !NET20 && !NET35 && !NET40
        public static async Task<IList<T>> InitializeAsync<T>(this IEnumerable<T> source, IEnumerable<Task<T>> data)
            => await source.InitializeAsync<T, ObservableCollection<T>>(data);
        public static async Task<TDefault> InitializeAsync<T, TDefault>(this IEnumerable<T> source, IEnumerable<Task<T>> data)
            where TDefault : IList<T>, new()
        {
            if (source == null)
                source = new TDefault();

            if (!data.IsNull() && source is TDefault soc) {
                await data.ForEachAsync(async dataItem => soc.Add(await dataItem));
                return soc;
            } else
                return new TDefault();
        }
#endif
    }
}

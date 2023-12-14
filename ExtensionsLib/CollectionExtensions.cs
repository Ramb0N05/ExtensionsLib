using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

#if !NET20

using System.Threading.Tasks;

#endif

namespace SharpRambo.ExtensionsLib {

    /// <summary>
    /// The CollectionExtensions class.
    /// </summary>
    public static class CollectionExtensions {
#if !NET20

        public static async Task ForEachAsync<T>(this IEnumerable<T> list, Func<T, Task> func) {
            foreach (T value in list)
                await func(value).ConfigureAwait(false);
        }

        public static async Task<TResult> ForEachAsync<T, TResult>(this IEnumerable<T> list, Func<T, Task<T>> func)
            where TResult : IList<T>, new() {
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

        /// <summary>
        /// Adds each list item to a <see langword="string"/> and separates it with commas and
        /// whitespaces by default. Behavior is configurable.
        /// </summary>
        /// <param name="list">The list that will be converted.</param>
        /// <param name="separator">The separator char to use. Default is <c>','</c>.</param>
        /// <param name="addTrailingWhitespaces">
        /// Determines whether to add whitespaces after each separator. Default is <see langword="True"/>.
        /// </param>
        /// <returns>The separated string of the list.</returns>
        public static string ToSeparatedString(this List<string> list, char separator = ',', bool addTrailingWhitespaces = true) {
            string trailingWhitespace = addTrailingWhitespaces ? " " : string.Empty;
            StringBuilder result =
#if NET5_0_OR_GREATER
                new();
#else
                new StringBuilder();
#endif

            if (!list.IsNull())
                foreach (string item in list) {
                    result
                        .Append(item)
                        .Append(
#if NETCOREAPP3_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
                        item != list[^1]
#else
                        item != list[list.Count - 1]
#endif
                            ? separator + trailingWhitespace
                            : string.Empty);
                }

            return result.ToString();
        }

        /// <summary>
        /// Adds each list item to a <see langword="string"/> (
        /// <c><![CDATA['<ValueName>=<Value>']]></c>) and separates it with commas and whitespaces
        /// by default. Behavior is configurable. <br/> The objects are Parsed to a <see
        /// langword="string"/> with <see cref="GenericExtensions.ParseToString(object, Type, bool)"/>.
        /// </summary>
        /// <param name="dictionary">The dictionary that will be converted.</param>
        /// <param name="separator">The separator char to use. Default is <c>','</c>.</param>
        /// <param name="addTrailingWhitespaces">
        /// Determines whether to add whitespaces after each separator. Default is <see langword="True"/>.
        /// </param>
        /// <returns>The separated string of the dictionary.</returns>
        public static string ToSeparatedString(this Dictionary<string, KeyValuePair<object, Type>> dictionary, char separator = ',', bool addTrailingWhitespaces = true) {
            string trailingWhitespace = addTrailingWhitespaces ? " " : string.Empty;
            StringBuilder result =
#if NET5_0_OR_GREATER
                new();
#else
                new StringBuilder();
#endif

            if (!dictionary.IsNull()) {
                string[] myKeys = new string[dictionary.Count];
                dictionary.Keys.CopyTo(myKeys, 0);

                string lastKey =
#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
                    myKeys[^1];
#else
                    myKeys[myKeys.Length - 1];
#endif

                foreach (KeyValuePair<string, KeyValuePair<object, Type>> item in dictionary) {
                    result
                        .Append(item.Key)
                        .Append('=')
                        .Append(item.Value.Key.ParseToString(item.Value.Value))
                        .Append(item.Key != lastKey ? separator + trailingWhitespace : string.Empty);
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Adds each key item to a <see langword="string"/> and separates it with commas and
        /// whitespaces by default. Behavior is configurable.
        /// </summary>
        /// <param name="dictionary">The dictionary from which the keys are converted.</param>
        /// <param name="separator">The separator char to use. Default is <c>','</c>.</param>
        /// <param name="addTrailingWhitespaces">
        /// Determines whether to add whitespaces after each separator. Default is <see langword="True"/>.
        /// </param>
        /// <returns></returns>
        public static string KeysToSeparatedString(this Dictionary<string, KeyValuePair<object, Type>> dictionary, char separator = ',', bool addTrailingWhitespaces = true)
            => !dictionary.IsNull() ? new List<string>(dictionary.Keys).ToSeparatedString(separator, addTrailingWhitespaces) : string.Empty;

        /// <summary>
        /// Adds each value item to a <see langword="string"/> and separates it with commas and
        /// whitespaces by default. Behavior is configurable.
        /// </summary>
        /// <param name="dictionary">The dictionary from which the values are converted.</param>
        /// <param name="separator">The separator char to use. Default is <c>','</c>.</param>
        /// <param name="addTrailingWhitespaces">
        /// Determines whether to add whitespaces after each separator. Default is <see langword="True"/>.
        /// </param>
        /// <returns></returns>
        public static string ValuesToSeparatedString(this Dictionary<string, KeyValuePair<object, Type>> dictionary, char separator = ',', bool addTrailingWhitespaces = true) {
            if (!dictionary.IsNull()) {
                List<string> values =
#if NET5_0_OR_GREATER
                    new();
#else
                    new List<string>();
#endif

                foreach (KeyValuePair<object, Type> value in dictionary.Values)
                    values.Add(value.Key.ParseToString(value.Value));

                return values.ToSeparatedString(separator, addTrailingWhitespaces);
            } else
                return string.Empty;
        }

        /// <summary>
        /// <inheritdoc cref="Dictionary{TKey, TValue}.Add(TKey, TValue)" path="/summary/node()"/>
        /// </summary>
        /// <param name="dictionary">The dictionary to which key-value pairs are added.</param>
        /// <param name="key">The name of the value.</param>
        /// <param name="value">The value.</param>
        /// <param name="valueType">
        /// The <see cref="Type"/> of the value. If null it defaults to <c>typeof(<see cref="string"/>)</c>.
        /// </param>
        public static void Add(this Dictionary<string, KeyValuePair<object, Type>> dictionary, string key, object value = null, Type valueType = null)
            => dictionary.Add(key, new KeyValuePair<object, Type>(value, valueType ?? typeof(string)));

        /// <summary>
        /// Adds the specified key and <see cref="KeyValuePair"/> to the dictionary.
        /// </summary>
        /// <typeparam name="TDictionaryKey">The type of the dictionary key.</typeparam>
        /// <typeparam name="TKVPKey">The type of the <see cref="KeyValuePair"/> key.</typeparam>
        /// <typeparam name="TKVPValue">The type of the <see cref="KeyValuePair"/> value.</typeparam>
        /// <param name="dictionaryKVP">The dictionary.</param>
        /// <param name="key">The key of the dictionary.</param>
        /// <param name="valueKey">The key of the <see cref="KeyValuePair"/>.</param>
        /// <param name="value">The value of the <see cref="KeyValuePair"/>.</param>
        public static void Add<TDictionaryKey, TKVPKey, TKVPValue>(this Dictionary<TDictionaryKey, KeyValuePair<TKVPKey, TKVPValue>> dictionaryKVP, TDictionaryKey key, TKVPKey valueKey, TKVPValue value)
            => dictionaryKVP.Add(key, new KeyValuePair<TKVPKey, TKVPValue>(valueKey, value));

#if !NET20

        public static async Task<IList<T>> InitializeAsync<T>(this IEnumerable<T> source, IEnumerable<Task<T>> data)
            => await source.InitializeAsync<T, ObservableCollection<T>>(data);

        public static async Task<TDefault> InitializeAsync<T, TDefault>(this IEnumerable<T> source, IEnumerable<Task<T>> data)
            where TDefault : IList<T>, new() {
#if NET5_0_OR_GREATER
            source ??= new TDefault();
#else
            if (source == null)
                source = new TDefault();
#endif

            if (!data.IsNull() && source is TDefault soc) {
                await data.ForEachAsync(async dataItem => soc.Add(await dataItem));
                return soc;
            } else
                return new TDefault();
        }

#endif
    }
}

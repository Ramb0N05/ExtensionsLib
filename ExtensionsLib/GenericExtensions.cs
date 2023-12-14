using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.CodeAnalysis;

#if NET35 || NET40
using System.Threading.Tasks;
#endif

namespace SharpRambo.ExtensionsLib {

    /// <summary>The GenericExtensions class.</summary>
    public static class GenericExtensions {

        #region Object Extensions

        /// <summary>Parses an object of a given <see cref="Type"/> to a <see cref="string"/>.</summary>
        /// <remarks>
        /// <see cref="string"/>-Types are enclosed in single quotation marks if <paramref name="quoteStringValues"/> is set to <see langword="True"/>.<br/>
        /// <see cref="bool"/>-Types are converted to a <see cref="string"/> representation of an <see cref="int"/> (without quotation marks).<br/>
        /// Empty values are parsed as <c>"NULL".</c>
        /// </remarks>
        /// <param name="obj">The value as <see cref="object"/>.</param>
        /// <param name="ValueType">The <see cref="Type"/> of the value.</param>
        /// <param name="quoteStringValues">Specifies whether <see langword="string"/> values should be enclosed in quotation marks.</param>
        /// <returns>The parsed value as <see cref="string"/>.</returns>
        public static string ParseToString(this object obj, Type ValueType = null, bool quoteStringValues = true) {
            string quote = quoteStringValues ? "'" : string.Empty;

#if NET5_0_OR_GREATER

            return obj == null
                ? "NULL"
                : Type.GetTypeCode(ValueType) switch {
                    TypeCode.Char or TypeCode.String => quote + Convert.ToString(obj) + quote,
                    TypeCode.DateTime => quote + Convert.ToDateTime(obj).ToString() + quote,
                    TypeCode.Boolean => Convert.ToInt32(Convert.ToBoolean(obj)).ToString(),
                    TypeCode.Empty or TypeCode.DBNull => "NULL",
                    _ => Convert.ToString(obj),
                };

#else
            if (obj != null) {
                switch (Type.GetTypeCode(ValueType)) {
                    case TypeCode.Char:
                    case TypeCode.String:
                        return quote + Convert.ToString(obj) + quote;

                    case TypeCode.DateTime:
                        return quote + Convert.ToDateTime(obj).ToString() + quote;

                    case TypeCode.Boolean:
                        return Convert.ToInt32(Convert.ToBoolean(obj)).ToString();

                    case TypeCode.Empty:
                    case TypeCode.DBNull:
                        return "NULL";

                    default:
                        return Convert.ToString(obj);
                }
            } else
                return "NULL";
#endif
        }

        #endregion Object Extensions

        #region String Extensions

        /// <summary>
        /// Decode from Base64 string.
        /// </summary>
        /// <param name="base64">The base64 string.</param>
        /// <returns></returns>
        public static string FromBase64(this string base64)
            => !base64.IsNull() ? Encoding.UTF8.GetString(Convert.FromBase64String(base64)) : string.Empty;

        /// <summary>
        /// Encode the string to Base64 string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static string ToBase64(this string str) {
            if (str.IsNull())
                return string.Empty;

#if NET5_0_OR_GREATER
            ReadOnlySpan<byte>
#else
            byte[]
#endif
            bytes = Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(bytes);
        }

        /// <summary><inheritdoc cref="GetBytes(string, Encoding)"/></summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static byte[] GetBytes(this string str) => GetBytes(str, null);

        /// <summary>
        /// Gets the byte array of the string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public static byte[] GetBytes(this string str, [AllowNull] Encoding encoding) {
            if (encoding == null) {
                byte[] bytes = new byte[str.Length * sizeof(char)];
                Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);

                return bytes;
            } else
                return encoding.GetBytes(str);
        }

        /// <summary><inheritdoc cref="GetString(byte[], Encoding)"/></summary>
        /// <param name="bytes">The byte array.</param>
        /// <returns></returns>
        public static string GetString(this byte[] bytes) => GetString(bytes, null);

        /// <summary>
        /// Gets the string of an byte array.
        /// </summary>
        /// <param name="bytes">The byte array.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public static string GetString(this byte[] bytes, [AllowNull] Encoding encoding) {
            if (encoding == null) {
                char[] chars = new char[bytes.Length / sizeof(char)];
                Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);

                return new string(chars);
            } else
                return encoding.GetString(bytes);
        }

        #endregion String Extensions

        #region Task Extensions
#if NET35 || NET40
        public static Task GetCompletedTask()
                => new Task(() => { return; }, new System.Threading.CancellationToken(false), TaskCreationOptions.AttachedToParent);
#endif
        #endregion Task Extensions
    }
}

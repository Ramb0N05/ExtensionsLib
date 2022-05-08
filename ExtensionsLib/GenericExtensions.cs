using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

#if NET35 || NET40
using System.Threading.Tasks;
#endif

namespace SharpRambo.ExtensionsLib
{
    /// <summary>The GenericExtensions class.</summary>
    public static class GenericExtensions
    {
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
        public static string ParseToString(this object obj, Type ValueType = null, bool quoteStringValues = true)
#if NET5_0_OR_GREATER
            => obj == null
                ? "NULL"
                : Type.GetTypeCode(ValueType) switch
                {
                    TypeCode.Char or TypeCode.String => (quoteStringValues ? "'" : string.Empty) + Convert.ToString(obj) + (quoteStringValues ? "'" : string.Empty),
                    TypeCode.DateTime => (quoteStringValues ? "'" : string.Empty) + Convert.ToDateTime(obj).ToString() + (quoteStringValues ? "'" : string.Empty),
                    TypeCode.Boolean => Convert.ToInt32(Convert.ToBoolean(obj)).ToString(),
                    TypeCode.Empty or TypeCode.DBNull => "NULL",
                    _ => Convert.ToString(obj),
                };
#else
        {
            if (obj != null) {
                switch (Type.GetTypeCode(ValueType)) {
                    case TypeCode.Char:
                    case TypeCode.String:
                        return (quoteStringValues ? "'" : string.Empty) + Convert.ToString(obj) + (quoteStringValues ? "'" : string.Empty);
                    
                    case TypeCode.DateTime:
                        return (quoteStringValues ? "'" : string.Empty) + Convert.ToDateTime(obj).ToString() + (quoteStringValues ? "'" : string.Empty);

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
        }
#endif

        /// <summary><inheritdoc cref="Directory.CreateDirectory(string)" path="/summary/node()"/></summary>
        /// <param name="directoryInfo">The <see cref="DirectoryInfo"/> of the Directory to create</param>
        /// <returns></returns>
        public static DirectoryInfo CreateAnyway(this DirectoryInfo directoryInfo)
            => Directory.CreateDirectory(directoryInfo.FullName);

        /// <inheritdoc cref="File.ReadAllBytes(string)"/>
        public static byte[] ReadAllBytes(this FileInfo fileInfo)
            => fileInfo != null && fileInfo.Exists
                ? File.ReadAllBytes(Path.GetFullPath(fileInfo.FullName))
#if !NET20 && !NET35 && !NET40 && !NET45
                : Array.Empty<byte>();
#else
                : new byte[] {};
#endif

        /// <inheritdoc cref="File.ReadAllLines(string, System.Text.Encoding)"/>
        public static string[] ReadAllLines(this FileInfo fileInfo, System.Text.Encoding encoding = null)
            => fileInfo != null && fileInfo.Exists
                ? (encoding != null ? File.ReadAllLines(Path.GetFullPath(fileInfo.FullName), encoding) : File.ReadAllLines(Path.GetFullPath(fileInfo.FullName)))
#if !NET20 && !NET35 && !NET40 && !NET45
                : Array.Empty<string>();
#else
                : new string[] {};
#endif

        /// <inheritdoc cref="File.ReadAllText(string, System.Text.Encoding)"/>
        public static string ReadAllText(this FileInfo fileInfo, System.Text.Encoding encoding = null)
            => fileInfo != null && fileInfo.Exists
                ? (encoding != null
                    ? File.ReadAllText(Path.GetFullPath(fileInfo.FullName), encoding)
                    : File.ReadAllText(Path.GetFullPath(fileInfo.FullName)))
                : string.Empty;

        /// <inheritdoc cref="File.WriteAllLines(string, IEnumerable{string}, System.Text.Encoding)"/>
        public static FileInfo WriteAllLines(this FileInfo fileInfo, string[] contents, System.Text.Encoding encoding = null, bool overrideFile = true)
        {
            IEnumerable<string> c = contents;
            return WriteAllLines(fileInfo, c, encoding, overrideFile);
        }

        /// <inheritdoc cref="File.WriteAllBytes(string, byte[])"/>
        public static FileInfo WriteAllBytes(this FileInfo fileInfo, byte[] bytes, bool overrideFile = true)
        {
            if (fileInfo == null) throw new ArgumentNullException(nameof(fileInfo));
            if (bytes.IsNull<byte>()) bytes = new byte[] { 0 };

            if (overrideFile || !fileInfo.Exists)
                File.WriteAllBytes(Path.GetFullPath(fileInfo.FullName), bytes);

            fileInfo.Refresh();
            return fileInfo;
        }

        /// <inheritdoc cref="File.WriteAllLines(string, IEnumerable{string}, System.Text.Encoding)"/>
        public static FileInfo WriteAllLines(this FileInfo fileInfo, IEnumerable<string> contents, System.Text.Encoding encoding = null, bool overrideFile = true)
        {
            if (fileInfo == null) throw new ArgumentNullException(nameof(fileInfo));
            if (contents.IsNull()) contents = new string[] { string.Empty };

            if (overrideFile || (!fileInfo.Exists)) {
                if (encoding != null)
                    File.WriteAllLines(Path.GetFullPath(fileInfo.FullName), contents.ToArray(), encoding);
                else
                    File.WriteAllLines(Path.GetFullPath(fileInfo.FullName), contents.ToArray());
            }

            fileInfo.Refresh();
            return fileInfo;
        }

        /// <inheritdoc cref="File.WriteAllText(string, string?, System.Text.Encoding)"/>
        public static FileInfo WriteAllText(this FileInfo fileInfo, string contents = null, System.Text.Encoding encoding = null, bool overrideFile = true)
        {
            if (fileInfo == null) throw new ArgumentNullException(nameof(fileInfo));

            if (overrideFile || !fileInfo.Exists) {
                if (encoding != null)
                    File.WriteAllText(Path.GetFullPath(fileInfo.FullName), contents, encoding);
                else
                    File.WriteAllText(Path.GetFullPath(fileInfo.FullName), contents);
            }

            fileInfo.Refresh();
            return fileInfo;
        }

#if NET35 || NET40
        public static Task GetCompletedTask()
                => new Task(() => { return; }, new System.Threading.CancellationToken(false), TaskCreationOptions.AttachedToParent);
#endif
    }
}

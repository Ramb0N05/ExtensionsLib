using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;

#if !NET20

using System.Threading;
using System.Threading.Tasks;

#endif

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER

using System.Diagnostics.CodeAnalysis;

#endif

namespace SharpRambo.ExtensionsLib {

    /// <summary>
    /// The IOExtensions class.
    /// </summary>
    public static class IOExtensions {

        #region DirectoryInfo Extensions

        /// <summary>
        /// Determines whether this directory contains the given directory.
        /// </summary>
        /// <param name="baseDirectory">The base directory.</param>
        /// <param name="checkDirectory">The directory to check.</param>
        /// <returns>
        ///   <c>True</c> if <c>checkDirectory</c> is sub-directory of <c>baseDirectory</c>; otherwise, <c>False</c>.
        /// </returns>
        public static bool Contains(this DirectoryInfo baseDirectory,
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
            [AllowNull]
#endif
        DirectoryInfo checkDirectory)
            => checkDirectory?.Exists == true && checkDirectory.Parent != null
                && (checkDirectory.FullName == baseDirectory.FullName || checkDirectory.Parent.FullName == baseDirectory.FullName || baseDirectory.Contains(checkDirectory.Parent));

        /// <summary>
        /// Determines whether this directory contains the given file.
        /// </summary>
        /// <param name="baseDirectory">The base directory.</param>
        /// <param name="checkFile">The file to check.</param>
        /// <returns>
        ///   <c>True</c> if <c>checkDirectory</c> is located in <c>baseDirectory</c> or in an sub-directory of <c>baseDirectory</c>; otherwise, <c>False</c>.
        /// </returns>
        public static bool Contains(this DirectoryInfo baseDirectory,
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
            [AllowNull]
#endif
        FileInfo checkFile)
            => baseDirectory.Contains(checkFile?.Directory);

        /// <summary><inheritdoc cref="Directory.CreateDirectory(string)" path="/summary/node()"/></summary>
        /// <param name="directoryInfo">The <see cref="DirectoryInfo"/> of the Directory to create</param>
        /// <returns></returns>
        public static DirectoryInfo CreateAnyway(this DirectoryInfo directoryInfo)
            => Directory.CreateDirectory(directoryInfo.FullName);

        #endregion DirectoryInfo Extensions

        #region FileInfo Extensions

        /// <inheritdoc cref="File.ReadAllBytes(string)"/>
        public static byte[] ReadAllBytes(this FileInfo fileInfo)
            => fileInfo?.Exists == true
                ? File.ReadAllBytes(Path.GetFullPath(fileInfo.FullName))
#if NET8_0_OR_GREATER
                : [];

#elif !NET20 && !NET35 && !NET40 && !NET45
                : Array.Empty<byte>();

#else
                : new byte[] { };
#endif

#if !NET20 && ( NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER )

        /// <inheritdoc cref="File.ReadAllBytesAsync(string, CancellationToken)"/>
        public static async Task<byte[]> ReadAllBytesAsync(this FileInfo fileInfo, CancellationToken cancellationToken = default)
            => fileInfo?.Exists == true
                ? await File.ReadAllBytesAsync(Path.GetFullPath(fileInfo.FullName), cancellationToken)
#if NET8_0_OR_GREATER
                : [];

#elif !NET20 && !NET35 && !NET40 && !NET45
                : Array.Empty<byte>();

#else
                : new byte[] { };
#endif
#endif

        /// <inheritdoc cref="File.ReadAllLines(string)"/>
        public static string[] ReadAllLines(this FileInfo fileInfo)
            => ReadAllLines(fileInfo, null);

        /// <inheritdoc cref="File.ReadAllLines(string, System.Text.Encoding)"/>
        public static string[] ReadAllLines(this FileInfo fileInfo,
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
            [AllowNull]
#endif
        System.Text.Encoding encoding) {
            if (fileInfo?.Exists != true)
                return
#if NET8_0_OR_GREATER
                [];
#elif !NET20 && !NET35 && !NET40 && !NET45
                Array.Empty<string>();
#else
                new string[] { };
#endif

            return encoding != null
                ? File.ReadAllLines(Path.GetFullPath(fileInfo.FullName), encoding)
                : File.ReadAllLines(Path.GetFullPath(fileInfo.FullName));
        }

#if !NET20 && (NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER)

        /// <inheritdoc cref="File.ReadAllLinesAsync(string, CancellationToken)"/>
        public static async Task<string[]> ReadAllLinesAsync(this FileInfo fileInfo, CancellationToken cancellationToken = default)
            => await ReadAllLinesAsync(fileInfo, null, cancellationToken);

        /// <inheritdoc cref="File.ReadAllLinesAsync(string, System.Text.Encoding, CancellationToken)"/>
        public static async Task<string[]> ReadAllLinesAsync(this FileInfo fileInfo,
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
            [AllowNull]
#endif
        System.Text.Encoding encoding, CancellationToken cancellationToken = default) {
            if (fileInfo?.Exists != true)
                return
#if NET8_0_OR_GREATER
                [];
#elif !NET20 && !NET35 && !NET40 && !NET45
                Array.Empty<string>();
#else
                new string[] { };
#endif

            return encoding != null
                ? await File.ReadAllLinesAsync(Path.GetFullPath(fileInfo.FullName), encoding, cancellationToken)
                : await File.ReadAllLinesAsync(Path.GetFullPath(fileInfo.FullName), cancellationToken);
        }

#endif

        /// <inheritdoc cref="File.ReadAllText(string)"/>
        public static string ReadAllText(this FileInfo fileInfo)
            => ReadAllText(fileInfo, null);

        /// <inheritdoc cref="File.ReadAllText(string, System.Text.Encoding)"/>
        public static string ReadAllText(this FileInfo fileInfo,
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
            [AllowNull]
#endif
        System.Text.Encoding encoding) {
            if (fileInfo?.Exists != true)
                return string.Empty;

            return encoding != null
                    ? File.ReadAllText(Path.GetFullPath(fileInfo.FullName), encoding)
                    : File.ReadAllText(Path.GetFullPath(fileInfo.FullName));
        }

#if !NET20 && (NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER)

        /// <inheritdoc cref="File.ReadAllTextAsync(string, CancellationToken)"/>
        public static async Task<string> ReadAllTextAsync(this FileInfo fileInfo, CancellationToken cancellationToken)
            => await ReadAllTextAsync(fileInfo, null, cancellationToken);

        /// <inheritdoc cref="File.ReadAllTextAsync(string, System.Text.Encoding, CancellationToken)"/>
        public static async Task<string> ReadAllTextAsync(this FileInfo fileInfo,
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
            [AllowNull]
#endif
        System.Text.Encoding encoding, CancellationToken cancellationToken) {
            if (fileInfo?.Exists != true)
                return string.Empty;

            return encoding != null
                    ? await File.ReadAllTextAsync(Path.GetFullPath(fileInfo.FullName), encoding, cancellationToken)
                    : await File.ReadAllTextAsync(Path.GetFullPath(fileInfo.FullName), cancellationToken);
        }

#endif

        /// <inheritdoc cref="File.WriteAllBytes(string, byte[])"/>
        public static FileInfo WriteAllBytes(this FileInfo fileInfo, byte[] bytes, bool overrideFile = true) {
            if (fileInfo == null)
                throw new ArgumentNullException(nameof(fileInfo));
            if (bytes.IsNull<byte>())
                bytes =
#if NET8_0_OR_GREATER
                    [0];
#else
                    new byte[] { 0 };
#endif

            if (overrideFile || !fileInfo.Exists)
                File.WriteAllBytes(Path.GetFullPath(fileInfo.FullName), bytes);

            fileInfo.Refresh();
            return fileInfo;
        }

#if !NET20 && (NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER)

        /// <inheritdoc cref="File.WriteAllBytesAsync(string, byte[], CancellationToken)"/>
        public static async Task<FileInfo> WriteAllBytesAsync(this FileInfo fileInfo, byte[] bytes, bool overrideFile = true, CancellationToken cancellationToken = default) {
            if (fileInfo == null)
                throw new ArgumentNullException(nameof(fileInfo));

            if (bytes.IsNull<byte>())
                bytes =
#if NET8_0_OR_GREATER
                    [0];
#else
                    new byte[] { 0 };
#endif

            if (overrideFile || !fileInfo.Exists)
                await File.WriteAllBytesAsync(Path.GetFullPath(fileInfo.FullName), bytes, cancellationToken);

            fileInfo.Refresh();
            return fileInfo;
        }

#endif

        /// <inheritdoc cref="File.WriteAllLines(string, IEnumerable{string})"/>
        public static FileInfo WriteAllLines(this FileInfo fileInfo, IEnumerable<string> contents, bool overrideFile = true)
            => WriteAllLines(fileInfo, contents, null, overrideFile);

        /// <inheritdoc cref="File.WriteAllLines(string, IEnumerable{string}, System.Text.Encoding)"/>
        public static FileInfo WriteAllLines(this FileInfo fileInfo, IEnumerable<string> contents,
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
            [AllowNull]
#endif
        System.Text.Encoding encoding, bool overrideFile = true) {
            if (fileInfo == null)
                throw new ArgumentNullException(nameof(fileInfo));
            if (contents.IsNull())
                contents = new string[] { string.Empty };

            if (overrideFile || (!fileInfo.Exists)) {
                if (encoding != null)
                    File.WriteAllLines(Path.GetFullPath(fileInfo.FullName), contents.ToArray(), encoding);
                else
                    File.WriteAllLines(Path.GetFullPath(fileInfo.FullName), contents.ToArray());
            }

            fileInfo.Refresh();
            return fileInfo;
        }

#if !NET20 && (NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER)

        /// <inheritdoc cref="File.WriteAllLinesAsync(string, IEnumerable{string}, System.Text.Encoding, CancellationToken)"/>
        public static async Task<FileInfo> WriteAllLinesAsync(this FileInfo fileInfo, IEnumerable<string> contents, bool overrideFile = true, CancellationToken cancellationToken = default)
            => await WriteAllLinesAsync(fileInfo, contents, null, overrideFile, cancellationToken);

        public static async Task<FileInfo> WriteAllLinesAsync(this FileInfo fileInfo, IEnumerable<string> contents,
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
            [AllowNull]
#endif
        System.Text.Encoding encoding, bool overrideFile = true, CancellationToken cancellationToken = default) {
            if (fileInfo == null)
                throw new ArgumentNullException(nameof(fileInfo));
            if (contents.IsNull())
                contents = new string[] { string.Empty };

            if (overrideFile || (!fileInfo.Exists)) {
                if (encoding != null)
                    await File.WriteAllLinesAsync(Path.GetFullPath(fileInfo.FullName), contents.ToArray(), encoding, cancellationToken);
                else
                    await File.WriteAllLinesAsync(Path.GetFullPath(fileInfo.FullName), contents.ToArray(), cancellationToken);
            }

            fileInfo.Refresh();
            return fileInfo;
        }

#endif

        /// <inheritdoc cref="File.WriteAllText(string, string?)"/>
        public static FileInfo WriteAllText(this FileInfo fileInfo,
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
            [AllowNull]
#endif
        string contents, bool overrideFile = true)
            => WriteAllText(fileInfo, contents, null, overrideFile);

        /// <inheritdoc cref="File.WriteAllText(string, string?, System.Text.Encoding)"/>
        public static FileInfo WriteAllText(this FileInfo fileInfo,
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
            [AllowNull]
#endif
        string contents,
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
            [AllowNull]
#endif
        System.Text.Encoding encoding, bool overrideFile = true) {
            if (fileInfo == null)
                throw new ArgumentNullException(nameof(fileInfo));

            if (overrideFile || !fileInfo.Exists) {
                if (encoding != null)
                    File.WriteAllText(Path.GetFullPath(fileInfo.FullName), contents, encoding);
                else
                    File.WriteAllText(Path.GetFullPath(fileInfo.FullName), contents);
            }

            fileInfo.Refresh();
            return fileInfo;
        }

#if !NET20 && (NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER)

        /// <inheritdoc cref="File.WriteAllTextAsync(string, string?, CancellationToken)"/>
        public static async Task<FileInfo> WriteAllTextAsync(this FileInfo fileInfo,
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
            [AllowNull]
#endif
        string contents, bool overrideFile = true, CancellationToken cancellationToken = default)
            => await WriteAllTextAsync(fileInfo, contents, null, overrideFile, cancellationToken);

        /// <inheritdoc cref="File.WriteAllTextAsync(string, string?, System.Text.Encoding, CancellationToken)"/>
        public static async Task<FileInfo> WriteAllTextAsync(this FileInfo fileInfo,
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
            [AllowNull]
#endif
        string contents,
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
            [AllowNull]
#endif
        System.Text.Encoding encoding, bool overrideFile = true, CancellationToken cancellationToken = default) {
            if (fileInfo == null)
                throw new ArgumentNullException(nameof(fileInfo));

            if (overrideFile || !fileInfo.Exists) {
                if (encoding != null)
                    await File.WriteAllTextAsync(Path.GetFullPath(fileInfo.FullName), contents, encoding, cancellationToken);
                else
                    await File.WriteAllTextAsync(Path.GetFullPath(fileInfo.FullName), contents, cancellationToken);
            }

            fileInfo.Refresh();
            return fileInfo;
        }

#endif

        #endregion FileInfo Extensions
    }
}

using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER

using System.Diagnostics.CodeAnalysis;

#endif

namespace SharpRambo.ExtensionsLib {

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
        #endregion DirectoryInfo Extensions

        #region FileInfo Extensions

        /// <inheritdoc cref="File.ReadAllLines(string, System.Text.Encoding)"/>
        public static string[] ReadAllLines(this FileInfo fileInfo, System.Text.Encoding encoding = null) {
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

        /// <inheritdoc cref="File.ReadAllText(string, System.Text.Encoding)"/>
        public static string ReadAllText(this FileInfo fileInfo, System.Text.Encoding encoding = null) {
            if (fileInfo?.Exists != true)
                return string.Empty;

            return encoding != null
                    ? File.ReadAllText(Path.GetFullPath(fileInfo.FullName), encoding)
                    : File.ReadAllText(Path.GetFullPath(fileInfo.FullName));
        }

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

        /// <inheritdoc cref="File.WriteAllLines(string, IEnumerable{string}, System.Text.Encoding)"/>
        public static FileInfo WriteAllLines(this FileInfo fileInfo, string[] contents, System.Text.Encoding encoding = null, bool overrideFile = true) {
            IEnumerable<string> c = contents;
            return WriteAllLines(fileInfo, c, encoding, overrideFile);
        }

        /// <inheritdoc cref="File.WriteAllLines(string, IEnumerable{string}, System.Text.Encoding)"/>
        public static FileInfo WriteAllLines(this FileInfo fileInfo, IEnumerable<string> contents, System.Text.Encoding encoding = null, bool overrideFile = true) {
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

        /// <inheritdoc cref="File.WriteAllText(string, string?, System.Text.Encoding)"/>
        public static FileInfo WriteAllText(this FileInfo fileInfo, string contents = null, System.Text.Encoding encoding = null, bool overrideFile = true) {
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

        #endregion FileInfo Extensions
    }
}

using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security.Cryptography;
using System.Text;

#if !NET20
using System.Threading.Tasks;
#endif

namespace SharpRambo.ExtensionsLib {
    public static class HashingExtensions {
        #region Synchron
        #region String hashes
        public static string ToSHA1(this string rawData)
            => ToSHA1(rawData, Encoding.UTF8);

        public static string ToSHA1(this string rawData,
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
            [DisallowNull]
#endif
        Encoding encoding) {
#if !NET5_0_OR_GREATER
            SHA1 algo = SHA1.Create();
#endif

            byte[] bytes =
#if NET5_0_OR_GREATER
                SHA1.HashData(encoding.GetBytes(rawData));
#else
                algo.ComputeHash(encoding.GetBytes(rawData));
#endif

            StringBuilder builder =
#if NET5_0_OR_GREATER
                new();
#else
                new StringBuilder();
#endif

            foreach (byte b in bytes)
                builder.Append(b.ToString("x2"));

            return builder.ToString();
        }

        public static string ToSHA256(this string rawData)
            => ToSHA256(rawData, Encoding.UTF8);

        public static string ToSHA256(this string rawData,
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
            [DisallowNull]
#endif
        Encoding encoding) {
#if !NET5_0_OR_GREATER
            SHA256 algo = SHA256.Create();
#endif

            byte[] bytes =
#if NET5_0_OR_GREATER
                SHA256.HashData(encoding.GetBytes(rawData));
#else
                algo.ComputeHash(encoding.GetBytes(rawData));
#endif

            StringBuilder builder =
#if NET5_0_OR_GREATER
                new();
#else
                new StringBuilder();
#endif

            foreach(byte b in bytes)
                builder.Append(b.ToString("x2"));

            return builder.ToString();
        }

        public static string ToSHA512(this string rawData)
            => ToSHA512(rawData, Encoding.UTF8);

        public static string ToSHA512(this string rawData,
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
            [DisallowNull]
#endif
        Encoding encoding) {
#if !NET5_0_OR_GREATER
            SHA512 algo = SHA512.Create();
#endif

            byte[] bytes =
#if NET5_0_OR_GREATER
                SHA512.HashData(encoding.GetBytes(rawData));
#else
                algo.ComputeHash(encoding.GetBytes(rawData));
#endif

            StringBuilder builder =
#if NET5_0_OR_GREATER
                new();
#else
                new StringBuilder();
#endif

            foreach(byte b in bytes)
                builder.Append(b.ToString("x2"));

            return builder.ToString();
        }

        #endregion String hashes

        #region File hashes

        public static string ToSHA1(this FileInfo file) {
            if (!file.Exists)
                return string.Empty;

#if !NET7_0_OR_GREATER
            SHA1 algo = SHA1.Create();
#endif

            FileStream stream = File.OpenRead(file.FullName);
            byte[] bytes =
#if NET7_0_OR_GREATER
                SHA1.HashData(stream);
#else
                algo.ComputeHash(stream);
#endif

            StringBuilder builder =
#if NET5_0_OR_GREATER
                new();
#else
                new StringBuilder();
#endif

            foreach (byte b in bytes)
                builder.Append(b.ToString("x2"));

            return builder.ToString();
        }

        public static string ToSHA256(this FileInfo file) {
            if (!file.Exists)
                return string.Empty;

#if !NET7_0_OR_GREATER
            SHA256 algo = SHA256.Create();
#endif

            FileStream stream = File.OpenRead(file.FullName);
            byte[] bytes =
#if NET7_0_OR_GREATER
                SHA256.HashData(stream);
#else
                algo.ComputeHash(stream);
#endif

            StringBuilder builder =
#if NET5_0_OR_GREATER
                new();
#else
                new StringBuilder();
#endif

            foreach(byte b in bytes)
                builder.Append(b.ToString("x2"));

            return builder.ToString();
        }

        public static string ToSHA512(this FileInfo file) {
            if (!file.Exists)
                return string.Empty;

#if !NET7_0_OR_GREATER
            SHA256 algo = SHA256.Create();
#endif

            FileStream stream = File.OpenRead(file.FullName);
            byte[] bytes =
#if NET7_0_OR_GREATER
                SHA512.HashData(stream);
#else
                algo.ComputeHash(stream);
#endif

            StringBuilder builder =
#if NET5_0_OR_GREATER
                new();
#else
                new StringBuilder();
#endif

            foreach (byte b in bytes)
                builder.Append(b.ToString("x2"));

            return builder.ToString();
        }

        #endregion File hashes
        #endregion Synchron

#if NET5_0_OR_GREATER
        #region Asynchron
        #region String hashes
        public static async Task<string> ToSHA1Async(this string rawData)
            => await ToSHA1Async(rawData, Encoding.UTF8);

        public static async Task<string> ToSHA1Async(this string rawData,
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
            [DisallowNull]
#endif
        Encoding encoding) {
#if !NET7_0_OR_GREATER
            SHA1 algo = SHA1.Create();
#endif

            MemoryStream dataStream = new(encoding.GetBytes(rawData));
            byte[] bytes =
#if NET7_0_OR_GREATER
                await SHA1.HashDataAsync(dataStream);
#elif NET5_0_OR_GREATER
                await algo.ComputeHashAsync(dataStream);
#endif

            StringBuilder builder = new();

            await bytes.ForEachAsync(async b => {
                builder.Append(b.ToString("x2"));
                await AsyncExtensions.CompletedTask;
            });

            return builder.ToString();
        }

        public static async Task<string> ToSHA256Async(this string rawData)
            => await ToSHA256Async(rawData, Encoding.UTF8);

        public static async Task<string> ToSHA256Async(this string rawData,
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
            [DisallowNull]
#endif
        Encoding encoding) {
#if !NET7_0_OR_GREATER
            SHA256 algo = SHA256.Create();
#endif

            MemoryStream dataStream = new(encoding.GetBytes(rawData));
            byte[] bytes =
#if NET7_0_OR_GREATER
                await SHA256.HashDataAsync(dataStream);
#elif NET5_0_OR_GREATER
                await algo.ComputeHashAsync(dataStream);
#endif

            StringBuilder builder = new();

            await bytes.ForEachAsync(async b => {
                builder.Append(b.ToString("x2"));
                await AsyncExtensions.CompletedTask;
            });

            return builder.ToString();
        }

        public static async Task<string> ToSHA512Async(this string rawData)
            => await ToSHA512Async(rawData, Encoding.UTF8);

        public static async Task<string> ToSHA512Async(this string rawData,
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
            [DisallowNull]
#endif
        Encoding encoding) {
#if !NET7_0_OR_GREATER
            SHA512 algo = SHA512.Create();
#endif

            MemoryStream dataStream = new(encoding.GetBytes(rawData));
            byte[] bytes =
#if NET7_0_OR_GREATER
                await SHA512.HashDataAsync(dataStream);
#elif NET5_0_OR_GREATER
                await algo.ComputeHashAsync(dataStream);
#endif

            StringBuilder builder = new();

            await bytes.ForEachAsync(async b => {
                builder.Append(b.ToString("x2"));
                await AsyncExtensions.CompletedTask;
            });

            return builder.ToString();
        }

        #endregion String hashes

        #region File hashes
        public static async Task<string> ToSHA1Async(this FileInfo file) {
            if (!file.Exists)
                return string.Empty;

#if !NET7_0_OR_GREATER
            SHA1 algo = SHA1.Create();
#endif

            FileStream stream = File.OpenRead(file.FullName);
            byte[] bytes =
#if NET7_0_OR_GREATER
                await SHA1.HashDataAsync(stream);
#elif NET5_0_OR_GREATER
                await algo.ComputeHashAsync(stream);
#endif

            StringBuilder builder = new();

            await bytes.ForEachAsync(async b => {
                builder.Append(b.ToString("x2"));
                await AsyncExtensions.CompletedTask;
            });

            return builder.ToString();
        }

        public static async Task<string> ToSHA256Async(this FileInfo file) {
            if (!file.Exists)
                return string.Empty;

#if !NET7_0_OR_GREATER
            SHA256 algo = SHA256.Create();
#endif

            FileStream stream = File.OpenRead(file.FullName);
            byte[] bytes =
#if NET7_0_OR_GREATER
                await SHA256.HashDataAsync(stream);
#elif NET5_0_OR_GREATER
                await algo.ComputeHashAsync(stream);
#endif

            StringBuilder builder = new();

            await bytes.ForEachAsync(async b => {
                builder.Append(b.ToString("x2"));
                await AsyncExtensions.CompletedTask;
            });

            return builder.ToString();
        }

        public static async Task<string> ToSHA512Async(this FileInfo file) {
            if (!file.Exists)
                return string.Empty;

#if !NET7_0_OR_GREATER
            SHA256 algo = SHA256.Create();
#endif

            FileStream stream = File.OpenRead(file.FullName);
            byte[] bytes =
#if NET7_0_OR_GREATER
                await SHA512.HashDataAsync(stream);
#elif NET5_0_OR_GREATER
                await algo.ComputeHashAsync(stream);
#endif

            StringBuilder builder = new();

            await bytes.ForEachAsync(async b => {
                builder.Append(b.ToString("x2"));
                await AsyncExtensions.CompletedTask;
            });

            return builder.ToString();
        }

        #endregion File hashes
#endregion Asynchron
#endif
    }
}

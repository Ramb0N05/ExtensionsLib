#if !NET20

using System.Threading.Tasks;

namespace SharpRambo.ExtensionsLib {

    /// <summary>
    /// The AsyncExtensions class.
    /// </summary>
    public static class AsyncExtensions {

        #region Task Extensions

        /// <summary>
        /// A framework-independent alternative to <c>Task.CompletedTask</c> or <c>Task.FromResult()</c>:<br />
        /// "Gets a task that has already completed successfully."
        /// </summary>
        /// <returns>The successfully completed task.</returns>
        public static Task CompletedTask
#if NET35 || NET40

            => new Task(() => { }, new System.Threading.CancellationToken(false), TaskCreationOptions.AttachedToParent);

#elif NET45

            => Task.FromResult(0);

#else

            => Task.CompletedTask;

#endif

        #endregion Task Extensions
    }
}

#endif

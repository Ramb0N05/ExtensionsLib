using System;

namespace SharpRambo.ExtensionsLib.ConsoleExtensions {

    public class MenuItem {

        #region Private Fields

        private readonly ConsoleX _cmd;
        private readonly ushort _writeDividerChar;

        #endregion Private Fields

        #region Public Properties

        public string Code { get; set; }
        public string DisplayName { get; set; }
        public bool IsDivider { get; }

        #endregion Public Properties

        #region Public Constructors

        public MenuItem(ConsoleX cmd, string code, string displayName = null) {
            if (code.IsNull())
                throw new ArgumentNullException(nameof(code));

            _cmd = cmd ?? new ConsoleX();
            Code = code;
            IsDivider = false;
            DisplayName = !displayName.IsNull() ? displayName : string.Empty;
        }

        public MenuItem(ConsoleX cmd, ushort writeDividerChar = 0) {
            _cmd = cmd ?? new ConsoleX();
            IsDivider = true;
            _writeDividerChar = writeDividerChar;
        }

        #endregion Public Constructors

        #region Public Methods

        public void WriteToConsole() {
            if (IsDivider) {
                if (_writeDividerChar == 1)
                    _cmd.WriteSmallDivider();
                else if (_writeDividerChar == 2)
                    _cmd.WriteDivider();
                else
                    Console.WriteLine();
            } else
                Console.WriteLine("[" + (Code.Length == 1 ? " " + Code + " " : Code.PadLeft(3)) + "]" + (!DisplayName.IsNull() ? " - " + DisplayName : string.Empty));
        }

        #endregion Public Methods
    }
}

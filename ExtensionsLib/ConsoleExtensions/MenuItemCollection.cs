using System;
using System.Collections.Generic;

namespace SharpRambo.ExtensionsLib.ConsoleExtensions {

    public class MenuItemCollection : List<MenuItem> {

        #region Private Fields

        private readonly ConsoleX _cmd;

        #endregion Private Fields

        #region Public Properties

        public string ItemPrefix { get; set; }

        #endregion Public Properties

        #region Public Constructors

        public MenuItemCollection() : base() {
            ItemPrefix = string.Empty;
            _cmd = new ConsoleX();
        }

        public MenuItemCollection(int capacity) : base(capacity) {
            ItemPrefix = string.Empty;
            _cmd = new ConsoleX();
        }

        public MenuItemCollection(IEnumerable<MenuItem> collection) : base(collection) {
            _cmd = new ConsoleX();
            ItemPrefix = string.Empty;
        }

        public MenuItemCollection(ConsoleX cmd) : this() {
            _cmd = cmd ?? new ConsoleX();
        }

        public MenuItemCollection(ConsoleX cmd, string prefix) : this() {
            _cmd = cmd ?? new ConsoleX();
            ItemPrefix = !prefix.IsNull() ? prefix : string.Empty;
        }

        public MenuItemCollection(ConsoleX cmd, string prefix, int capacity) : this(capacity) {
            _cmd = cmd ?? new ConsoleX();
            ItemPrefix = !prefix.IsNull() ? prefix : string.Empty;
        }

        public MenuItemCollection(ConsoleX cmd, string prefix, IEnumerable<MenuItem> collection) : this(collection) {
            _cmd = cmd ?? new ConsoleX();
            ItemPrefix = !prefix.IsNull() ? prefix : string.Empty;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Add(string menuCode, string displayName)
            => Add(new MenuItem(_cmd, menuCode, displayName));

        public void Add(ushort writeDividerChar = 0)
            => Add(new MenuItem(_cmd, writeDividerChar));

        public void WriteToConsole() {
            foreach (MenuItem item in this) {
                if (!item.IsDivider)
                    Console.Write(ItemPrefix);
                item.WriteToConsole();
            }
        }

        #endregion Public Methods
    }
}

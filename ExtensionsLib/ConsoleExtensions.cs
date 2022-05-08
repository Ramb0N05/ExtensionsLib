using System;
using System.Data;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SharpRambo.ExtensionsLib
{
    public class ConsoleX
    {
        public uint MenuWidth { get; set; }
        public char MenuDividerChar { get; set; }
        public char MenuSmallDividerChar { get; set; }
        public string MenuCaptionAffix { get; set; }
        public string PauseMessage { get; set; }
        public uint WindowHeight { get; set; }
        public uint BufferHeight { get; set; }

        public ConsoleX(uint menuWidth = 85, uint windowHeight = 30, uint bufferHeight = 255, char menuDividerChar = '=', char menuSmallDividerChar = '-', string menuCaptionAffix = "~~", string pauseMessage = "Drücken Sie die Eingabetaste. . . ")
        {
            MenuWidth = menuWidth > 0 ? menuWidth : 85;
            MenuDividerChar = menuDividerChar;
            MenuSmallDividerChar = menuSmallDividerChar;
            MenuCaptionAffix = !menuCaptionAffix.IsNull() ? menuCaptionAffix : "~~";
            PauseMessage = !pauseMessage.IsNull() ? pauseMessage : "Drücken Sie die Eingabetaste . . . ";
            WindowHeight = windowHeight > 0 ? windowHeight : 30;
            BufferHeight = bufferHeight > 0 && bufferHeight >= windowHeight ? bufferHeight : 255;
        }

        public void ApplyBufferAndWindowSize()
        {
#if !NETFRAMEWORK
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
#endif
                int cmdWidth = Convert.ToInt32(MenuWidth + 1);
                Console.SetWindowSize(cmdWidth, Convert.ToInt32(WindowHeight));
                Console.SetBufferSize(cmdWidth, Convert.ToInt32(BufferHeight));
#if !NETFRAMEWORK
            }
#endif
        }

        public void Pause(bool newLine = true)
        {
            Console.Write((newLine ? Environment.NewLine : string.Empty) + PauseMessage);
            Console.ReadLine();
        }

        public void RunCommandNoShell(string command, string arguments = null, bool waitForExit = false) => RunCommand(command, arguments, waitForExit, false);
        public void RunCommand(string command, string arguments = null, bool waitForExit = false, bool shell = true)
        {
            if (command.IsNull()) throw new ArgumentNullException(nameof(command));

            try
            {
                ProcessStartInfo psi =
#if NET5_0_OR_GREATER
                    new(
#else
                    new ProcessStartInfo(
#endif
                        command, !arguments.IsNull() ? arguments : string.Empty
                    ) {
                        UseShellExecute = shell
                    };

                Process p = Process.Start(psi);

                if (waitForExit)
                    p.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.Clear();
                WriteException(ex, "Befehl: " + command);
            }
        }

        public void WriteCaption(string caption, bool clearConsole = false, bool writeDividers = true, bool appendAffixWhitespace = true)
        {
            if (!caption.IsNull())
            {
                if (!MenuCaptionAffix.IsNull())
                    caption = MenuCaptionAffix + (appendAffixWhitespace ? " " : string.Empty) + caption + (appendAffixWhitespace ? " " : string.Empty) + MenuCaptionAffix;

                uint menuCenter = MenuWidth / 2;
                uint captionCenter = Convert.ToUInt32(caption.Length / 2);
                uint captionStart = menuCenter > captionCenter ? menuCenter - captionCenter : 1;

                for (int i = 1; i <= captionStart; i++)
                    caption = caption.Insert(0, " ");

                if (clearConsole) Console.Clear();
                if (writeDividers) WriteDivider();
                Console.WriteLine(caption);
                if (writeDividers) WriteDivider();
            }
        }

#if DEBUG
        public void WriteDebug(string message, bool pause = false)
        {
            WriteLines(
                Environment.NewLine + Environment.NewLine + "##  DEBUG  ##",
                message,
                Environment.NewLine + "#############"
            );

            if (pause) Pause(true);
        }
#endif

        public void WriteDivider(char? overrideDividerChar = null)
        {
            string dividerString = string.Empty;

            for (int i = 1; i <= MenuWidth; i++)
                dividerString += overrideDividerChar != null ? overrideDividerChar : MenuDividerChar;

            if (!dividerString.IsNull())
                Console.WriteLine(dividerString);
        }

        public void WriteSmallDivider()
            => WriteDivider(MenuSmallDividerChar);

        public void WriteException(string exceptionMessage, string additionalMessage = null, bool pause = true)
        {
            Console.WriteLine(Environment.NewLine + "##  ERROR  ##");
            if (!additionalMessage.IsNull()) Console.WriteLine(additionalMessage);

            WriteLines(
                !exceptionMessage.IsNull() ? exceptionMessage : "Unknown Error.",
                "#############"
            );

            if (pause) Pause(true);
        }

        public void WriteException(Exception ex, string additionalMessage = null, bool pause = true)
            => WriteException(ex.Message, additionalMessage, pause);

        public void WriteLines(params string[] values) => WriteLines(new List<string>(values));
        public void WriteLines(string prefix, params string[] values) => WriteLines(new List<string>(values), prefix);
        public void WriteLines(IEnumerable<string> values, string prefix = null) {
                foreach (string value in values)
                    Console.Write((!prefix.IsNull() ? prefix : string.Empty) + (!value.IsNull() ? value : string.Empty) + Environment.NewLine);
        }

        public void WriteMenuItem(string menuCode, string displayName = null, string itemPrefix = null)
            => Console.WriteLine(
                (!itemPrefix.IsNull() ? itemPrefix : string.Empty) +
                (menuCode.Length == 1 ? "[ " + menuCode + " ]" : "[" + menuCode.PadLeft(3) + "]") +
                (!displayName.IsNull() ? " - " + displayName : string.Empty)
            );

        public string WriteQuestion(string message, bool newline = true)
        {
            Console.Write((newline ? Environment.NewLine : string.Empty) + message);
            return Console.ReadLine();
        }

        public void WriteWarning(string message, bool newLine = true, bool pause = true)
        {
            Console.WriteLine((newLine ? Environment.NewLine : string.Empty) + "[Warning] " + message);
            if (pause) Pause(true);
        }

        public void WriteTable(DataTable table, bool writeEndingDivider = false, char? overrideDividerChar = null, char columnDivider = '|')
        {
            if (overrideDividerChar == null) WriteSmallDivider();
            else WriteDivider(overrideDividerChar);

            foreach (DataColumn col in table.Columns)
                Console.Write(columnDivider + " " + col.Caption.PadRight(col.MaxLength > 0 ? col.MaxLength : col.Caption.Length) + " ");
            Console.Write(Environment.NewLine);

            if (overrideDividerChar == null) WriteSmallDivider();
            else WriteDivider(overrideDividerChar);

            foreach (DataRow row in table.Rows) {
                int i = 0;

                foreach (var cell in row.ItemArray)
                {
                    string cellStr = Convert.ToString(cell);
                    int pad = i <= table.Columns.Count ? table.Columns[i].MaxLength : cellStr.Length;

                    Console.Write("| " + cellStr.PadRight(pad > 0 ? pad : cellStr.Length) + " ");
                    i++;
                }

                Console.Write(Environment.NewLine);
            }

            if (writeEndingDivider)
            {
                if (overrideDividerChar == null) WriteSmallDivider();
                else WriteDivider(overrideDividerChar);
            }
        }
    }

    public class MenuItemCollection : List<MenuItem>
    {
        public string ItemPrefix { get; set; }
        private ConsoleX _CMD = null;

        public MenuItemCollection() : base()
        {
            ItemPrefix = string.Empty;
            _CMD = new ConsoleX();
        }

        public MenuItemCollection(int capacity) : base(capacity)
        {
            ItemPrefix = string.Empty;
            _CMD = new ConsoleX();
        }

        public MenuItemCollection(IEnumerable<MenuItem> collection) : base(collection)
        {
            _CMD = new ConsoleX();
            ItemPrefix = string.Empty;
        }

        public MenuItemCollection(ConsoleX cmd) : this()
        {
            _CMD = cmd ?? new ConsoleX();
        }

        public MenuItemCollection(ConsoleX cmd, string prefix) : this()
        {
            _CMD = cmd ?? new ConsoleX();
            ItemPrefix = !prefix.IsNull() ? prefix : string.Empty;
        }

        public MenuItemCollection(ConsoleX cmd, string prefix, int capacity) : this(capacity)
        {
            _CMD = cmd ?? new ConsoleX();
            ItemPrefix = !prefix.IsNull() ? prefix : string.Empty;
        }

        public MenuItemCollection(ConsoleX cmd, string prefix, IEnumerable<MenuItem> collection) : this(collection)
        {
            _CMD = cmd ?? new ConsoleX();
            ItemPrefix = !prefix.IsNull() ? prefix : string.Empty;
        }

        public void WriteToConsole()
        {
            foreach (MenuItem item in this)
            {
                if (!item.IsDivider) Console.Write(ItemPrefix);
                item.WriteToConsole();
            }
        }

        public void Add(string menuCode, string displayName)
            => Add(new MenuItem(_CMD, menuCode, displayName));

        public void Add(ushort writeDividerChar = 0)
            => Add(new MenuItem(_CMD, writeDividerChar));
    }

    public class MenuItem
    {
        public string Code { get; set; }
        public string DisplayName { get; set; }
        public bool IsDivider { get; private set; }
        private ushort _WriteDividerChar = 0;
        private ConsoleX _CMD = null;

        public MenuItem(ConsoleX cmd, string code, string displayName = null)
        {
            if (code.IsNull()) throw new ArgumentNullException(nameof(code));

            _CMD = cmd ?? new ConsoleX();
            Code = code;
            IsDivider = false;
            DisplayName = !displayName.IsNull() ? displayName : string.Empty;
        }

        public MenuItem(ConsoleX cmd, ushort writeDividerChar = 0)
        {
            _CMD = cmd ?? new ConsoleX();
            IsDivider = true;
            _WriteDividerChar = writeDividerChar;
        }

        public void WriteToConsole()
        {
            if (IsDivider)
            {
                if (_WriteDividerChar == 1) _CMD.WriteSmallDivider();
                else if (_WriteDividerChar == 2) _CMD.WriteDivider();
                else Console.WriteLine();
            }
            else Console.WriteLine("[" + (Code.Length == 1 ? " " + Code + " " : Code.PadLeft(3)) + "]" + (!DisplayName.IsNull() ? " - " + DisplayName : string.Empty));
        }
    }
}

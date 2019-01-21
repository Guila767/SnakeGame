using System;
using System.Text;
using System.Runtime.InteropServices;

namespace WinApiConsole
{
    [StructLayout(LayoutKind.Sequential)]
    public struct COORD
    {
        public short X;
        public short Y;

        public COORD(short X, short Y)
        {
            this.X = X;
            this.Y = Y;
        }
    };

    [Flags]
    public enum Buttons
    {
        // Source: https://docs.microsoft.com/pt-br/windows/desktop/inputdev/virtual-key-codes

        /// <summary>
        /// BACKSPACE Key
        /// </summary>
        VK_BACK = 0x08,
        /// <summary>
        /// TAB key
        /// </summary>
        VK_TAB = 0x09,
        /// <summary>
        /// CLEAR key
        /// </summary>
        VK_CLEAR = 0x0C,
        /// <summary>
        /// ENTER key
        /// </summary>
        VK_RETURN = 0X0D,
        /// <summary>
        /// SHIFT key
        /// </summary>
        VK_SHIFT  = 0X10,
        /// <summary>
        /// CONTROL key
        /// </summary>
        VK_CONTROL = 0X11,
        /// <summary>
        /// ALT key
        /// </summary>
        VK_MENU = 0X12,
        /// <summary>
        /// PAUSE key
        /// </summary>
        VK_PAUSE = 0X13,
        /// <summary>
        /// CAPS LOCK key
        /// </summary>
        VK_CAPITAL = 0X14,
        /// <summary>
        /// ESC key
        /// </summary>
        VK_ESCAPE = 0X1B,
        /// <summary>
        /// SPACEBAR
        /// </summary>
        VK_SPACE = 0X20,
        /// <summary>
        /// PAGE UP key
        /// </summary>
        VK_PRIOR = 0X21,
        /// <summary>
        /// PAGE DOWN key
        /// </summary>
        VK_NEXT = 0X22,
        /// <summary>
        /// END key
        /// </summary>
        VK_END = 0X23,
        /// <summary>
        /// HOME key
        /// </summary>
        VK_HOME = 0X24,
        /// <summary>
        /// LEFT ARROW key
        /// </summary>
        VK_LEFT = 0X25,
        /// <summary>
        /// UP ARROW key
        /// </summary>
        VK_UP = 0X26,
        /// <summary>
        /// RIGHT ARROW key
        /// </summary>
        VK_RIGHT = 0X27,
        /// <summary>
        /// DOWN ARROW key
        /// </summary>
        VK_DOWN = 0X28,
        /// <summary>
        /// SELECT key
        /// </summary>
        VK_SELECT = 0X29,
        /// <summary>
        /// PRINT key
        /// </summary>
        VK_PRINT = 0X2A,
        /// <summary>
        /// INS key
        /// </summary>
        VK_INSERT = 0X2D,
        /// <summary>
        /// DEL key
        /// </summary>
        VK_DELETE = 0X2E
    }

    public static class _Console
	{
        /// <summary>
        /// Other open operations can be performed on the console screen buffer for read access.
        /// </summary>
        public const uint FILE_SHARE_READ = 0x00000001;
        /// <summary>
        /// Other open operations can be performed on the console screen buffer for write access.
        /// </summary>
        public const uint FILE_SHARE_WRITE = 0x00000002;

        public const uint GENERIC_READ = 0x80000000;
		public const uint GENERIC_WRITE = 0x40000000;
		public const int CONSOLE_TEXTMODE_BUFFER = 1;

        public static class WINCON
        {
            public const short FOREGROUND_BLUE = 1;
            public const short FOREGROUND_GREEN = 2;
            public const short FOREGROUND_RED = 4;
            public const short FOREGROUND_INTENSITY = 8;
            public const short BACKGROUND_BLUE = 16;
            public const short BACKGROUND_GREEN = 32;
            public const short BACKGROUND_RED = 64;
            public const short BACKGROUND_INTENSITY = 128;
            public const short CTRL_C_EVENT = 0;
            public const short CTRL_BREAK_EVENT = 1;
            public const short CTRL_CLOSE_EVENT = 2;
            public const short CTRL_LOGOFF_EVENT = 5;
            public const short CTRL_SHUTDOWN_EVENT = 6;
            public const short ENABLE_LINE_INPUT = 2;
            public const short ENABLE_ECHO_INPUT = 4;
            public const short ENABLE_PROCESSED_INPUT = 1;
            public const short ENABLE_WINDOW_INPUT = 8;
            public const short ENABLE_MOUSE_INPUT = 16;
            public const short ENABLE_PROCESSED_OUTPUT = 1;
            public const short ENABLE_WRAP_AT_EOL_OUTPUT = 2;
            public const short KEY_EVENT = 1;
            public const short MOUSE_EVENT = 2;
            public const short WINDOW_BUFFER_SIZE_EVENT = 4;
            public const short MENU_EVENT = 8;
            public const short FOCUS_EVENT = 16;
            public const short CAPSLOCK_ON = 128;
            public const short ENHANCED_KEY = 256;
            public const short RIGHT_ALT_PRESSED = 1;
            public const short LEFT_ALT_PRESSED = 2;
            public const short RIGHT_CTRL_PRESSED = 4;
            public const short LEFT_CTRL_PRESSED = 8;
            public const short SHIFT_PRESSED = 16;
            public const short NUMLOCK_ON = 32;
            public const short SCROLLLOCK_ON = 64;
            public const short FROM_LEFT_1ST_BUTTON_PRESSED = 1;
            public const short RIGHTMOST_BUTTON_PRESSED = 2;
            public const short FROM_LEFT_2ND_BUTTON_PRESSED = 4;
            public const short FROM_LEFT_3RD_BUTTON_PRESSED = 8;
            public const short FROM_LEFT_4TH_BUTTON_PRESSED = 16;
            public const short MOUSE_MOVED = 1;
            public const short DOUBLE_CLICK = 2;
            public const short MOUSE_WHEELED = 4;
        }

        /// <summary>
        /// Sets the specified screen buffer to be the currently displayed console screen buffer.
        /// </summary>
        /// <param name="Handle">A handle to the console screen buffer.</param>
        /// <returns>If the function succeeds, the return value is nonzero. If the function fails, the return value is zero.To get extended error information, call GetLastError.</returns>
        [DllImport("Kernel32.dll")]
        public static extern bool SetConsoleActiveScreenBuffer(IntPtr Handle);

        /// <summary>
        /// Copies a number of characters to consecutive cells of a console screen buffer, beginning at a specified location.
        /// </summary>
        /// <param name="HANDLE">A handle to the console screen buffer. The handle must have the GENERIC_WRITE access right. For more information, see Console Buffer Security and Access Rights.</param>
        /// <param name="lpCharacter">The characters to be written to the console screen buffer.</param>
        /// <param name="nLenght">The number of characters to be written.</param>
        /// <param name="dwWriteCoord">A COORD structure that specifies the character coordinates of the first cell in the console screen buffer to which characters will be written.</param>
        /// <param name="lpNumberOfCharsWritten">A pointer to a variable that receives the number of characters actually written.</param>
        /// <returns>If the function succeeds, the return value is nonzero. If the function fails, the return value is zero.To get extended error information, call GetLastError.</returns>
        [DllImport("Kernel32.dll")]
        public static extern bool WriteConsoleOutputCharacter(
                IntPtr HANDLE,
                string lpCharacter,
                UInt32 nLenght,
                COORD dwWriteCoord,
                out UInt32 lpNumberOfCharsWritten
            );

        [DllImport("Kernel32.dll")]
        public static extern bool WriteConsoleOutputCharacter(
                IntPtr HANDLE,
                char[] lpCharacter,
                UInt32 nLenght,
                COORD dwWriteCoord,
                out UInt32 lpNumberOfCharsWritten
            );

        /// <summary>
        /// Creates a console screen buffer.
        /// </summary>
        /// <param name="dwDesiredAccess">The access to the console screen buffer. For a list of access rights, see "Console Buffer Security and Access Rights".</param>
        /// <param name="dwShareMode">This parameter can be zero, indicating that the buffer cannot be shared</param>
        /// <param name="lpSecurityAttributes">A pointer to a SECURITY_ATTRIBUTES structure that determines whether the returned handle can be inherited by child processes. If lpSecurityAttributes is NULL, the handle cannot be inherited. 
        ///     The lpSecurityDescriptor member of the structure specifies a security descriptor for the new console screen buffer. If lpSecurityAttributes is NULL, the console screen buffer gets a default security descriptor. 
        ///     The ACLs in the default security descriptor for a console screen buffer come from the primary or impersonation token of the creator.</param>
        /// <param name="dwFlags">The type of console screen buffer to create. The only supported screen buffer type is CONSOLE_TEXTMODE_BUFFER.</param>
        /// <param name="lpScreenBufferData">Reserved; should be NULL. (IntPtr.Zero)</param>
        /// <returns>If the function succeeds, the return value is a handle to the new console screen buffer. If the function fails, the return value is INVALID_HANDLE_VALUE. To get extended error information, call GetLastError.</returns>
        /// More Info: https://docs.microsoft.com/en-us/windows/console/createconsolescreenbuffer
        [DllImport("Kernel32.dll")]
        public static extern IntPtr CreateConsoleScreenBuffer(
                UInt32 dwDesiredAccess,
                UInt32 dwShareMode,
                IntPtr lpSecurityAttributes,
                int dwFlags,
                IntPtr lpScreenBufferData
            );

        [DllImport("User32.dll")]
        public static extern short GetAsyncKeyState(int Vkey);

        public static short GetAsyncKeyState(Buttons Button)
        {
            return GetAsyncKeyState((int)Button);
        }

        [DllImport("Kernel32.dll")]
        public static extern COORD GetLargestConsoleWindowSize(IntPtr hConsole);

        [DllImport("Kernel32.dll")]
        public static extern bool SetConsoleTextAttribute(IntPtr hConsole, ushort wAttributes);
        
        [DllImport("msvcrt.Dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int sprintf(
            [In, Out]StringBuilder buffer, 
            String fmt,
            String arg1);

        public static char[] WriteText(char[] buffer, int Pos, string Text)
        {
            if (Pos > buffer.Length || Pos < 0)
                throw new ArgumentException("Pos can't be bigger than buffer lenght", "Pos");

            for(int x=0; x < Text.Length; x++)
            {
                buffer[Pos++] = Text[x];
            }

            return buffer;
        }
    }
}
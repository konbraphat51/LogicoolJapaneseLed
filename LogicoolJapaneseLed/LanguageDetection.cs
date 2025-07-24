using System.Runtime.InteropServices;

namespace LogicoolJapaneseLed
{
    /// <summary>
    /// Detects English / Japanese currently inputing
    /// </summary>
    internal class LanguageDetection
    {
        // Win32 API declarations
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr lpdwProcessId);

        [DllImport("user32.dll")]
        static extern uint GetCurrentThreadId();

        [DllImport("user32.dll")]
        static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

        [DllImport("user32.dll")]
        static extern IntPtr GetFocus();

        [DllImport("imm32.dll")]
        static extern IntPtr ImmGetContext(IntPtr hWnd);

        [DllImport("imm32.dll")]
        static extern bool ImmGetConversionStatus(
            IntPtr hIMC,
            out int lpfdwConversion,
            out int lpfdwSentence
        );

        /// <summary>
        /// Gets the current input language.
        /// </summary>
        /// <returns>True if Japanese, false if else.
        /// + False if no input context available.
        /// </returns>
        public static (bool, bool) IsJapanese()
        {
            IntPtr immContext = GetImmContext();
            if (immContext == IntPtr.Zero)
            {
                // No input context available
                return (false, false);
            }

            (bool isAlphanumeric, bool isSuccess) = IsAlphanumeric(immContext);
            if (!isSuccess)
            {
                // Failed to determine if alphanumeric
                return (false, false);
            }

            return (!isAlphanumeric, true);
        }

        private static IntPtr GetFocusPtr()
        {
            IntPtr foregroundWindow = GetForegroundWindow();
            if (foregroundWindow == IntPtr.Zero)
            {
                return IntPtr.Zero;
            }

            uint threadId = GetWindowThreadProcessId(foregroundWindow, IntPtr.Zero);
            uint currentThreadId = GetCurrentThreadId();

            AttachThreadInput(currentThreadId, threadId, true);
            IntPtr focusHandle = GetFocus();
            AttachThreadInput(currentThreadId, threadId, false);

            return focusHandle;
        }

        private static IntPtr GetImmContext()
        {
            IntPtr hWnd = GetFocusPtr();
            if (hWnd == IntPtr.Zero)
            {
                return IntPtr.Zero;
            }

            return ImmGetContext(hWnd);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="immContext"></param>
        /// <returns>
        /// (isAlphanumeric, isSuccess)
        /// </returns>
        private static (bool, bool) IsAlphanumeric(IntPtr immContext)
        {
            const int IME_CMODE_ALPHANUMERIC = 0x0001;

            if (ImmGetConversionStatus(immContext, out int conversion, out _))
            {
                return ((conversion & IME_CMODE_ALPHANUMERIC) != 0, true);
            }
            else
            {
                // Failed to get conversion status
                return (false, false);
            }
        }
    }
}

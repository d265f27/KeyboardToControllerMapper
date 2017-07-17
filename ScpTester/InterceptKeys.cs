/* Class to read key presses via a Windows' hook.
 * Based on code from Stephen Toub/Microsoft 2006, see
 * https://blogs.msdn.microsoft.com/toub/2006/05/03/low-level-keyboard-hook-in-c/
 * or May 2006 MSDN Magazine article on Managed Debugging Assistants.
 * Used under Microsoft Limited Public License, any modifications from that source
 * are Copyright Elliot Dawber 2017, MIT License.
 */

using System;

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace KeyboardToControllerMapper
{
    public class InterceptKeys
    {
        
        // Low Level Keyboard Hook, see Windows' hook information, 
        private const int WH_KEYBOARD_LL = 13;
        // Callback for when we get a hit from Windows' hook.
        private static LowLevelKeyboardProc _proc = HookCallback;
        // Hook ID of our Windows' hook.
        private static IntPtr _hookID = IntPtr.Zero;


        /// <summary>
        /// The external delegate callback that we should send the key information to.
        /// </summary>
        private static KeyboardCallback ExternalCallback;


        /// <summary>
        /// Creates and connects the keyboard hooks.
        /// </summary>
        /// <param name="callback">The delegate callback we should send key information to.</param>
        public InterceptKeys(KeyboardCallback callback)
        {
            ExternalCallback = callback;
            Connect();
        }


        /// <summary>
        /// Connects the keyboard hooks.
        /// </summary>
        public static void Connect()
        {
            _hookID = SetHook(_proc);
        }


        /// <summary>
        /// Sets the keyboard hook.
        /// </summary>
        /// <param name="proc"></param>
        /// <returns></returns>
        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }


        /// <summary>
        /// Delegate for this InterceptKeys's internal keypress callback.
        /// </summary>
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        /// <summary>
        /// Delegate for this InterceptKeys's external keypress callback.
        /// </summary>
        public delegate void KeyboardCallback(IntPtr wParam, IntPtr lParam);

        
        /// <summary>
        /// Internal keypress callback, the hook calls this when a key state alters.
        /// </summary>
        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                ExternalCallback(wParam, lParam);
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }


        // Windows DLL calls.
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}

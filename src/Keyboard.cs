using System;
using System.Runtime.InteropServices;
using System.Threading;
using Robot.NET.Native;

namespace Robot.NET
{
    /// <summary>
    /// 
    /// </summary>
    public static class Keyboard
    {
        private static int keyboardDelay = 10;
        private static readonly INativeImplementation impl;
        static Keyboard()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                impl = OSX.Instance;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                impl = Windows.Instance;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                impl = Linux.Instance;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="milliseconds"></param>
        public static void SetKeyboardDelay(int milliseconds)
        {
            Interlocked.Exchange(ref keyboardDelay, milliseconds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="modifier"></param>
        public static void KeyTap(char key, KeyModifiers modifier = KeyModifiers.None)
        {
            impl.TapKeyCode(key, modifier);
            Thread.Sleep(keyboardDelay);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="modifier"></param>
        public static void KeyTap(Keys key, KeyModifiers modifier = KeyModifiers.None)
        {
            impl.TapKeyCode(key, modifier);
            Thread.Sleep(keyboardDelay);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="down"></param>
        /// <param name="modifier"></param>
        public static void KeyToggle(char key, bool down, KeyModifiers modifier = KeyModifiers.None)
        {
            impl.ToggleKeyCode(key, down, modifier);
            Thread.Sleep(keyboardDelay);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="down"></param>
        /// <param name="modifier"></param>
        public static void KeyTtoggle(Keys key, bool down, KeyModifiers modifier = KeyModifiers.None)
        {
            impl.ToggleKeyCode(key, down, modifier);
            Thread.Sleep(keyboardDelay);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        public static void TypeString(string str)
        {
            impl.TypeString(str);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="cpm"></param>
        public static void TypeStringDelayed(string str, int cpm)
        {
            impl.TypeStringDelayed(str, cpm);
        }
    }
}
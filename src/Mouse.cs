using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using Robot.NET.Native;

namespace Robot.NET
{
    /// <summary>
    /// 
    /// </summary>
    public static class Mouse
    {
        private static int mouseDelay = 10;
        private static readonly INativeImplementation impl;
        static Mouse()
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
        public static void SetMouseDelay(int milliseconds)
        {
            Interlocked.Exchange(ref mouseDelay, milliseconds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void MoveMouse(int x, int y)
        {
            var point = new Point(x, y);
            impl.MoveMouse(point);
            Thread.Sleep(mouseDelay);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void MoveMouseSmooth(int x, int y)
        {
            var point = new Point(x, y);
            impl.SmoothlyMoveMouse(point);
            Thread.Sleep(mouseDelay);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="button"></param>
        /// <param name="doubleClick"></param>
        public static void MouseClick(MouseButtons button = MouseButtons.Left, bool doubleClick = false)
        {
            if (doubleClick)
            {
                impl.DoubleClick(button);
            }
            else
            {
                impl.ClickMouse(button);
            }
            Thread.Sleep(mouseDelay);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="down"></param>
        /// <param name="button"></param>
        public static void MouseToggle(bool down = true, MouseButtons button = MouseButtons.Left)
        {
            impl.ToggleMouse(down, button);
            Thread.Sleep(mouseDelay);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void DragMouse(int x, int y, MouseButtons button = MouseButtons.Left)
        {
            var point = new Point(x, y);
            impl.DragMouse(point, button);
            Thread.Sleep(mouseDelay);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Point GetMousePos()
        {
            return impl.GetMousePos();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void ScrollMouse(int x, int y)
        {
            impl.ScrollMouse(x, y);
            Thread.Sleep(mouseDelay);
        }
    }
}
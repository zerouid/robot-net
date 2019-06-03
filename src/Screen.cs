using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Robot.NET.Native;

namespace Robot.NET
{
    /// <summary>
    /// 
    /// </summary>
    public static class Screen
    {
        private static readonly INativeImplementation impl;
        static Screen()
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
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Color GetPixelColor(int x, int y)
        {
            Point point = new Point(x, y);
            if (!impl.PointVisibleOnMainDisplay(point))
            {
                throw new ArgumentOutOfRangeException(nameof(point), "Requested coordinates are outside the main screen's dimensions.");
            }
            using (Bitmap bmp = impl.CopyBitmapFromDisplayInRect(new Rectangle(point, new Size(1, 1))))
            {
                return bmp.GetPixel(0, 0);
            }
        }

        public static Size GetScreenSize()
        {
            return impl.GetMainDisplaySize();
        }

        public static Bitmap CaptureScreen(int x = 0, int y = 0, int width = -1, int height = -1)
        {
            if (width <= 0 || height <= 0)
            {
                Size s = impl.GetMainDisplaySize();
                if (width <= 0) width = s.Width;
                if (height <= 0) height = s.Height;
            }

            Bitmap bmp = impl.CopyBitmapFromDisplayInRect(new Rectangle(x, y, width, height));
            return bmp;
        }
    }
}
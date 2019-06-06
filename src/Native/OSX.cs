using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace Robot.NET.Native
{
    internal class OSX : INativeImplementation
    {
        private static Dictionary<char, ushort> charToKeyCodeMap;
        private static IntPtr eventDrvrRef = IntPtr.Zero;

        private OSX() { }
        public static INativeImplementation Instance { get; } = new OSX();
        private struct CGPoint
        {
            public double x;
            public double y;

            public static implicit operator CGPoint(Point p)
            {
                return new CGPoint { x = p.X, y = p.Y };
            }
            public static implicit operator Point(CGPoint p)
            {
                return new Point((int)p.x, (int)p.y);
            }
        }
        struct CGSize
        {
            public double width;
            public double height;

            public static implicit operator CGSize(Size p)
            {
                return new CGSize { width = p.Width, height = p.Height };
            }
            public static implicit operator Size(CGSize p)
            {
                return new Size((int)p.width, (int)p.height);
            }
        };
        private struct CGRect
        {
            public CGPoint origin;
            public CGSize size;

            public static implicit operator CGRect(Rectangle p)
            {
                return new CGRect { origin = p.Location, size = p.Size };
            }
            public static implicit operator Rectangle(CGRect p)
            {
                return new Rectangle(p.origin, p.size);
            }
        }

        private struct IOGPoint
        {
            public short x;
            public short y;
        };

        private struct CFRange
        {
            public IntPtr location;
            public IntPtr length;
        }

#pragma warning disable CS0169, CS0649
        [StructLayout(LayoutKind.Explicit)]
        private struct NXEventData
        {
            public struct Compound
            {   /* For window-changed, sys-defined, and app-defined events */

                public short reserved;
                public short subType;  /* event subtype for compound events */
                [StructLayout(LayoutKind.Explicit)]
                public struct Misc
                {
                    [FieldOffset(0)]
                    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
                    public float[] F; /* for use in compound events */
                    [FieldOffset(0)]
                    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
                    public long[] L;  /* for use in compound events */
                    [FieldOffset(0)]
                    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
                    public short[] S; /* for use in compound events */
                    [FieldOffset(0)]
                    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
                    public char[] C;  /* for use in compound events */
                }
                public Misc misc;
                int reserved2;
                int reserved3;
                int reserved4;
                int reserved5;
                int reserved6;
            }
            [FieldOffset(0)]
            public Compound compound;
        }
#pragma warning restore CS0169, CS0649


        [DllImport(OSXDefinitions.CoreGraphicsFrameworkPath)]
        private static extern IntPtr CGEventCreateMouseEvent(IntPtr source, uint mouseType, CGPoint mouseCursorPosition, uint mouseButton);

        [DllImport(OSXDefinitions.CoreGraphicsFrameworkPath)]
        private static extern IntPtr CGEventCreateKeyboardEvent(IntPtr source, ushort virtualKey, bool keyDown);
        [DllImport(OSXDefinitions.CoreGraphicsFrameworkPath)]
        private static extern void CGEventSetFlags(IntPtr @event, ulong flags);

        [DllImport(OSXDefinitions.CoreGraphicsFrameworkPath)]
        private static extern void CGEventPost(uint tap, IntPtr @event);

        [DllImport(OSXDefinitions.CoreGraphicsFrameworkPath)]
        private static extern IntPtr CGEventCreate(IntPtr source);

        [DllImport(OSXDefinitions.CoreGraphicsFrameworkPath)]
        private static extern CGPoint CGEventGetLocation(IntPtr @event);

        [DllImport(OSXDefinitions.CoreGraphicsFrameworkPath)]
        private static extern void CGEventSetIntegerValueField(IntPtr @event, uint field, long value);

        [DllImport(OSXDefinitions.CoreGraphicsFrameworkPath)]
        private static extern void CGEventSetType(IntPtr @event, uint type);

        [DllImport(OSXDefinitions.CoreGraphicsFrameworkPath)]
        private static extern IntPtr CGEventCreateScrollWheelEvent(IntPtr source, uint units, uint wheelCount, int wheel1, int wheel2);

        [DllImport(OSXDefinitions.CoreGraphicsFrameworkPath)]
        private static extern uint CGMainDisplayID();

        [DllImport(OSXDefinitions.CoreGraphicsFrameworkPath)]
        private static extern int CGDisplayPixelsWide(uint display);

        [DllImport(OSXDefinitions.CoreGraphicsFrameworkPath)]
        private static extern int CGDisplayPixelsHigh(uint display);

        [DllImport(OSXDefinitions.CoreGraphicsFrameworkPath)]
        private static extern IntPtr CGDataProviderCopyData(IntPtr provider);

        [DllImport(OSXDefinitions.CoreGraphicsFrameworkPath)]
        private static extern IntPtr CGImageGetDataProvider(IntPtr image);

        [DllImport(OSXDefinitions.CoreGraphicsFrameworkPath)]
        private static extern void CGImageRelease(IntPtr image);

        [DllImport(OSXDefinitions.CoreGraphicsFrameworkPath)]
        private extern static void CGEventKeyboardSetUnicodeString(IntPtr @event, uint stringLength, [MarshalAs(UnmanagedType.LPWStr)] string unicodeString);

        [DllImport(OSXDefinitions.CoreGraphicsFrameworkPath)]
        private extern static IntPtr CGDisplayCreateImageForRect(uint display, CGRect rect);
        [DllImport(OSXDefinitions.CoreGraphicsFrameworkPath)]
        private static extern int CGImageGetWidth(IntPtr image);

        [DllImport(OSXDefinitions.CoreGraphicsFrameworkPath)]
        private static extern int CGImageGetHeight(IntPtr image);

        [DllImport(OSXDefinitions.CoreGraphicsFrameworkPath)]
        private static extern int CGImageGetBytesPerRow(IntPtr image);

        [DllImport(OSXDefinitions.CoreGraphicsFrameworkPath)]
        private static extern int CGImageGetBitsPerPixel(IntPtr image);

        [DllImport(OSXDefinitions.CoreGraphicsFrameworkPath)]
        private static extern void CGDataProviderRelease(IntPtr provider);

        [DllImport(OSXDefinitions.CoreFoundationFrameworkPath)]
        private extern static void CFRelease(IntPtr ptr);

        [DllImport(OSXDefinitions.CoreFoundationFrameworkPath)]
        extern static int CFDataGetLength(IntPtr handle);

        [DllImport(OSXDefinitions.CoreFoundationFrameworkPath)]
        extern static void CFDataGetBytes(IntPtr handle, CFRange range, IntPtr buffer);

        [DllImport(OSXDefinitions.CarbonFrameworkPath)]
        private extern static IntPtr TISGetInputSourceProperty(IntPtr inputSource, IntPtr propertyKey);

        [DllImport(OSXDefinitions.CarbonFrameworkPath)]
        private extern static IntPtr TISCopyCurrentKeyboardLayoutInputSource();

        [DllImport(OSXDefinitions.CarbonFrameworkPath)]
        private extern static IntPtr __CFStringMakeConstantString(string cString);

        [DllImport(OSXDefinitions.CarbonFrameworkPath)]
        private static extern IntPtr CFDataGetBytePtr(IntPtr data);

        [DllImport(OSXDefinitions.CarbonFrameworkPath)]
        private extern static Int32 UCKeyTranslate(IntPtr keyLayoutPtr, UInt16 virtualKeyCode, UInt16 keyAction, UInt32 modifierKeyState, UInt32 keyboardType, UInt32 keyTranslateOptions, ref UInt32 deadKeyState, UInt32 maxStringLength, ref UInt32 actualStringLength, IntPtr unicodeString);

        [DllImport(OSXDefinitions.CarbonFrameworkPath)]
        private static extern byte LMGetKbdType();

        [DllImport(OSXDefinitions.IOKitFrameworkPath)]
        private static extern int IOMasterPort(IntPtr bootstrapPort, out IntPtr masterPort);

        [DllImport(OSXDefinitions.IOKitFrameworkPath)]
        private static extern int IOServiceGetMatchingServices(
                    IntPtr masterPort, IntPtr matchingDictionary, out IntPtr iterator);

        [DllImport(OSXDefinitions.IOKitFrameworkPath)]
        private static extern IntPtr IOServiceMatching(string name);

        [DllImport(OSXDefinitions.IOKitFrameworkPath)]
        private static extern IntPtr IOIteratorNext(IntPtr iterator);

        [DllImport(OSXDefinitions.IOKitFrameworkPath)]
        private static extern int IOServiceOpen(IntPtr service, IntPtr owningTask, UInt32 type, out IntPtr connect);

        [DllImport(OSXDefinitions.IOKitFrameworkPath)]
        private static extern IntPtr mach_task_self();

        [DllImport(OSXDefinitions.IOKitFrameworkPath)]
        private static extern void IOObjectRelease(IntPtr @object);

        [DllImport(OSXDefinitions.IOKitFrameworkPath)]
        private static extern int IOHIDPostEvent(IntPtr connect, uint eventType, IOGPoint location, NXEventData eventData,
                                                uint eventDataVersion, uint eventFlags, uint options);

        private static void calculateDeltas(IntPtr @event, Point point)
        {
            /**
             * The next few lines are a workaround for games not detecting mouse moves.
             * See this issue for more information:
             * https://github.com/octalmage/robotjs/issues/159
             */
            IntPtr get = CGEventCreate(IntPtr.Zero);
            CGPoint mouse = CGEventGetLocation(get);

            // Calculate the deltas.
            long deltaX = (long)(point.X - mouse.x);
            long deltaY = (long)(point.Y - mouse.y);

            CGEventSetIntegerValueField(@event, OSXDefinitions.kCGMouseEventDeltaX, deltaX);
            CGEventSetIntegerValueField(@event, OSXDefinitions.kCGMouseEventDeltaY, deltaY);

            CFRelease(get);
        }
        private static Dictionary<char, ushort> getCharToKeyCodeMap()
        {
            if (charToKeyCodeMap == null)
            {
                charToKeyCodeMap = new Dictionary<char, ushort>();
                for (ushort key = 0; key < 128; key++)
                {
                    var str = createStringForKey(key);
                    if (str?.Length == 1 && !charToKeyCodeMap.ContainsKey(str[0]))
                    {
                        charToKeyCodeMap.Add(str[0], key);
                    }
                }
            }
            return charToKeyCodeMap;
        }
        private static string createStringForKey(ushort keyCode)
        {
            IntPtr currentKeyboard = TISCopyCurrentKeyboardLayoutInputSource();
            var layoutData = TISGetInputSourceProperty(currentKeyboard, __CFStringMakeConstantString("TISPropertyUnicodeKeyLayoutData"));
            var keyboardLayout = CFDataGetBytePtr(layoutData);

            uint flags = 0;
            var modifierKeyState = (flags >> 16) & 0xFF;
            var chars = new char[4];
            UInt32 realLength = 0;

            int size = sizeof(UInt16) * chars.Length;
            IntPtr buffer = IntPtr.Zero;
            UInt32 deadKeyState = 0;

            try
            {
                buffer = Marshal.AllocCoTaskMem(size);
                UCKeyTranslate(keyboardLayout, keyCode, OSXDefinitions.kUCKeyActionDisplay, modifierKeyState, LMGetKbdType(), 0,
                    ref deadKeyState, (uint)chars.Length, ref realLength, buffer);

                if (realLength != 0)
                    Marshal.Copy(buffer, chars, 0, chars.Length);

                //Debug.WriteLine("DeadKeyState = {0:X8}", deadKeyState);
                return new string(chars, 0, (int)realLength);
            }
            finally
            {
                if (buffer != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(buffer);
            }
        }

        private static void toggleKeyCode(uint code, bool down, KeyModifiers flags)
        {
            const uint NX_SYSDEFINED = 14;
            const short NX_SUBTYPE_AUX_CONTROL_BUTTONS = 8;
            const uint kNXEventDataVersion = 2;
            /* The media keys all have 1000 added to them to help us detect them. */
            if (code >= 1000)
            {
                code = code - 1000; /* Get the real keycode. */
                NXEventData eventData = new NXEventData();
                //         kern_return_t kr;
                IOGPoint loc = new IOGPoint() { x = 0, y = 0 };
                uint evtInfo = code << 16 | (down ? OSXDefinitions.kCGEventKeyDown : OSXDefinitions.kCGEventKeyUp) << 8;
                // 		bzero(&event, sizeof(NXEventData));
                eventData.compound.subType = NX_SUBTYPE_AUX_CONTROL_BUTTONS;
                eventData.compound.misc.L[0] = evtInfo;
                int kr = IOHIDPostEvent(getAuxiliaryKeyDriver(), NX_SYSDEFINED, loc, eventData, kNXEventDataVersion, 0, 0);
                if (kr != 0)
                {
                    throw new Exception($"IOKit returned error: 0x{kr:x8}.");
                }
            }
            else
            {
                IntPtr keyEvent = CGEventCreateKeyboardEvent(IntPtr.Zero, (ushort)code, down);
                //     assert(keyEvent != NULL);

                CGEventSetType(keyEvent, down ? OSXDefinitions.kCGEventKeyDown : OSXDefinitions.kCGEventKeyUp);
                CGEventSetFlags(keyEvent, flags.ToCGEventFlags());
                CGEventPost(OSXDefinitions.kCGSessionEventTap, keyEvent);
                CFRelease(keyEvent);
            }
        }

        private static IntPtr getAuxiliaryKeyDriver()
        {
            if (eventDrvrRef == IntPtr.Zero)
            {
                Action<int> assert = (result) =>
                {
                    if (result != 0)
                    {
                        throw new Exception($"IOKit returned error: 0x{result:x8}.");
                    }
                };
                IntPtr service = IntPtr.Zero;
                IntPtr iterator = IntPtr.Zero;
                try
                {
                    int kr = IOMasterPort(IntPtr.Zero, out IntPtr masterPort);
                    assert(kr);
                    kr = IOServiceGetMatchingServices(masterPort, IOServiceMatching("IOHIDSystem"), out iterator);
                    assert(kr);
                    service = IOIteratorNext(iterator);
                    kr = IOServiceOpen(service, mach_task_self(), OSXDefinitions.kIOHIDParamConnectType, out eventDrvrRef);
                    assert(kr);
                }
                finally
                {
                    if (service != IntPtr.Zero) IOObjectRelease(service);
                    if (iterator != IntPtr.Zero) IOObjectRelease(iterator);
                }

            }
            return eventDrvrRef;
        }

        private static void toggleUnicodeKey(char ch, bool down)
        {
            /* This function relies on the convenient
             * CGEventKeyboardSetUnicodeString(), which allows us to not have to
             * convert characters to a keycode, but does not support adding modifier
             * flags. It is therefore only used in typeString() and typeStringDelayed()
             * -- if you need modifier keys, use the above functions instead. */
            IntPtr keyEvent = CGEventCreateKeyboardEvent(IntPtr.Zero, 0, down);
            if (keyEvent == IntPtr.Zero)
            {
                throw new Exception("Could not create keyboard event.");
            }

            // if (ch > 0xFFFF)
            // {
            //     // encode to utf-16 if necessary
            //     UniChar surrogates[2] = {
            //         0xD800 + ((ch - 0x10000) >> 10),
            //         0xDC00 + (ch & 0x3FF)
            //     };

            //     CGEventKeyboardSetUnicodeString(keyEvent, 2, (UniChar*)&surrogates);
            // }
            // else
            // {
            CGEventKeyboardSetUnicodeString(keyEvent, 1, $"{ch}");
            // }

            CGEventPost(OSXDefinitions.kCGSessionEventTap, keyEvent);
            CFRelease(keyEvent);
        }

        /*
         * A crude, fast hypot() approximation to get around the fact that hypot() is
         * not a standard ANSI C function.
         *
         * It is not particularly accurate but that does not matter for our use case.
         *
         * Taken from this StackOverflow answer:
         * http://stackoverflow.com/questions/3506404/fast-hypotenuse-algorithm-for-embedded-processor#3507882
         *
         */
        private static double crude_hypot(double x, double y)
        {
            double big = Math.Abs(x); /* max(|x|, |y|) */
            double small = Math.Abs(y); /* min(|x|, |y|) */

            if (big > small)
            {
                double temp = big;
                big = small;
                small = temp;
            }

            const double M_SQRT2 = 1.4142135623730950488016887;

            return ((M_SQRT2 - 1.0) * small) + big;
        }


        public void MoveMouse(Point point)
        {
            IntPtr move = CGEventCreateMouseEvent(IntPtr.Zero, OSXDefinitions.kCGEventMouseMoved,
                                              point,
                                              OSXDefinitions.kCGMouseButtonLeft);

            calculateDeltas(move, point);

            CGEventPost(OSXDefinitions.kCGSessionEventTap, move);
            CFRelease(move);
        }

        public void DragMouse(Point point, MouseButtons button)
        {
            IntPtr drag = CGEventCreateMouseEvent(IntPtr.Zero, button.MouseDragToCGEventType(),
                                                            point,
                                                            button.ToCGMouseButton());
            calculateDeltas(drag, point);

            CGEventPost(OSXDefinitions.kCGSessionEventTap, drag);
            CFRelease(drag);
        }

        public Point GetMousePos()
        {
            IntPtr evt = CGEventCreate(IntPtr.Zero);
            CGPoint point = CGEventGetLocation(evt);
            CFRelease(evt);

            return point;

        }

        public void ToggleMouse(bool down, MouseButtons button)
        {
            CGPoint currentPos = GetMousePos();
            IntPtr evt = CGEventCreateMouseEvent(IntPtr.Zero,
                                                       button.MouseToCGEventType(down),
                                                       currentPos,
                                                       button.ToCGMouseButton());
            CGEventPost(OSXDefinitions.kCGSessionEventTap, evt);
            CFRelease(evt);
        }

        public void ClickMouse(MouseButtons button)
        {
            ToggleMouse(true, button);
            ToggleMouse(false, button);
        }
        public void DoubleClick(MouseButtons button)
        {
            /* Double click for Mac. */
            CGPoint currentPos = GetMousePos();
            uint mouseTypeDown = button.MouseToCGEventType(true);
            uint mouseTypeUP = button.MouseToCGEventType(false);

            IntPtr evt = CGEventCreateMouseEvent(IntPtr.Zero, mouseTypeDown, currentPos, OSXDefinitions.kCGMouseButtonLeft);

            /* Set event to double click. */
            CGEventSetIntegerValueField(evt, OSXDefinitions.kCGMouseEventClickState, 2);

            CGEventPost(OSXDefinitions.kCGHIDEventTap, evt);

            CGEventSetType(evt, mouseTypeUP);
            CGEventPost(OSXDefinitions.kCGHIDEventTap, evt);

            CFRelease(evt);
        }

        public void ScrollMouse(int x, int y)
        {
            IntPtr evt = CGEventCreateScrollWheelEvent(IntPtr.Zero, OSXDefinitions.kCGScrollEventUnitPixel, 2, y, x);
            CGEventPost(OSXDefinitions.kCGHIDEventTap, evt);

            CFRelease(evt);
        }

        public bool SmoothlyMoveMouse(Point endPoint)
        {
            Point pos = GetMousePos();
            Size screenSize = GetMainDisplaySize();
            double velo_x = 0.0, velo_y = 0.0;
            double distance;
            Random rng = new Random();

            while ((distance = crude_hypot((double)pos.X - endPoint.X,
                                           (double)pos.Y - endPoint.Y)) > 1.0)
            {
                double gravity = 5.0 + rng.NextDouble() * 495.0; // DEADBEEF_UNIFORM(5.0, 500.0);
                double veloDistance;
                velo_x += (gravity * ((double)endPoint.X - pos.X)) / distance;
                velo_y += (gravity * ((double)endPoint.Y - pos.Y)) / distance;

                /* Normalize velocity to get a unit vector of length 1. */
                veloDistance = crude_hypot(velo_x, velo_y);
                velo_x /= veloDistance;
                velo_y /= veloDistance;

                pos.X += (int)Math.Floor(velo_x + 0.5);
                pos.Y += (int)Math.Floor(velo_y + 0.5);

                /* Make sure we are in the screen boundaries!
                 * (Strange things will happen if we are not.) */
                if (pos.X >= screenSize.Width || pos.Y >= screenSize.Height)
                {
                    return false;
                }

                MoveMouse(pos);

                /* Wait 1 - 3 milliseconds. */
                Thread.Sleep(rng.Next(1, 3));
                //microsleep(DEADBEEF_UNIFORM(1.0, 3.0));
            }

            return true;
        }

        public Size GetMainDisplaySize()
        {
            uint displayID = CGMainDisplayID();
            return new Size(CGDisplayPixelsWide(displayID),
                              CGDisplayPixelsHigh(displayID));
        }

        public void TapKeyCode(char key, KeyModifiers modifier)
        {
            if (getCharToKeyCodeMap().TryGetValue(key, out ushort code))
            {
                toggleKeyCode(code, true, modifier);
                toggleKeyCode(code, false, modifier);
            }
        }

        public void TapKeyCode(Keys key, KeyModifiers modifier)
        {
            var code = key.ToVKeyCode();
            toggleKeyCode(code, true, modifier);
            toggleKeyCode(code, false, modifier);
        }

        public void ToggleKeyCode(char key, bool down, KeyModifiers modifier)
        {
            if (getCharToKeyCodeMap().TryGetValue(key, out ushort code))
            {
                toggleKeyCode(code, down, modifier);
            }
        }

        public void ToggleKeyCode(Keys key, bool down, KeyModifiers modifier)
        {
            var code = key.ToVKeyCode();
            toggleKeyCode(code, down, modifier);
        }

        public void TypeString(string str)
        {
            foreach (char c in str)
            {
                toggleUnicodeKey(c, true);
                toggleUnicodeKey(c, false);
            }
        }

        public void TypeStringDelayed(string str, int cpm)
        {
            double cps = cpm / 60.0;
            double mspc = (cps == 0.0) ? 0.0 : 1000.0 / cps;
            Random rnd = new Random();
            foreach (char c in str)
            {
                toggleUnicodeKey(c, true);
                toggleUnicodeKey(c, false);
                Thread.Sleep((int)(mspc + rnd.Next(0, 62)));
            }
        }

        public bool PointVisibleOnMainDisplay(Point point)
        {
            var displaySize = GetMainDisplaySize();
            return point.X < displaySize.Width && point.Y < displaySize.Height;
        }

        public Bitmap CopyBitmapFromDisplayInRect(Rectangle rectangle)
        {
            uint displayID = CGMainDisplayID();
            IntPtr image = CGDisplayCreateImageForRect(displayID, rectangle);
            if (image == IntPtr.Zero)
            {
                return null;
            }

            IntPtr prov = CGImageGetDataProvider(image);
            IntPtr imageData = CGDataProviderCopyData(prov);

            long bufferSize = (long)CFDataGetLength(imageData);
            byte[] buffer = new byte[bufferSize];
            CFRange range = new CFRange { location = IntPtr.Zero, length = (IntPtr)bufferSize };
            unsafe
            {
                fixed (byte* bufptr = buffer)
                {
                    CFDataGetBytes(imageData, range, (IntPtr)bufptr);
                }
            }
            var width = CGImageGetWidth(image);
            var height = CGImageGetHeight(image);
            var bpp = CGImageGetBitsPerPixel(image);
            // var bpr = CGImageGetBytesPerRow(image);

            Bitmap bitmap = new Bitmap(width, height);


            BitmapData bmpdata = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
            ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);

            Marshal.Copy(buffer, 0, bmpdata.Scan0, buffer.Length);
            bitmap.UnlockBits(bmpdata);

            CFRelease(imageData);
            CGImageRelease(image);
            return bitmap;
        }
    }
}
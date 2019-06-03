using System;

namespace Robot.NET.Native
{
    public static class OSXExtensions
    {
        public static uint ToCGMouseButton(this MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.Right:
                    return OSXDefinitions.kCGMouseButtonRight;
                case MouseButtons.Middle:
                    return OSXDefinitions.kCGMouseButtonCenter;
                case MouseButtons.Left:
                default:
                    return OSXDefinitions.kCGMouseButtonLeft;
            }
        }

        public static uint MouseDragToCGEventType(this MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.Left:
                    return OSXDefinitions.kCGEventLeftMouseDragged;
                case MouseButtons.Middle:
                    return OSXDefinitions.kCGEventRightMouseDragged;
                default:
                    return OSXDefinitions.kCGEventOtherMouseDragged;
            }
        }
        public static uint MouseToCGEventType(this MouseButtons button, bool down)
        {
            switch (button)
            {
                case MouseButtons.Left:
                    return down ? OSXDefinitions.kCGEventLeftMouseDown : OSXDefinitions.kCGEventLeftMouseUp;
                case MouseButtons.Middle:
                    return down ? OSXDefinitions.kCGEventRightMouseDown : OSXDefinitions.kCGEventRightMouseUp;
                default:
                    return down ? OSXDefinitions.kCGEventOtherMouseDown : OSXDefinitions.kCGEventOtherMouseUp;
            }
        }

        public static ulong ToCGEventFlags(this KeyModifiers modifiers)
        {
            ulong flags = 0;
            if (modifiers.HasFlag(KeyModifiers.Command)) flags |= OSXDefinitions.kCGEventFlagMaskCommand;
            if (modifiers.HasFlag(KeyModifiers.Alt)) flags |= OSXDefinitions.kCGEventFlagMaskAlternate;
            if (modifiers.HasFlag(KeyModifiers.Control)) flags |= OSXDefinitions.kCGEventFlagMaskControl;
            if (modifiers.HasFlag(KeyModifiers.Shift)) flags |= OSXDefinitions.kCGEventFlagMaskShift;
            return flags;
        }

        public static uint ToVKeyCode(this Keys key)
        {
            switch (key)
            {
                case Keys.Backspace: return 0x33;
                case Keys.Delete: return 0x75;
                case Keys.Enter: return 0x24;
                case Keys.Tab: return 0x30;
                case Keys.Escape: return 0x35;
                case Keys.Up: return 0x7E;
                case Keys.Down: return 0x7D;
                case Keys.Right: return 0x7C;
                case Keys.Left: return 0x7B;
                case Keys.Home: return 0x73;
                case Keys.End: return 0x77;
                case Keys.PageUp: return 0x74;
                case Keys.PageDown: return 0x79;
                case Keys.F1: return 0x7A;
                case Keys.F2: return 0x78;
                case Keys.F3: return 0x63;
                case Keys.F4: return 0x76;
                case Keys.F5: return 0x60;
                case Keys.F6: return 0x61;
                case Keys.F7: return 0x62;
                case Keys.F8: return 0x64;
                case Keys.F9: return 0x65;
                case Keys.F10: return 0x6D;
                case Keys.F11: return 0x67;
                case Keys.F12: return 0x6F;
                case Keys.Command: return 0x37;
                case Keys.Alt: return 0x3A;
                case Keys.Control: return 0x3B;
                case Keys.Shift: return 0x38;
                case Keys.Right_Shift: return 0x3C;
                case Keys.Space: return 0x31;
                case Keys.Audio_Mute: return 1007;
                case Keys.Audio_Vol_Down: return 1001;
                case Keys.Audio_Vol_Up: return 1000;
                case Keys.Audio_Play: return 1016;
                case Keys.Audio_Pause: return 1016;
                case Keys.Audio_Prev: return 1018;
                case Keys.Audio_Next: return 1017;
                case Keys.Numpad_0: return 0x52;
                case Keys.Numpad_1: return 0x53;
                case Keys.Numpad_2: return 0x54;
                case Keys.Numpad_3: return 0x55;
                case Keys.Numpad_4: return 0x56;
                case Keys.Numpad_5: return 0x57;
                case Keys.Numpad_6: return 0x58;
                case Keys.Numpad_7: return 0x59;
                case Keys.Numpad_8: return 0x5B;
                case Keys.Numpad_9: return 0x5C;
                case Keys.Lights_Mon_Up: return 1002;
                case Keys.Lights_Mon_Down: return 1003;
                case Keys.Lights_Kbd_Toggle: return 1023;
                case Keys.Lights_Kbd_Up: return 1021;
                case Keys.Lights_Kbd_Down: return 1022;
                default:
                    throw new NotSupportedException();

            }
        }
    }
}
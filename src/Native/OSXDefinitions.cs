using System;

namespace Robot.NET.Native
{
    internal static class OSXDefinitions
    {
        #region Mouse Buttons
        internal const uint kCGMouseButtonLeft = 0;
        internal const uint kCGMouseButtonRight = 1;
        internal const uint kCGMouseButtonCenter = 2;
        #endregion Mouse Buttons
        #region MouseEvents
        internal const uint kCGEventLeftMouseDown = 1;
        internal const uint kCGEventLeftMouseUp = 2;
        internal const uint kCGEventRightMouseDown = 3;
        internal const uint kCGEventRightMouseUp = 4;
        internal const uint kCGEventMouseMoved = 5;
        internal const uint kCGEventLeftMouseDragged = 6;
        internal const uint kCGEventRightMouseDragged = 7;
        internal const uint kCGEventKeyDown = 10;
        internal const uint kCGEventKeyUp = 11;
        internal const uint kCGEventOtherMouseDown = 25;
        internal const uint kCGEventOtherMouseUp = 26;
        internal const uint kCGEventOtherMouseDragged = 27;
        #endregion MouseEvents
        #region Misc
        internal const uint kCGSessionEventTap = 1;
        internal const uint kCGHIDEventTap = 0;
        internal const uint kCGMouseEventClickState = 1;
        internal const uint kCGMouseEventDeltaX = 4;
        internal const uint kCGMouseEventDeltaY = 5;
        internal const uint kCGScrollEventUnitPixel = 0;

        internal const UInt16 kUCKeyActionDown = 0;
        internal const UInt16 kUCKeyActionUp = 1;
        internal const UInt16 kUCKeyActionAutoKey = 2;
        internal const UInt16 kUCKeyActionDisplay = 3;

        internal const uint kIOHIDParamConnectType = 1;

        internal const ulong kCGEventFlagMaskCommand = 0x00100000;
        internal const ulong kCGEventFlagMaskAlternate = 0x00080000;
        internal const ulong kCGEventFlagMaskControl = 0x00040000;
        internal const ulong kCGEventFlagMaskShift = 0x00020000;
        #endregion Misc
        #region Library Paths
        internal const string CoreGraphicsFrameworkPath = "/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics";
        internal const string CarbonFrameworkPath = "/System/Library/Frameworks/Carbon.framework/Versions/Current/Carbon";
        internal const string IOKitFrameworkPath = "/System/Library/Frameworks/IOKit.framework/IOKit";
        internal const string CoreFoundationFrameworkPath = "/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation";
        #endregion Library Paths
    }

}
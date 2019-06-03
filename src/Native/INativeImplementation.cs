using System.Drawing;

namespace Robot.NET.Native
{
    public interface INativeImplementation
    {
        void MoveMouse(Point point);
        void DragMouse(Point point, MouseButtons button);
        Point GetMousePos();
        void ToggleMouse(bool down, MouseButtons button);
        void ClickMouse(MouseButtons button);
        void DoubleClick(MouseButtons button);
        void ScrollMouse(int x, int y);
        bool SmoothlyMoveMouse(Point endPoint);
        Size GetMainDisplaySize();
        void TapKeyCode(char key, KeyModifiers modifier);
        void TapKeyCode(Keys key, KeyModifiers modifier);
        void ToggleKeyCode(char key, bool down, KeyModifiers modifier);
        void ToggleKeyCode(Keys key, bool down, KeyModifiers modifier);
        void TypeString(string str);
        void TypeStringDelayed(string str, int cpm);
        bool PointVisibleOnMainDisplay(Point point);
        Bitmap CopyBitmapFromDisplayInRect(Rectangle rectangle);
    }
}
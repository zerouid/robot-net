using System.Drawing;

namespace Robot.NET.Native
{
    internal class Linux : INativeImplementation
    {
        private Linux() { }
        public static INativeImplementation Instance { get; } = new Linux();

        public void ClickMouse(MouseButtons button)
        {
            throw new System.NotImplementedException();
        }

        public Bitmap CopyBitmapFromDisplayInRect(Rectangle rectangle)
        {
            throw new System.NotImplementedException();
        }

        public void DoubleClick(MouseButtons button)
        {
            throw new System.NotImplementedException();
        }

        public void DragMouse(Point point, MouseButtons button)
        {
            throw new System.NotImplementedException();
        }

        public Size GetMainDisplaySize()
        {
            throw new System.NotImplementedException();
        }

        public Point GetMousePos()
        {
            throw new System.NotImplementedException();
        }

        public void MoveMouse(Point point)
        {
            throw new System.NotImplementedException();
        }

        public bool PointVisibleOnMainDisplay(Point point)
        {
            throw new System.NotImplementedException();
        }

        public void ScrollMouse(int x, int y)
        {
            throw new System.NotImplementedException();
        }

        public bool SmoothlyMoveMouse(Point endPoint)
        {
            throw new System.NotImplementedException();
        }

        public void TapKeyCode(char key, KeyModifiers modifier)
        {
            throw new System.NotImplementedException();
        }

        public void TapKeyCode(Keys key, KeyModifiers modifier)
        {
            throw new System.NotImplementedException();
        }

        public void ToggleKeyCode(char key, bool down, KeyModifiers modifier)
        {
            throw new System.NotImplementedException();
        }

        public void ToggleKeyCode(Keys key, bool down, KeyModifiers modifier)
        {
            throw new System.NotImplementedException();
        }

        public void ToggleMouse(bool down, MouseButtons button)
        {
            throw new System.NotImplementedException();
        }

        public void TypeString(string str)
        {
            throw new System.NotImplementedException();
        }

        public void TypeStringDelayed(string str, int cpm)
        {
            throw new System.NotImplementedException();
        }
    }
}
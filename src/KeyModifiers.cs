using System;

namespace Robot.NET
{
    [Flags]
    /// <summary>Key modifiers.</summary>
    public enum KeyModifiers
    {
        None = 0,
        Alt = 1,
        Command = 2,
        Control = 4,
        Shift = 8,
    }
}
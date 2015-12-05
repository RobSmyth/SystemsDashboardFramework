using System;


namespace NoeticTools.SystemsDashboard.Framework.Input
{
    [Flags]
    public enum RelativeDropPostion
    {
        OnTop = 1,
        Top = 2,
        Left = 4,
        Right = 8,
        Bottom = 16,
        Horizontal = Top + Bottom,
        Vertical = Left + Right
    }
}
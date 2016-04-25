using System;


namespace NoeticTools.TeamStatusBoard.Framework.Input
{
    [Flags]
    public enum RelativeDropPostion
    {
        None = 0,

        OnTop = 1,
        Above = 2,
        ToLeft = 4,
        ToRight = 8,
        Below = 16,

        TopHalf = 32,
        BottomHalf = 64,
        LeftHalf = 128,
        RightHalf = 256,

        Horizontal = Above + Below,
        Vertical = ToLeft + ToRight,

        NewGroup = TopHalf + BottomHalf + LeftHalf + RightHalf
    }
}
﻿namespace Oligopoly.Squares;

internal sealed class JailSquare : Square
{
    /// <inheritdoc/>
    public override SquareType Type
    {
        get
        {
            return SquareType.Jail;
        }
    }
}

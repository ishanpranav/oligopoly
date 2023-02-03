using System;
using MessagePack;

namespace Oligopoly.EventArgs;

[MessagePackObject]
public class GameEventArgs : System.EventArgs
{
    public GameEventArgs(Game game)
    {
        ArgumentNullException.ThrowIfNull(game);

        Game = game;
    }

    [Key(0)]
    public Game Game { get; }
}

using System;
using MessagePack;

namespace Oligopoly.EventArgs;

[MessagePackObject]
public class PlayerEventArgs : System.EventArgs
{
    public PlayerEventArgs(Player player)
    {
        Player = player;
    }

    [Key(0)]
    public Player Player { get; }
}

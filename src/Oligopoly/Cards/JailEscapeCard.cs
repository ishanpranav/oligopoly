using MessagePack;

namespace Oligopoly.Cards;

[MessagePackObject]
public class JailEscapeCard : Card
{
    public JailEscapeCard(string name) : base(name) { }

    public override void Play(Player player)
    {
        player.JailTurns = 0;
    }
}

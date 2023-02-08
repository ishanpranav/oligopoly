using System.IO;
using MessagePack;

namespace Oligopoly;

public static class OligopolySerializer
{
    private static MessagePackSerializerOptions? s_options;

    private static MessagePackSerializerOptions Options
    {
        get
        {
            if (s_options is null)
            {
                s_options = MessagePackSerializerOptions.Standard
                    .WithCompression(MessagePackCompression.Lz4Block)
                    .WithSecurity(MessagePackSecurity.UntrustedData);
            }

            return s_options;
        }
    }

    public static void Write(Stream output, Game value)
    {
        MessagePackSerializer.Serialize(output, value, Options);
    }

    public static void Write(Stream output, Board value)
    {
        MessagePackSerializer.Serialize(output, value, Options);
    }

    public static Game ReadGame(Stream input)
    {
        return MessagePackSerializer.Deserialize<Game>(input, Options);
    }

    public static Board ReadBoard(Stream input)
    {
        return MessagePackSerializer.Deserialize<Board>(input, Options);
    }
}

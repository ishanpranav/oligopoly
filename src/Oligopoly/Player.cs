using System;
using System.Collections.Generic;
using MessagePack;
using Oligopoly.Agents;
using Oligopoly.Assets;

namespace Oligopoly;

[MessagePackObject]
public class Player : IAsset
{
    private const int FreeJailDuration = -1;

    private readonly HashSet<Deed> _deeds;

    private Agent? _agent;

    public Player(string name)
    {
        Name = name;
        _deeds = new HashSet<Deed>();
    }

    public Player(string name, IEnumerable<Deed> deeds)
    {
        Name = name;
        _deeds = new HashSet<Deed>(deeds);
    }

    [Key(0)]
    public string Name { get; }

    [Key(1)]
    public int Cash { get; set; }

    [IgnoreMember]
    public Agent Agent
    {
        get
        {
            if (_agent is null)
            {
                return Agent.Default;
            }

            return _agent;
        }
        set
        {
            _agent = value;
        }
    }

    [IgnoreMember]
    public bool Jailed
    {
        get
        {
            return JailDuration is not FreeJailDuration;
        }
    }

    [Key(2)]
    public int JailDuration { get; private set; } = FreeJailDuration;

    [Key(3)]
    public IReadOnlyCollection<Deed> Deeds
    {
        get
        {
            return _deeds;
        }
    }

    public void Arrest()
    {
        if (Jailed)
        {
            throw new InvalidOperationException();
        }

        JailDuration = 0;
    }

    /// <inheritdoc/>
    public int Appraise()
    {
        int result = Cash;

        foreach (Deed deed in _deeds)
        {
            result += deed.Appraise();
        }

        return result;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}

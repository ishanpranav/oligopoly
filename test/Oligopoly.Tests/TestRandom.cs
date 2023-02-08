using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Oligopoly.Tests;

internal sealed class TestRandom : Random
{
    private readonly IEnumerator<int> _enumerator;

    public TestRandom(params int[] items)
    {
        _enumerator = ((IEnumerable<int>)items).GetEnumerator();
    }

    /// <inheritdoc/>
    public override int Next()
    {
        if (_enumerator.MoveNext())
        {
            return _enumerator.Current;
        }

        return Shared.Next();
    }

    /// <inheritdoc/>
    public override int Next(int minValue, int maxValue)
    {
        if (!_enumerator.MoveNext())
        {
            return Shared.Next(minValue, maxValue);
        }

        Assert.IsTrue(_enumerator.Current >= minValue);
        Assert.IsTrue(_enumerator.Current < maxValue || minValue == maxValue);

        return _enumerator.Current;
    }

    /// <inheritdoc/>
    public override void NextBytes(byte[] buffer)
    {
        for (int i = 0; i < buffer.Length; i++)
        {
            if (!_enumerator.MoveNext())
            {
                Shared.NextBytes(buffer.AsSpan(i));

                return;
            }

            checked
            {
                buffer[i] = (byte)_enumerator.Current;
            }
        }
    }
}

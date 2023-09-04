using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake;

public class Direction
{
    public static readonly Direction Left = new Direction(0, -1); 
    public static readonly Direction Right = new Direction(0, 1); 
    public static readonly Direction Up = new Direction(-1, 0); 
    public static readonly Direction Down = new Direction(1, 0); 
    public int RowOffset { get; }
    public int ColOffset { get; }

    public Direction(int rowOffset, int colOffset)
    {
        RowOffset = rowOffset;
        ColOffset = colOffset;
    }

    public Direction Opposite()
    {
        return new Direction(-RowOffset, -ColOffset);
    }

    protected bool Equals(Direction other)
    {
        return RowOffset == other.RowOffset && ColOffset == other.ColOffset;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Direction)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(RowOffset, ColOffset);
    }

    public static bool operator ==(Direction? left, Direction? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Direction? left, Direction? right)
    {
        return !Equals(left, right);
    }
}

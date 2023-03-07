using System;

public struct GridPosition : IEquatable<GridPosition>
{
    public int x { get; private set; }
    public int z { get; private set; }

    public GridPosition(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public override string ToString() => $"({this.x}, {this.z})";
    public static bool operator ==(GridPosition a, GridPosition b) => a.x == b.x && a.z == b.z;
    public static bool operator !=(GridPosition a, GridPosition b) => a.x != b.x || a.z != b.z;
    public override int GetHashCode() => HashCode.Combine(x, z);
    public bool Equals(GridPosition other) => this == other;

    public override bool Equals(object obj)
    {
        return obj is GridPosition position &&
               x == position.x &&
               z == position.z;
    }
}

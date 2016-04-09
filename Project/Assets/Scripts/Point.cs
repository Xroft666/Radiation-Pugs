using UnityEngine;
using System.Collections;

public struct Point
{
    public int x;
    public int y;

    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override bool Equals(System.Object obj)
    {
        if (! (obj is Point)) return false;

        Point p = (Point) obj;
        return x == p.x & y == p.y;
    }

    public override int GetHashCode()
    { 
        return ShiftAndWrap(x.GetHashCode(), 2) ^ y.GetHashCode();
    } 

    public override string ToString()
    {
        return string.Format("{0}, {1}", x, y);
    }

    private int ShiftAndWrap(int value, int positions)
    {
        positions = positions & 0x1F;

        // Save the existing bit pattern, but interpret it as an unsigned integer.
        uint number = System.BitConverter.ToUInt32(System.BitConverter.GetBytes(value), 0);
        // Preserve the bits to be discarded.
        uint wrapped = number >> (32 - positions);
        // Shift and wrap the discarded bits.
        return System.BitConverter.ToInt32(System.BitConverter.GetBytes((number << positions) | wrapped), 0);
    }
}

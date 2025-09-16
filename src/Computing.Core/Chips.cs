namespace ComputingSystem.Core;

public static class Chips
{
    /// <summary>
    /// Nand implemented as a bitwise operation because this is considered a primitive.
    /// </summary>
    public static int Nand(int a, int b)
    {
        return ~(a & b);
    }

    public static int Not(int a)
    {
        return Nand(a, a);
    }

    public static int And(int a, int b)
    {
        return Not(Nand(a, b));
    }

    public static int Or(int a, int b)
    {
        return Nand(Not(a), Not(b));
    }

    public static int Xor(int a, int b)
    {
        return And(Or(a, b), Nand(a, b));
    }

    public static int Mux_MBit(int load, int a, int b)
    {
        load = load == 0 ? 0 : -1;
        return a & ~load | b & load;
    }

    public static int Mux_4way_MBit(int load0, int load1, int a, int b, int c, int d)
    {
        return Mux_MBit(load0, Mux_MBit(load1, a, b), Mux_MBit(load1, c, d));
    }

    public static int Mux_8way_MBit(int load0, int load1, int load2, int a, int b, int c, int d, int e, int f, int g, int h)
    {
        return Mux_MBit(load0, Mux_4way_MBit(load1, load2, a, b, c, d), Mux_4way_MBit(load1, load2, e, f, g, h));
    }

    public static (int, int) DMux_MBit(int load, int a)
    {
        load = load == 0 ? 0 : -1;
        return (a & ~load, a & load);
    }

    public static (int, int, int, int) DMux_4way_MBit(int load0, int load1, int a)
    {
        var (a0, a1) = DMux_MBit(load0, a);
        var (b0, b1) = DMux_MBit(load1, a1);
        return (a0, b0, b1, a1);
    }
}
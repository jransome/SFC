public class Facing
{
    public static Facing Bow { get; } = new Facing(0);
    public static Facing PortBow { get; } = new Facing(1);
    public static Facing StarboardBow { get; } = new Facing(2);
    public static Facing PortStern { get; } = new Facing(3);
    public static Facing StarboardStern { get; } = new Facing(4);
    public static Facing Stern { get; } = new Facing(5);

    public int Index { get; }

    public Facing(int index)
    {
        Index = index;
    }

    public static Facing GetFacingByHeading(float heading)
    {
        if (heading > 30)
        {
            if (heading <= 90) return StarboardBow;
            if (heading <= 150) return StarboardStern;
        }
        else
        {
            if (heading > -30) return Bow;
            if (heading > -90) return PortBow;
            if (heading > -150) return PortStern;
        }
        return Stern;
    }
}

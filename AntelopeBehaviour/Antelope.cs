using Common;
using Common.Interfaces;
using Common.ValueObjects;

namespace AntelopeBehaviour;

public class Antelope : IAnimal, IPrey
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Speed { get; init; } = 2;
    public int VisionRange { get; init; } = 5;
    public double Health { get; set; }
    public string Name => "Antelope";
    public char Symbol => 'A';
    public Antelope() { }

    public Direction GetDirectionTo(IAnimal other)
    {
        // Antelopes run away from lions
        if (other is IPredator)
        {
            return new Direction {X = X > other.X ? Speed : -Speed, Y = Y > other.Y ? Speed : -Speed};
        }
        return new Direction { X = 0, Y = 0 };
    }

    public void InteractWith(IAnimal other)
    {

    }
}

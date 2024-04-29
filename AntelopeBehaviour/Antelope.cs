using Common;
using Common.Interfaces;
using Common.ValueObjects;

namespace AntelopeBehaviour;

public class Antelope : Animal, IAnimal, IPrey
{
    private static int animalId = 1;
    private int _offspring;
    public int X { get; set; }
    public int Y { get; set; }
    public int Speed { get; init; } = 2;
    public int VisionRange { get; init; } = 5;
    public int Age { get; set; }
    public double Health { get; set; }
    public string Species => "Antelope";
    public override char Symbol => 'A';
    public override string Icon => "🦌";
    public int Offspring => _offspring;

    public Antelope()
    {
        Id = animalId++;
        _offspring = 0;
    }

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

    Direction IAnimal.GetDirectionTo(IAnimal other)
    {
        throw new NotImplementedException();
    }

    public void IncrementOffspringCount()
    {
        _offspring++;
    }
}

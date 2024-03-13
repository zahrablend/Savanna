using CodeLibrary.Interfaces;

namespace CodeLibrary.Animals;

public class Antelope : IAnimal
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Speed { get; init; } = 2;
    public int VisionRange { get; init; } = 5;
    public double Health { get; set; }

    public Antelope() { }

    public void InteractWith(IAnimal otherAnimal)
    {
        if (otherAnimal is Lion)
        {
            this.Health = 0;
        }
    }
}

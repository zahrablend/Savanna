using CodeLibrary.Interfaces;

namespace CodeLibrary.Animals;

public class Lion : IAnimal
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Speed { get; init; } = 3;
    public int VisionRange { get; init; } = 4;
    public double Health { get; set; }
    public int SameTypeNeighborCount { get; set; }
    public IAnimal? LastNeighbor { get; set; }
}


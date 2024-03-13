namespace CodeLibrary.Interfaces;

public interface IAnimal
{
    int X { get; set; }
    int Y { get; set; }
    int Speed { get; init; }
    int VisionRange { get; init; }
    double Health { get; set; }

    void InteractWith(IAnimal otherAnimal);
}

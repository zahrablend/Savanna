namespace Common.Interfaces;

public interface IAnimal
{
    int X { get; set; }
    int Y { get; set; }
    int Speed { get; init; }
    int VisionRange { get; init; }
    double Health { get; set; }
    string Name { get; }
    char Symbol { get; }
    Direction GetDirectionTo(IAnimal other);
    void InteractWith(IAnimal other);
}

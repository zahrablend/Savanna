using Common.ValueObjects;

namespace Common.Interfaces;

public interface IAnimal
{
    int Id { get; }
    int X { get; set; }
    int Y { get; set; }
    int Speed { get; init; }
    int VisionRange { get; init; }
    double Health { get; set; }
    int Offspring { get; }
    int Age { get; set; }
    string Species { get; }
    Direction GetDirectionTo(IAnimal other);
    void InteractWith(IAnimal other);
    void IncrementOffspringCount();
}

namespace CodeLibrary.Interfaces;

public interface IAnimal
{
    int X { get; set; }
    int Y { get; set; }
    int Speed { get; init; }
    int VisionRange { get; init; }
    double Health { get; set; }
    IAnimal? LastNeighbour { get; set; }
    int ConsecutiveInteractions { get; set; }

    void Move(int newX, int newY);
    void DecreaseHealth(double amount);
    void InteractWith(IAnimal otherAnimal);
}

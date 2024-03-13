using CodeLibrary.Interfaces;

namespace CodeLibrary.Animals;

public class Antelope : IAnimal
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Speed { get; init; } = 2;
    public int VisionRange { get; init; } = 5;
    public double Health { get; set; }
    public IAnimal? LastNeighbour { get; set; }
    public int ConsecutiveInteractions { get; set; }

    public void Move(int newX, int newY)
    {
        X = newX;
        Y = newY;
    }

    public void DecreaseHealth(double amount)
    {
        Health -= amount;
    }

    public void InteractWith(IAnimal otherAnimal)
    {
        if (otherAnimal is Antelope)
        {
            if (LastNeighbour == otherAnimal)
            {
                ConsecutiveInteractions++;
                Console.WriteLine($"Antelope {this} interacted with Antelope {otherAnimal}. Consecutive interactions: {ConsecutiveInteractions}");
            }
            else
            {
                LastNeighbour = otherAnimal;
                ConsecutiveInteractions = 1;
                Console.WriteLine($"Antelope {this} started interacting with Antelope {otherAnimal}");
            }
        }
    }
}

using CodeLibrary.Interfaces;

namespace CodeLibrary.Animals;

public class Lion : IAnimal
{
    public int X { get; set; }
    public int Y { get; set; } 
    public int Speed { get; init; } = 3;
    public int VisionRange { get; init; } = 4;
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
            // Increase the lion's health by 1
            Health += 1;
            // Decrease the antelope's health to 0
            otherAnimal.Health = 0;
        }
        else if (otherAnimal is Lion && LastNeighbour == otherAnimal)
        {
            if (LastNeighbour == otherAnimal)
            {
                ConsecutiveInteractions++;
            }
            else if (otherAnimal is Lion)
            {
                LastNeighbour = otherAnimal;
                ConsecutiveInteractions = 1;
            }
        }
    }
}


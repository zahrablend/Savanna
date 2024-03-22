using Common.Interfaces;

namespace LionBehaviour;

public class Lion : IAnimal, IPredator
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Speed { get; init; } = 3;
    public int VisionRange { get; init; } = 4;
    public double Health { get; set; }
    public string Name => "Lion";
    public char Symbol => 'L';
    public Lion() { }

    public (int directionX, int directionY) GetDirectionTo(IAnimal other)
    {
        // Lions chase antelopes
        if (other is IPrey)
        {
            return (X < other.X ? Speed : -Speed, Y < other.Y ? Speed : -Speed);
        }
        return (0, 0);
    }

    /// <summary>
    /// Specifies interaction between Lion and Antelope
    /// If the specified animal is a Lion and encounters an Antelope, 
    /// the Lion's health increases by 1 and the Antelope's health is set to 0.
    /// </summary>
    /// <param name="other">Other type of animal</param>
    public void InteractWith(IAnimal other)
    {
        // Lions increase their health by 1 when they encounter an antelope
        if (other is IPrey)
        {
            Health += 1;
            other.Health = 0;
        }
    }
}

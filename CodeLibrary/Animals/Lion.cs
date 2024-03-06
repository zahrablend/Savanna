using CodeLibrary.Interfaces;

namespace CodeLibrary.Animals;

public class Lion : IAnimal
{
    public int X { get; set; }
    public int Y { get; set; }

    public void MoveAnimal(IAnimal[,] gameField, int fieldHeight, int fieldWidth)
    {
        int visionRange = 5;
        int moveDistance = 3;

        // Check for antelopes within vision range
        for (int i = Math.Max(0, X - visionRange); i <= Math.Min(fieldWidth - 1, X + visionRange); i++)
        {
            for (int j = Math.Max(0, Y - visionRange); j <= Math.Min(fieldHeight - 1, Y + visionRange); j++)
            {
                if (gameField[i, j] is Antelope)
                {
                    // Move towards the antelope
                    X = Math.Max(0, X - moveDistance);
                    Y = Math.Max(0, Y - moveDistance);
                    return;
                }
            }
        }
    }
}

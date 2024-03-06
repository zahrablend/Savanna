using CodeLibrary.Interfaces;

namespace CodeLibrary.Animals;

public class Antelope : IAnimal
{
    public int X { get; set; }
    public int Y { get; set; }

    public void MoveAnimal(IAnimal[,] gameField, int fieldHeight, int fieldWidth)
    {
        int visionRange = 4;
        int moveDistance = 2;

        // Check for lions within vision range
        for (int i = Math.Max(0, X - visionRange); i <= Math.Min(fieldWidth - 1, X + visionRange); i++)
        {
            for (int j = Math.Max(0, Y - visionRange); j <= Math.Min(fieldHeight - 1, Y + visionRange); j++)
            {
                if (gameField[i, j] is Lion)
                {
                    // Move away from the lion
                    X = Math.Min(fieldWidth - 1, X + moveDistance);
                    Y = Math.Min(fieldHeight - 1, Y + moveDistance);
                    return;
                }
            }
        }
    }
}

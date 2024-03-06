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
        Random random = new Random();

        // Check for antelopes within vision range
        for (int i = Math.Max(0, X - visionRange); i <= Math.Min(fieldWidth - 1, X + visionRange); i++)
        {
            for (int j = Math.Max(0, Y - visionRange); j <= Math.Min(fieldHeight - 1, Y + visionRange); j++)
            {
                if (gameField[i, j] is Antelope)
                {
                    // Move towards the antelope
                    gameField[X, Y] = null; // Remove the lion from its current position
                    int newX = i > X ? Math.Min(fieldWidth - 1, X + moveDistance) : Math.Max(0, X - moveDistance);
                    int newY = j > Y ? Math.Min(fieldHeight - 1, Y + moveDistance) : Math.Max(0, Y - moveDistance);
                    gameField[newX, newY] = this; // Place the lion at its new position
                    X = newX;
                    Y = newY;
                    return;
                }
            }
        }

        // If no antelope is detected, move in a random direction
        gameField[X, Y] = null; // Remove the lion from its current position
        int randomX = random.Next(2) == 0 ? Math.Max(0, X - moveDistance) : Math.Min(fieldWidth - 1, X + moveDistance);
        int randomY = random.Next(2) == 0 ? Math.Max(0, Y - moveDistance) : Math.Min(fieldHeight - 1, Y + moveDistance);
        gameField[randomX, randomY] = this; // Place the lion at its new position
        X = randomX;
        Y = randomY;
    }
}

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
        for (int i = Math.Max(0, X - visionRange); i <= Math.Min(X + visionRange, fieldWidth - 1); i++)
        {
            for (int j = Math.Max(0, Y - visionRange); j <= Math.Min(Y + visionRange, fieldHeight - 1); j++)
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
        Console.WriteLine("No antelope detected. Lion is moving in a random direction.");
        gameField[X, Y] = null; // Remove the lion from its current position

        // Generate a random direction: 0 = up, 1 = down, 2 = left, 3 = right
        int direction = random.Next(4);

        // Calculate new position based on the random direction
        int randomX = X, randomY = Y;
        switch (direction)
        {
            case 0: // up
                randomY = Math.Max(0, Y - moveDistance);
                break;
            case 1: // down
                randomY = Math.Min(fieldHeight - 1, Y + moveDistance);
                break;
            case 2: // left
                randomX = Math.Max(0, X - moveDistance);
                break;
            case 3: // right
                randomX = Math.Min(fieldWidth - 1, X + moveDistance);
                break;
        }

        // Move the lion to the new position
        if (randomX >= 0 && randomX < fieldWidth && randomY >= 0 && randomY < fieldHeight)
        {
            gameField[randomX, randomY] = this; // Place the lion at its new position
            X = randomX;
            Y = randomY;
        }
        Console.WriteLine($"Lion moved to ({X}, {Y}).");
    }
}

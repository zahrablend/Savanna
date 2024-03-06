﻿using CodeLibrary.Interfaces;

namespace CodeLibrary.Animals;

public class Antelope : IAnimal
{
    public int X { get; set; }
    public int Y { get; set; }

    public void MoveAnimal(IAnimal[,] gameField, int fieldHeight, int fieldWidth)
    {
        int visionRange = 4;
        int moveDistance = 2;
        Random random = new Random();

        // Check for lions within vision range
        for (int i = Math.Max(0, X - visionRange); i <= Math.Min(X + visionRange, fieldWidth - 1); i++)
        {
            for (int j = Math.Max(0, Y - visionRange); j <= Math.Min(Y + visionRange, fieldHeight - 1); j++)
            {
                if (gameField[i, j] is Lion)
                {
                    Console.WriteLine($"Lion detected at ({i}, {j}). Antelope at ({X}, {Y}) is moving away.");

                    // Move away from the lion
                    gameField[X, Y] = null; // Remove the antelope from its current position
                    int newX = i < X ? Math.Min(fieldWidth - 1, X + moveDistance) : Math.Max(0, X - moveDistance);
                    int newY = j < Y ? Math.Min(fieldHeight - 1, Y + moveDistance) : Math.Max(0, Y - moveDistance);
                    if (newX >= 0 && newX < fieldWidth && newY >= 0 && newY < fieldHeight)
                    {
                        gameField[newX, newY] = this; // Place the antelope at its new position
                        X = newX;
                        Y = newY;
                    }
                    Console.WriteLine($"Antelope moved to ({X}, {Y}).");
                    return;
                }
            }
        }

        // If no lion is detected, move in a random direction
        Console.WriteLine("No lion detected. Antelope is moving in a random direction.");
        gameField[X, Y] = null; // Remove the antelope from its current position

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

        // Move the antelope to the new position
        if (randomX >= 0 && randomX < fieldWidth && randomY >= 0 && randomY < fieldHeight)
        {
            gameField[randomX, randomY] = this; // Place the antelope at its new position
            X = randomX;
            Y = randomY;
        }
        Console.WriteLine($"Antelope moved to ({X}, {Y}).");
    }
}

using CodeLibrary.Animals;
using CodeLibrary.Interfaces;
using System.Text;

namespace CodeLibrary;

public class FieldDisplayer
{
    /// <summary>
    /// Represents the size of the field.
    /// </summary>
    public struct FieldSize
    {
        public int Height { get; }
        public int Width { get; }

        /// <summary>
        /// Initializes a new instance of the FieldSize structure to the specified height and width.
        /// </summary>
        /// <param name="height">The height of the field.</param>
        /// <param name="width">The width of the field.</param>
        public FieldSize(int height, int width)
        {
            Height = height;
            Width = width;
        }
    }

    //Ignore the warning in this context
    /// <summary>
    /// Draws the game field as a string based on the current positions of the animals.
    /// </summary>
    /// <param name="gameField">The 2D array representing the game field.</param>
    /// <param name="fieldHeight">The height of the game field.</param>
    /// <param name="fieldWidth">The width of the game field.</param>
    /// <returns>A string representation of the game field.</returns>
    public string DrawField(IAnimal[,] gameField, int fieldHeight, int fieldWidth)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < fieldHeight; i++)
        {
            for (int j = 0; j < fieldWidth; j++)
            {
                var animal = gameField[i, j];
                if (animal is Antelope)
                {
                    sb.Append('A');
                }
                else if (animal is Lion)
                {
                    sb.Append('L');
                }
                else
                {
                    sb.Append('.');
                }
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }
}

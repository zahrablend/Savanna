using CodeLibrary.Animals;
using CodeLibrary.Interfaces;
using System.Text;

namespace CodeLibrary;

public class FieldDisplayer
{
    public struct FieldSize
    {
        public int Height { get; }
        public int Width { get; }

        public FieldSize(int height, int width)
        {
            Height = height;
            Width = width;
        }
    }
    //Ignore the warning in this context
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

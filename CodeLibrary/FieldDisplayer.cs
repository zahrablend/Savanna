using CodeLibrary.Animals;
using CodeLibrary.Interfaces;

namespace CodeLibrary;

public class FieldDisplayer
{
    public struct FieldSize
    {
        public int Width { get; }
        public int Height { get; }

        public FieldSize(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
    //Ignore the warning in this context
    public void DrawField(IAnimal[,] gameField, int fieldHeight, int fieldWidth)
    {
        for (int i = 0; i < fieldHeight; i++)
        {
            for (int j = 0; j < fieldWidth; j++)
            {
                var animal = gameField[i, j];
                if (animal is Antelope)
                {
                    Console.WriteLine("A");
                }
                else if (animal is Lion)
                {
                    Console.Write("L");
                }
                else
                {
                    Console.Write(".");
                }
            }
            Console.WriteLine();
        }
    }
}

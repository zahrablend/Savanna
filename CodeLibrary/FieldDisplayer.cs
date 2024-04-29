using CodeLibrary.GameEngine;
using Common.Interfaces;
using System.Text;
using static CodeLibrary.GameEngine.GameSetup;

namespace CodeLibrary;

public class FieldDisplayer
{
    public FieldSize Size { get; set; }
    private readonly GameSetup _gameSetup;

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

    public FieldDisplayer(GameSetup gameSetup)
    {
        _gameSetup = gameSetup;
    }

    public string DrawField(IGameField gameField, int fieldHeight, int fieldWidth, DisplayType displayType)
    {
        var sb = new StringBuilder();
        for (int i = 0; i < fieldHeight; i++)
        {
            for (int j = 0; j < fieldWidth; j++)
            {
                var cell = gameField.GetState(j, i);
                if (cell.State is IAnimal animal)
                {
                    sb.Append(_gameSetup.DisplayAnimalRepresentation(animal, displayType));
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

using Common.Interfaces;
using Common.ValueObjects;

namespace CodeLibrary;

public class GameField : IGameField
{
    private FieldCell[,] _field;

    public int Width { get; private set; }
    public int Height { get; private set; }

    public GameField(int width, int height)
    {
        Width = width;
        Height = height;
        _field = new FieldCell[height, width];
    }

    public void SetState(int x, int y, FieldCell state)
    {
        _field[y, x] = state;
    }

    public FieldCell GetState(int x, int y)
    {
        return _field[y, x];
    }

    public void Initialize(FieldCell initialState)
    {
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                _field[i, j] = initialState;
            }
        }
    }
}

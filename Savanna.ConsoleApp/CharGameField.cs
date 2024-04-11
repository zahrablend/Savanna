using Common.Interfaces;

namespace Savanna.ConsoleApp;

public class CharGameField : IGameField
{
    private char[,] _field;

    public CharGameField(int width, int height)
    {
        _field = new char[height, width];
    }

    public void SetState(int x, int y, object state)
    {
        if (state is IAnimal animal)
        {
            _field[y, x] = animal.Symbol;
        }
        else if (state is char c)
        {
            _field[y, x] = c;
        }
    }


    public object GetState(int x, int y)
    {
        return _field[y, x];
    }

    public void Initialize(object initialState)
    {
        if (initialState is char initialChar)
        {
            for (int i = 0; i < _field.GetLength(0); i++)
            {
                for (int j = 0; j < _field.GetLength(1); j++)
                {
                    _field[i, j] = initialChar;
                }
            }
        }
    }
}

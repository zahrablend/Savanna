using Common.Interfaces;

namespace Savanna.ConsoleApp;

public class CharGameField : IGameField
{
    private char[,] _field;
    private List<(int, int)> _freeLocations;

    public CharGameField(int width, int height)
    {
        _field = new char[height, width];
        _freeLocations = new List<(int, int)>();
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                _field[i, j] = '.';
                _freeLocations.Add((i, j));
            }
        }
    }

    public List<(int, int)> FreeLocations => _freeLocations;

    public void SetState(int x, int y, object state)
    {
        if (_freeLocations.Contains((y, x)))
        {
            if (state is IAnimal animal)
            {
                _field[y, x] = animal.Symbol;
                _freeLocations.Remove((y, x));
            }
            else if (state is char c)
            {
                _field[y, x] = c; // Swap x and y here as well
                _freeLocations.Remove((y, x));
            }
        }
        else
        {
            throw new ArgumentOutOfRangeException("x or y is out of the field bounds.");
        }
    }


    public object GetState(int x, int y)
    {
        if (x >= 0 && x < _field.GetLength(1) && y >= 0 && y < _field.GetLength(0))
        {
            return _field[y, x];
        }
        else
        {
            throw new ArgumentOutOfRangeException("x or y is out of the field bounds.");
        }
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

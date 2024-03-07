using CodeLibrary.Animals;
using CodeLibrary.Interfaces;

namespace CodeLibrary;

public class GameEngine
{
    private IAnimal[,] _gameField;
    private List<IAnimal> _animals;
    private FieldDisplayer.FieldSize _fieldSize;
    private FieldDisplayer _fieldDisplayer;

    public GameEngine(FieldDisplayer.FieldSize fieldSize, FieldDisplayer fieldDisplayer)
    {
        _fieldSize = fieldSize;
        _gameField = new IAnimal[_fieldSize.Height, _fieldSize.Width];
        _animals = new List<IAnimal>();
        _fieldDisplayer = fieldDisplayer;
    }

    public void AddAnimal(IAnimal animal)
    {
        int x;
        int y;

        do
        {
            x = new Random().Next(_fieldSize.Height);
            y = new Random().Next(_fieldSize.Width);
        }
        while(_gameField[x, y] != null);

        animal.X = x;
        animal.Y = y;
        _gameField[x, y] = animal;
        _animals.Add(animal);
    }

    public string DrawField()
    {
        return _fieldDisplayer.DrawField(_gameField, _fieldSize.Height, _fieldSize.Width);
    }

    public List<IAnimal> GetAnimals()
    {
        return _animals;
    }

    public IAnimal[,] GetGameField()
    {
        return _gameField;
    }

    public void UpdateGameField(IAnimal[,] gameField)
    {
        _gameField = gameField;
    }

    public void DisplayNewPosition(FieldDisplayer fieldDisplayer)
    {
        foreach (var animal in _animals)
        {
            int oldX = animal.X;
            int oldY = animal.Y;
            animal.MoveAnimal(_gameField, _fieldSize.Height, _fieldSize.Width, _fieldDisplayer.DisplayNewPosition);
            if (animal.X != oldX || animal.Y != oldY)
            {
                _gameField[oldX, oldY] = null;
                _gameField[animal.X, animal.Y] = animal;
            }
        }
    }
}

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

    private static Random random = new Random();
    public void MoveAnimal(IAnimal animal)
    {
        // Calculate the new position
        int dx = random.Next(-animal.Speed, animal.Speed + 1);
        int dy = random.Next(-animal.Speed, animal.Speed + 1);
        int newX = (animal.X + dx) % _fieldSize.Height;
        int newY = (animal.Y + dy) % _fieldSize.Width;

        // Ensure the new position is within the game field boundaries
        if (newX < 0) newX = 0;
        if (newX >= _fieldSize.Height) newX = _fieldSize.Height - 1;
        if (newY < 0) newY = 0;
        if (newY >= _fieldSize.Width) newY = _fieldSize.Width - 1;

        // Check if the new position is free
        if (_gameField[newX, newY] == null)
        {
            // Move the animal to the new position
            _gameField[animal.X, animal.Y] = null;
            animal.X = newX;
            animal.Y = newY;
            _gameField[animal.X, animal.Y] = animal;
        }
    }

    public string DrawField()
    {
        return _fieldDisplayer.DrawField(_gameField, _fieldSize.Height, _fieldSize.Width);
    }

    public IAnimal[,] GetGameField()
    {
        return _gameField;
    }

    public List<IAnimal> GetAnimals()
    {
        return _animals;
    }

    public void UpdateGameField(IAnimal[,] gameField)
    {
        _gameField = gameField;
    }
}

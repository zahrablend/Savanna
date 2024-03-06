using CodeLibrary.Interfaces;

namespace CodeLibrary;

public class GameEngine
{
    private IAnimal[,] _gameField;
    private List<IAnimal> _animals;
    private FieldDisplayer.FieldSize _fieldSize;

    public GameEngine(FieldDisplayer.FieldSize fieldSize)
    {
        _fieldSize = fieldSize;
        _gameField = new IAnimal[_fieldSize.Height, _fieldSize.Width];
        _animals = new List<IAnimal>();
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

    public string DrawField(FieldDisplayer fieldDisplayer)
    {
        return fieldDisplayer.DrawField(_gameField, _fieldSize.Height, _fieldSize.Width);
    }

    public List<IAnimal> GetAnimals()
    {
        return _animals;
    }

    public IAnimal[,] GetGameField()
    {
        return _gameField;
    }
}

using Common.Constants;
using Common.Interfaces;

namespace CodeLibrary.GameEngine;

public class GameSetup
{
    private readonly IGameField _gameField;
    private readonly FieldDisplayer _fieldDisplayer;
    private List<IAnimal> _animals;

    public List<IAnimal> GetAnimals => _animals;

    public GameSetup(IGameFieldFactory gameFieldFactory, FieldDisplayer fieldDisplayer)
    {
        _fieldDisplayer = fieldDisplayer;
        _animals = new List<IAnimal>();

        _gameField = gameFieldFactory.Create(_fieldDisplayer.Size.Height, _fieldDisplayer.Size.Width);
    }

    public void AddAnimal(IAnimal animal)
    {
        if (_gameField.FreeLocations.Count > 0)
        {
            var random = new Random();
            var index = random.Next(_gameField.FreeLocations.Count);
            var (y, x) = _gameField.FreeLocations[index];

            animal.X = x;
            animal.Y = y;
            animal.Health = Constant.InitialHealth;

            _gameField.SetState(x, y, animal);
            _animals.Add(animal);
        }
        else
        {
            throw new InvalidOperationException("No free locations available on the game field.");
        }
    }

    public string GetGameField()
    {
        return _fieldDisplayer.DrawField(_gameField, _fieldDisplayer.Size.Height, _fieldDisplayer.Size.Width);
    }
}

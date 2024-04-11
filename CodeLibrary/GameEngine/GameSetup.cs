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
        int x;
        int y;

        do
        {
            x = new Random().Next(_fieldDisplayer.Size.Height);
            y = new Random().Next(_fieldDisplayer.Size.Width);
        }
        while (_gameField.GetState(x, y) != null);

        animal.X = x;
        animal.Y = y;
        animal.Health = Constant.InitialHealth;

        _gameField.SetState(x, y, animal);
        _animals.Add(animal);
    }

    public string GetGameField()
    {
        return _fieldDisplayer.DrawField(_gameField, _fieldDisplayer.Size.Height, _fieldDisplayer.Size.Width);
    }
}

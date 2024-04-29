using Common.Constants;
using Common.Interfaces;

namespace CodeLibrary.GameEngine;

public class HealthMetricCounter
{
    private IGameField _gameField;
    private readonly FieldDisplayer _fieldDisplayer;

    public HealthMetricCounter(IGameField gameField, FieldDisplayer fieldDisplayer)
    {
        _gameField = gameField;
        _fieldDisplayer = fieldDisplayer;
    }

    public void DecreaseHealth(IAnimal animal)
    {
        animal.Health -= Constant.HealthDecreasePerMove;
    }

    public void InteractWith(IAnimal animal)
    {
        for (int i = Math.Max(0, animal.X - 1); i <= Math.Min(_fieldDisplayer.Size.Height - 1, animal.X + 1); i++)
        {
            for (int j = Math.Max(0, animal.Y - 1); j <= Math.Min(_fieldDisplayer.Size.Width - 1, animal.Y + 1); j++)
            {
                var otherAnimal = _gameField.GetState(i, j) as IAnimal;
                if (otherAnimal != null && otherAnimal != animal)
                {
                    animal.InteractWith(otherAnimal);
                }
            }
        }
    }
}

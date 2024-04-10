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

    /// <summary>
    /// Decreases the health of the specified animal by a constant value.
    /// </summary>
    /// <param name="animal"></param>
    public void DecreaseHealth(IAnimal animal)
    {
        animal.Health -= Constant.HealthDecreasePerMove;
    }

    /// <summary>
    /// Performs interactions between the specified animal and other animals in its vicinity.
    /// If the specified animal is a Lion and encounters an Antelope, the Lion's health increases by 1 and the Antelope's health is set to 0.
    /// </summary>
    /// <param name="animal">The animal that is interacting with others.</param>
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

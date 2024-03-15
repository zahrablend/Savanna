using CodeLibrary.Animals;
using CodeLibrary.Constants;
using CodeLibrary.Interfaces;

namespace CodeLibrary.GameEngine;

public class HealthMetricCounter
{
    private IAnimal?[,] _gameField;
    private readonly FieldDisplayer.FieldSize _fieldSize;

    public HealthMetricCounter(IAnimal?[,] gameField, FieldDisplayer.FieldSize fieldSize)
    {
        _gameField = gameField;
        _fieldSize = fieldSize;
    }

    public void DecreaseHealth(IAnimal animal)
    {
        animal.Health -= Constant.HealthDecreasePerMove;
    }

    public void InteractWith(IAnimal animal)
    {
        for (int i = Math.Max(0, animal.X - 1); i <= Math.Min(_fieldSize.Height - 1, animal.X + 1); i++)
        {
            for (int j = Math.Max(0, animal.Y - 1); j <= Math.Min(_fieldSize.Width - 1, animal.Y + 1); j++)
            {
                var otherAnimal = _gameField[i, j];
                if (otherAnimal != null)
                {
                    if (animal is Lion && otherAnimal is Antelope)
                    {
                        animal.Health += 1;
                        otherAnimal.Health = 0;
                    }
                }
            }
        }
    }
}

using Common.Interfaces;
using Common.ValueObjects;

namespace CodeLibrary.GameEngine;

public class AnimalMover
{
    private IGameField _gameField;
    private readonly FieldDisplayer _fieldDisplayer;
    private static readonly Random random = new();

    public AnimalMover(IGameField gameField, FieldDisplayer fieldDisplayer)
    {
        _gameField = gameField;
        _fieldDisplayer = fieldDisplayer;
    }


    public void MoveAnimal(IAnimal animal)
    {
        Direction direction = GetMovementDirection(animal);
        Direction newPosition = GetNewPosition(animal, direction);

        if (_gameField.GetState(newPosition.X, newPosition.Y) == null)
        {
            _gameField.SetState(animal.X, animal.Y, new FieldCell { State = null });
            animal.X = newPosition.X;
            animal.Y = newPosition.Y;
            _gameField.SetState(animal.X, animal.Y, new FieldCell { State = animal });
        }
    }

    private Direction GetMovementDirection(IAnimal animal)
    {
        Direction direction = new()
        {
            X = random.Next(-animal.Speed, animal.Speed + 1),
            Y = random.Next(-animal.Speed, animal.Speed + 1)
        };

        for (int i = Math.Max(0, animal.X - animal.VisionRange); i <= Math.Min(_fieldDisplayer.Size.Height - 1, animal.X + animal.VisionRange); i++)
        {
            for (int j = Math.Max(0, animal.Y - animal.VisionRange); j <= Math.Min(_fieldDisplayer.Size.Width - 1, animal.Y + animal.VisionRange); j++)
            {
                var otherAnimal = _gameField.GetState(i, j) as IAnimal;

                if (otherAnimal != null && otherAnimal.GetType() != animal.GetType())
                {
                    var otherDirection = animal.GetDirectionTo(otherAnimal);
                    direction.X = otherDirection.X;
                    direction.Y = otherDirection.Y;
                }
            }
        }
        return direction;
    }

    private Direction GetNewPosition(IAnimal animal, Direction direction)
    {
        Direction newPosition = new()
        {
            X = (animal.X + direction.X),
            Y = (animal.Y + direction.Y)
        };

        if (newPosition.X < 0) newPosition.X = 0;
        if (newPosition.X >= _fieldDisplayer.Size.Height) newPosition.X = _fieldDisplayer.Size.Height - 1;
        if (newPosition.Y < 0) newPosition.Y = 0;
        if (newPosition.Y >= _fieldDisplayer.Size.Width) newPosition.Y = _fieldDisplayer.Size.Width - 1;

        return newPosition;
    }
}

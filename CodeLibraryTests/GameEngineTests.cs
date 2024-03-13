using CodeLibrary;
using CodeLibrary.Animals;
using CodeLibrary.Interfaces;

namespace CodeLibraryTests;

public class GameEngineTests
{
    private GameEngine _gameEngine;
    private Antelope _antelope;
    private Lion _lion;

    public GameEngineTests()
    {
        var fieldSize = new FieldDisplayer.FieldSize(10, 10);
        var fieldDisplayer = new FieldDisplayer();
        _gameEngine = new GameEngine(fieldSize, fieldDisplayer);

        _antelope = new Antelope();
        _lion = new Lion();
    }

    [Fact]
    public void MoveAnimal_AntelopeMoved_AntelopeMovedCorrectDistance()
    {
        // Arrange
        _gameEngine.AddAnimal(_antelope);
        var oldX = _antelope.X;
        var oldY = _antelope.Y;

        // Act
        _gameEngine.MoveAnimal(_antelope);

        // Assert
        var distanceMoved = Math.Sqrt(Math.Pow(_antelope.X - oldX, 2) + Math.Pow(_antelope.Y - oldY, 2));
        Assert.True(distanceMoved <= _antelope.Speed);
    }

    [Fact]
    public void MoveAnimal_LionMoved_LionMovedCorrectDistance()
    {
        // Arrange
        _gameEngine.AddAnimal(_lion);
        var oldX = _lion.X;
        var oldY = _lion.Y;

        // Act
        _gameEngine.MoveAnimal(_lion);

        // Assert
        var distanceMoved = Math.Sqrt(Math.Pow(_lion.X - oldX, 2) + Math.Pow(_lion.Y - oldY, 2));
        Assert.True(distanceMoved <= _lion.Speed);
    }

    [Fact]
    public void MoveAnimal_AntelopeInteractedWithLion_AntelopeMovedAwayFromLion()
    {
        // Arrange
        _antelope.X = 5;
        _antelope.Y = 5;
        _lion.X = 5;
        _lion.Y = 5 + _antelope.VisionRange - 1; // Just within the antelope's vision range
        _gameEngine.AddAnimal(_antelope);
        _gameEngine.AddAnimal(_lion);

        var oldDistance = Math.Sqrt(Math.Pow(_antelope.X - _lion.X, 2) + Math.Pow(_antelope.Y - _lion.Y, 2));

        // Act
        _gameEngine.MoveAnimal(_antelope);

        var newDistance = Math.Sqrt(Math.Pow(_antelope.X - _lion.X, 2) + Math.Pow(_antelope.Y - _lion.Y, 2));

        // Assert
        Assert.True(newDistance > oldDistance);
    }

    [Fact]
    public void MoveAnimal_LionInteractedWithAntelope_LionMovedTowardsAntelope()
    {
        // Arrange
        _antelope.X = 5;
        _antelope.Y = 5;
        _lion.X = 5;
        _lion.Y = 5 + _lion.VisionRange - 1; // Just within the lion's vision range
        _gameEngine.AddAnimal(_antelope);
        _gameEngine.AddAnimal(_lion);

        var oldDistance = Math.Sqrt(Math.Pow(_antelope.X - _lion.X, 2) + Math.Pow(_antelope.Y - _lion.Y, 2));

        // Act
        _gameEngine.MoveAnimal(_lion);

        var newDistance = Math.Sqrt(Math.Pow(_antelope.X - _lion.X, 2) + Math.Pow(_antelope.Y - _lion.Y, 2));

        // Assert
        Assert.True(newDistance < oldDistance);
    }

    [Theory]
    [InlineData(typeof(Antelope))]
    [InlineData(typeof(Lion))]
    public void MoveAnimal_AnimalMoved_AnimalDoesNotMoveOutsideGameField(Type animalType)
    {
        // Arrange
        var animal = Activator.CreateInstance(animalType) as IAnimal;
        animal.X = 0;
        animal.Y = 0;

        if (animal is Antelope antelope)
        {
            antelope = new Antelope { Speed = 100 };
            _gameEngine.AddAnimal(antelope);
            animal = antelope;
        }
        else if (animal is Lion lion)
        {
            lion = new Lion { Speed = 100 };
            _gameEngine.AddAnimal(lion);
            animal = lion;
        }

        // Act
        _gameEngine.MoveAnimal(animal);

        // Assert
        Assert.True(animal.X >= 0 && animal.X < _gameEngine.GetGameField().GetLength(0));
        Assert.True(animal.Y >= 0 && animal.Y < _gameEngine.GetGameField().GetLength(1));
    }

    [Theory]
    [InlineData(typeof(Antelope))]
    [InlineData(typeof(Lion))]
    public void CheckAnimalStatus_AnimalHealthZero_AnimalRemovedFromGame(Type animalType)
    {
        // Arrange
        var animal = Activator.CreateInstance(animalType) as IAnimal;
        animal.Health = 0;
        _gameEngine.AddAnimal(animal);

        // Act
        _gameEngine.MoveAnimal(animal);

        // Assert
       // Assert.DoesNotContain(animal, _gameEngine.GetAnimals());
        Assert.DoesNotContain(animal, _gameEngine.GetGameField().OfType<IAnimal>());
    }

    [Theory]
    [InlineData(typeof(Antelope))]
    [InlineData(typeof(Lion))]
    public void CheckAnimalStatus_TwoAnimalsOfSameTypeAreNeighboursForThreeConsecutiveIterations_NewAnimalOfSameTypeAddedToGame(Type animalType)
    {
        // Arrange
        var animal = Activator.CreateInstance(animalType) as IAnimal;
        animal.ConsecutiveInteractions = 3;
        _gameEngine.AddAnimal(animal);

        // Act
        _gameEngine.MoveAnimal(animal);

        // Assert
        Assert.Equal(2, _gameEngine.GetAnimals().Count(a => a.GetType() == animalType));
    }

}

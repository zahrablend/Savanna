using CodeLibrary;
using CodeLibrary.Animals;
using CodeLibrary.Constants;
using CodeLibrary.Interfaces;
using Moq;

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
    public void AddAnimal_AnimalHealthSetToInitialHealth_WhenAnimalIsAdded()
    {
        // Arrange
        var mockAnimal = new Mock<IAnimal>();
        var gameEngine = new GameEngine(new FieldDisplayer.FieldSize(10, 10), new FieldDisplayer()); // Assuming FieldDisplayer is the class for displaying the field

        // Act
        gameEngine.AddAnimal(mockAnimal.Object);

        // Assert
        mockAnimal.VerifySet(a => a.Health = Constant.InitialHealth);
    }

    [Theory]
    [InlineData(typeof(Antelope))]
    [InlineData(typeof(Lion))]
    public void MoveAnimal_AnimalMoved_AnimalMovedWithCorrectSpeed(Type animalType)
    {
        // Arrange
        IAnimal animal;
        if (animalType == typeof(Antelope))
        {
            animal = new Antelope();
        }
        else if (animalType == typeof(Lion))
        {
            animal = new Lion();
        }
        else
        {
            throw new ArgumentException("Invalid animal type");
        }

        _gameEngine.AddAnimal(animal);
        var oldX = animal.X;
        var oldY = animal.Y;

        // Act
        _gameEngine.MoveAnimal(animal);

        // Assert
         
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
        if (animal is Antelope)
        {
            Antelope antelope = new Antelope { Speed = 100 };
            _gameEngine.AddAnimal(antelope);
            animal = antelope;
        }
        else if (animal is Lion)
        {
            Lion lion = new Lion { Speed = 100 };
            _gameEngine.AddAnimal(lion);
            animal = lion;
        }

        // Act
        _gameEngine.MoveAnimal(animal);

        // Assert
        Assert.True(animal.X >= 0 && animal.X < _gameEngine.GetGameField.GetLength(0));
        Assert.True(animal.Y >= 0 && animal.Y < _gameEngine.GetGameField.GetLength(1));
    }

    [Theory]
    [InlineData(typeof(Antelope))]
    [InlineData(typeof(Lion))]
    public void MoveAnimal_AnimalMoved_AnimalHealthDecreased(Type animalType)
    {
        // Arrange
        IAnimal animal = Activator.CreateInstance(animalType) as IAnimal;
        _gameEngine.AddAnimal(animal);

        var initialHealth = animal.Health;

        // Act
        _gameEngine.MoveAnimal(animal);

        var finalHealth = animal.Health;

        // Assert
        Assert.True(finalHealth == initialHealth - Constant.HealthDecreasePerMove);
    }

    [Theory]
    [InlineData(typeof(Antelope))]
    [InlineData(typeof(Lion))]
    public void MoveAnimal_AnimalHealthZero_DeadAnimalRemovedFromGame(Type animalType)
    {
        // Arrange
        IAnimal animal = Activator.CreateInstance(animalType) as IAnimal;
        animal.Health = 0; // Set health to 0 to simulate death
        _gameEngine.AddAnimal(animal);

        var initialAnimalCount = _gameEngine.GetAnimals.Count;

        // Act
        _gameEngine.MoveAnimal(animal);

        var finalAnimalCount = _gameEngine.GetAnimals.Count;

        // Assert
        Assert.True(finalAnimalCount == initialAnimalCount - 1);
    }




    //[Fact]
    //public void InteractWith_LionInteractsWithAntelope_LionHealthIncreasedByOne()
    //{
    //    // Arrange
    //    Lion lion = new Lion();
    //    Antelope antelope = new Antelope();

    //    var initialHealth = lion.Health;

    //    // Act
    //    lion.InteractWith(antelope);

    //    var finalHealth = lion.Health;

    //    // Assert
    //    Assert.True(finalHealth == initialHealth + 1);
    //}

    //[Fact]
    //public void InteractWith_AntelopeInteractsWithLion_AntelopeHealthZero()
    //{
    //    // Arrange
    //    Antelope antelope = new Antelope();
    //    Lion lion = new Lion();

    //    // Act
    //    antelope.InteractWith(lion);

    //    var finalHealth = antelope.Health;

    //    // Assert
    //    Assert.True(finalHealth == 0);
    //}
}

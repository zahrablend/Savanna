//using CodeLibrary;
//using CodeLibrary.Animals;
//using CodeLibrary.Constants;
//using CodeLibrary.Interfaces;
//using Moq;

//namespace CodeLibraryTests;

//public class GameEngineTests
//{
//    private GameEngineX _gameEngine;
//    private Antelope _antelope;
//    private Lion _lion;

//    public GameEngineTests()
//    {
//        var fieldSize = new FieldDisplayer.FieldSize(10, 10);
//        var fieldDisplayer = new FieldDisplayer();
//        _gameEngine = new GameEngineX(fieldSize, fieldDisplayer);

//        _antelope = new Antelope();
//        _lion = new Lion();
//    }

//    [Fact]
//    public void AddAnimal_AnimalHealthSetToInitialHealth_WhenAnimalIsAdded()
//    {
//        // Arrange
//        var mockAnimal = new Mock<IAnimal>();
//        var gameEngine = new GameEngineX(new FieldDisplayer.FieldSize(10, 10), new FieldDisplayer()); // Assuming FieldDisplayer is the class for displaying the field

//        // Act
//        gameEngine.AddAnimal(mockAnimal.Object);

//        // Assert
//        mockAnimal.VerifySet(a => a.Health = Constant.InitialHealth);
//    }

//    [Theory]
//    [InlineData(typeof(Antelope))]
//    [InlineData(typeof(Lion))]
//    public void MoveAnimal_AnimalMoved_AnimalMovedWithCorrectSpeed(Type animalType)
//    {
//        // Arrange
//        IAnimal animal;
//        if (animalType == typeof(Antelope))
//        {
//            animal = new Antelope();
//        }
//        else if (animalType == typeof(Lion))
//        {
//            animal = new Lion();
//        }
//        else
//        {
//            throw new ArgumentException("Invalid animal type");
//        }

//        _gameEngine.AddAnimal(animal);
//        var oldX = animal.X;
//        var oldY = animal.Y;

//        // Act
//        _gameEngine.MoveAnimal(animal);

//        // Assert

//    }

//    [Theory]
//    [InlineData(typeof(Antelope))]
//    [InlineData(typeof(Lion))]
//    public void MoveAnimal_AnimalMoved_AnimalDoesNotMoveOutsideGameField(Type animalType)
//    {
//        // Arrange
//        var animal = Activator.CreateInstance(animalType) as IAnimal;
//        animal.X = 0;
//        animal.Y = 0;
//        if (animal is Antelope)
//        {
//            Antelope antelope = new Antelope { Speed = 100 };
//            _gameEngine.AddAnimal(antelope);
//            animal = antelope;
//        }
//        else if (animal is Lion)
//        {
//            Lion lion = new Lion { Speed = 100 };
//            _gameEngine.AddAnimal(lion);
//            animal = lion;
//        }

//        // Act
//        _gameEngine.MoveAnimal(animal);

//        // Assert
//        Assert.True(animal.X >= 0 && animal.X < _gameEngine.GetGameField.GetLength(0));
//        Assert.True(animal.Y >= 0 && animal.Y < _gameEngine.GetGameField.GetLength(1));
//    }

//    [Theory]
//    [InlineData(typeof(Antelope))]
//    [InlineData(typeof(Lion))]
//    public void MoveAnimal_AnimalMoved_AnimalHealthDecreased(Type animalType)
//    {
//        // Arrange
//        IAnimal animal = Activator.CreateInstance(animalType) as IAnimal;
//        _gameEngine.AddAnimal(animal);

//        var initialHealth = animal.Health;

//        // Act
//        _gameEngine.MoveAnimal(animal);

//        var finalHealth = animal.Health;

//        // Assert
//        Assert.True(finalHealth == initialHealth - Constant.HealthDecreasePerMove);
//    }

//    [Theory]
//    [InlineData(typeof(Antelope))]
//    [InlineData(typeof(Lion))]
//    public void MoveAnimal_AnimalHealthZero_DeadAnimalRemovedFromGame(Type animalType)
//    {

//    }


//    [Fact]
//    public void InteractWith_LionInteractsWithAntelope_LionHealthIncreasedByOne()
//    {

//    }

//    [Fact]
//    public void InteractWith_AntelopeInteractsWithLion_AntelopeHealthZero()
//    {

//    }

//    [Fact]
//    public void Test_CreateAnimalOnBirth()
//    {
//        // Arrange
//        var animal1 = new Antelope { X = 5, Y = 5 };
//        var animal2 = new Antelope { X = 5, Y = 6 };

//        _gameEngine.AddAnimal(animal1);
//        _gameEngine.AddAnimal(animal2);

//        // Act
//        // Call the MoveAnimal method enough times to trigger the birth of a new animal
//        for (int i = 0; i < 3; i++)
//        {
//            // Here we're directly calling CreateAnimalOnBirth method instead of MoveAnimal
//            // because we want to keep the animals at the same position for 3 iterations
//            _gameEngine.CreateAnimalOnBirth(animal1);
//            _gameEngine.CreateAnimalOnBirth(animal2);
//        }

//        // Assert
//        // Check that a new animal has been added to the game field
//        var animalsCount = _gameEngine._animals.Count; // Use the _animals list directly
//        Assert.Equal(3, animalsCount);
//    }
//}

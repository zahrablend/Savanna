namespace CodeLibraryTests;

public class TestAnimal : IAnimal
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Speed { get; init; }
    public int VisionRange { get; init; }
    public double Health { get; set; }
}

public class AnimalCreatorTests
{
    private Mock<GameLogicOrchestrator> _logicMock;
    private AnimalCreator _animalCreator;

    public AnimalCreatorTests()
    {
        _logicMock = new Mock<GameLogicOrchestrator>();
        _animalCreator = new AnimalCreator(_logicMock.Object);
    }

    [Fact]
    public void CreateAnimalOnBirth_ShouldAddNewAnimalWhenTwoAnimalsOfSameTypeAreNeighboursForThreeConsecutiveRounds()
    {
        // Arrange
        var animal1 = new TestAnimal { X = 5, Y = 5, Speed = 0 };
        var animal2 = new TestAnimal { X = 6, Y = 5, Speed = 0 };

        // Act & Assert
        for (int i = 0; i < 3; i++)
        {
            _animalCreator.CreateAnimalOnBirth(animal1);
            _animalCreator.CreateAnimalOnBirth(animal2);
        }

        // Verify that AddAnimal was called once (a new animal was added)
        _logicMock.Verify(l => l.AddAnimal(It.IsAny<IAnimal>()), Times.Once);
    }
}




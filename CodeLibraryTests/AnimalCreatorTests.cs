namespace CodeLibraryTests;

public class AnimalCreatorTests
{
    private readonly Mock<GameLogicOrchestrator> _logicMock;
    private readonly AnimalCreator _animalCreator;

    public AnimalCreatorTests()
    {
        _logicMock = new Mock<GameLogicOrchestrator>();
        _animalCreator = new AnimalCreator(_logicMock.Object);
    }

    [Fact]
    public void CreateAnimalOnBirth_TwoAnimalsOfSameTypeAreNeighboursForThreeConsecutiveRounds_AddNewAnimal()
    {
        // Arrange
        var animal1 = new Antelope { X = 5, Y = 5 };
        var animal2 = new Antelope { X = 6, Y = 5 };
        _animalCreator.SetAnimals(new List<IAnimal> { animal1, animal2 });

        // Act
        for (int i = 0; i < 3; i++)
        {
            _animalCreator.CreateAnimalOnBirth();
        }

        // Assert
        _logicMock.Verify(l => l.AddAnimal(It.IsAny<IAnimal>()), Times.Once);
    }
}




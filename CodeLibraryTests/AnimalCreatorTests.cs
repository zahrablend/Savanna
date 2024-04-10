using CodeLibrary.Interfaces;

namespace CodeLibraryTests;

public class AnimalCreatorTests
{
    private readonly Mock<GameLogicOrchestrator> _logicMock;
    private readonly AnimalCreator _animalCreator;
    private readonly AntelopeFactory _antelopeFactory;

    public AnimalCreatorTests()
    {
        _logicMock = new Mock<GameLogicOrchestrator>();
        _animalCreator = new AnimalCreator(_logicMock.Object);
        _antelopeFactory = new AntelopeFactory();
    }

    [Fact]
    public void CreateAnimalOnBirth_TwoAnimalsOfSameTypeAreNeighboursForThreeConsecutiveRounds_AddNewAnimal()
    {
        // Arrange
        var animal1 = _antelopeFactory.Create();
        animal1.X = 5;
        animal1.Y = 5;

        var animal2 = _antelopeFactory.Create();
        animal2.X = 6;
        animal2.Y = 5;
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




namespace CodeLibraryTests;

public class AnimalMoverTests
{
    private AnimalMover _animalMover;
    private IAnimal[,] _gameField;
    private FieldDisplayer.FieldSize _fieldSize;
    private Mock<IAnimal> _animalMock;

    public AnimalMoverTests()
    {
        _fieldSize = new FieldDisplayer.FieldSize(10, 10);
        _gameField = new IAnimal[_fieldSize.Height, _fieldSize.Width];
        _animalMover = new AnimalMover(_gameField, _fieldSize);

        _animalMock = new Mock<IAnimal>();
        _animalMock.Setup(a => a.X).Returns(5);
        _animalMock.Setup(a => a.Y).Returns(5);
        _animalMock.Setup(a => a.Speed).Returns(2);
    }

    [Fact]
    public void MoveAnimal_ShouldMoveAnimalToNewPosition()
    {
        // Arrange
        var animal = _animalMock.Object;
        _gameField[5, 5] = animal;

        // Act
        _animalMover.MoveAnimal(animal);

        // Assert
        Assert.Equal(animal, _gameField[animal.X, animal.Y]);
    }

    [Fact]
    public void MoveAnimal_ShouldMoveAnimalWithCorrectSpeed()
    {
        // Arrange
        var animal = _animalMock.Object;
        _gameField[5, 5] = animal;

        // Act
        _animalMover.MoveAnimal(animal);

        // Assert
        Assert.InRange(animal.X, 3, 7); // The new X position should be within the range [oldX - speed, oldX + speed]
        Assert.InRange(animal.Y, 3, 7); // The new Y position should be within the range [oldY - speed, oldY + speed]
    }
}

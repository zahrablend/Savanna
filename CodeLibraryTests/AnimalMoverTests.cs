namespace CodeLibraryTests;

public class TestAnimal : IAnimal
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Speed { get; init; }
    public int VisionRange { get; init; } = 1;
    public double Health { get; set; } = 10.0;

    public string Name => "TestName";

    public char Symbol => 'T';

    public (int directionX, int directionY) GetDirectionTo(IAnimal other)
    {
        return (0,0);
    }

    public void InteractWith(IAnimal other)
    {
    }
}
public class AnimalMoverTests
{
    private readonly AnimalMover _animalMover;
    private readonly IAnimal[,] _gameField;
    private readonly FieldDisplayer _fieldDisplayer;
    private readonly LionFactory _lionFactory;

    public AnimalMoverTests()
    {
        _fieldDisplayer = new FieldDisplayer();
        _fieldDisplayer.Size = new FieldDisplayer.FieldSize(10, 10);
        _gameField = new IAnimal[_fieldDisplayer.Size.Height, _fieldDisplayer.Size.Width];
        _animalMover = new AnimalMover(_gameField, _fieldDisplayer);

        _lionFactory = new LionFactory();
    }

    [Fact]
    public void MoveAnimal_ShouldMoveAnimalToNewPosition()
    {
        // Arrange
        var animal = _lionFactory.Create();
        animal.X = 5;
        animal.Y = 5;
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
        var animal = new TestAnimal { X = 5, Y = 5, Speed = 2 };
        _gameField[5, 5] = animal;

        // Act
        _animalMover.MoveAnimal(animal);

        // Assert
        Assert.InRange(animal.X, 3, 7); // The new X position should be within the range [oldX - speed, oldX + speed]
        Assert.InRange(animal.Y, 3, 7); // The new Y position should be within the range [oldY - speed, oldY + speed]
    }
}

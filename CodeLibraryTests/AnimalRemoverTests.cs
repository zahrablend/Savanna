using CodeLibrary.Interfaces;

namespace CodeLibraryTests;

public class AnimalRemoverTests
{
    private readonly AnimalRemover _animalRemover;
    private readonly IAnimal[,] _gameField;
    private readonly List<IAnimal> _animals;

    public AnimalRemoverTests()
    {
        _gameField = new IAnimal[10, 10];
        _animals = new List<IAnimal>();
        _animalRemover = new AnimalRemover(_gameField, _animals);
    }

    [Fact]
    public void RemoveAnimalOnDeath_AnimalHealthIsZero_ShouldRemoveAnimalFromList()
    {
        // Arrange
        var animal = new Lion { X = 5, Y = 5, Health = 0 }; 
        _gameField[5, 5] = animal;
        _animals.Add(animal);

        // Act
        _animalRemover.RemoveAnimalOnDeath(animal);

        // Assert
        Assert.DoesNotContain(animal, _animals);
    }

    [Fact]
    public void RemoveAnimalOnDeath_AnimalHealthIsZero_ShouldRemoveAnimalFromGameField()
    {
        // Arrange
        var animal = new Lion { X = 5, Y = 5, Health = 0 }; 
        _gameField[5, 5] = animal;
        _animals.Add(animal);

        // Act
        _animalRemover.RemoveAnimalOnDeath(animal);

        // Assert
        Assert.Null(_gameField[5, 5]);

    }
}


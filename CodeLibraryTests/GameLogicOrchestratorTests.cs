namespace CodeLibraryTests;

public class GameLogicOrchestratorTests
{
    private readonly GameLogicOrchestrator _orchestrator;

    public GameLogicOrchestratorTests()
    {
        // Initialize orchestrator with a field size of your choice
        _orchestrator = new GameLogicOrchestrator(new FieldDisplayer.FieldSize(10,10), new FieldDisplayer());
    }

    [Fact]
    public void AddAnimal_ShouldAddAnimalToAnimalsList()
    {
        // Arrange
        var animal = new Lion(); // Replace YourAnimalClass with your actual class that implements IAnimal

        // Act
        _orchestrator.AddAnimal(animal);

        // Assert
        Assert.Contains(animal, _orchestrator.GetAnimals);
    }

    [Fact]
    public void AddAnimal_ShouldAddAnimalToGameField()
    {

        // Arrange
        var animal = new Lion(); // Replace YourAnimalClass with your actual class that implements IAnimal

        // Act
        _orchestrator.AddAnimal(animal);

        // Assert
        Assert.Contains(animal, _orchestrator.GetGameField.Cast<IAnimal>());
    }
}

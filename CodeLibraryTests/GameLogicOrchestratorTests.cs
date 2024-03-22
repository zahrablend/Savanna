namespace CodeLibraryTests;

public class GameLogicOrchestratorTests
{
    private readonly GameLogicOrchestrator _orchestrator;
    private readonly LionFactory _lionFactory;

    public GameLogicOrchestratorTests()
    {
        _orchestrator = new GameLogicOrchestrator(new FieldDisplayer { Size = new FieldDisplayer.FieldSize(10, 10) });
        _lionFactory = new LionFactory();
    }

    [Fact]
    public void AddAnimal_ShouldAddAnimalToAnimalsList()
    {
        // Arrange
        var animal = _lionFactory.Create();

        // Act
        _orchestrator.AddAnimal(animal);

        // Assert
        Assert.Contains(animal, _orchestrator.GetAnimals);
    }

    [Fact]
    public void AddAnimal_ShouldAddAnimalToGameField()
    {

        // Arrange
        var animal = _lionFactory.Create();

        // Act
        _orchestrator.AddAnimal(animal);

        // Assert
        Assert.Contains(animal, _orchestrator.GetGameField.Cast<IAnimal?>().Where(a => a != null).Select(a => a!));
    }
}

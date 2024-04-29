namespace Common.Interfaces;

public interface IAnimalFactory
{
    string? Species { get; }
    char Symbol { get; }
    string? Icon { get; }
    IAnimal Create();
}

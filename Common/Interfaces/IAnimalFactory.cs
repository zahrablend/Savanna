namespace Common.Interfaces;

public interface IAnimalFactory
{
    char Symbol { get; }
    IAnimal Create();
}

using Common.Interfaces;

namespace Savanna.Web.Services;

public class AnimalGameFieldFactoryService : IGameFieldFactory
{
    public IGameField Create(int width, int height)
    {
        return new AnimalGameFieldService(width, height);
    }
}

using Common.Interfaces;

namespace Savanna.ConsoleApp;

public class CharGameFieldFactory : IGameFieldFactory
{
    public IGameField Create(int width, int height)
    {
        return new CharGameField(width, height);
    }
}

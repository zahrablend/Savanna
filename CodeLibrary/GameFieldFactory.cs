using Common.Interfaces;

namespace CodeLibrary;

public class GameFieldFactory : IGameFieldFactory
{
    public IGameField Create(int width, int height)
    {
        return new GameField(width, height);
    }
}

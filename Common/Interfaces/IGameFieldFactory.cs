namespace Common.Interfaces;

public interface IGameFieldFactory
{
    IGameField Create(int width, int height);
}

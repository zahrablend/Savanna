using Common.Entities;

namespace Common.Interfaces;

public interface IGameRepository
{
    Task SaveGame(GameEntity gameEntity);
    Task<GameEntity> LoadGame(int id);
}

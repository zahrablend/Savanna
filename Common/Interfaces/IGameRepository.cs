using Common.Entities;

namespace Common.Interfaces;

public interface IGameRepository
{
    Task SaveGame(GameEntity gameEntity);
    Task<GameEntity> LoadGame(int id);
    Task<IEnumerable<GameEntity>> GetSavedGames(Guid userId);
    Task<IEnumerable<GameEntity>> GetGamesByUserId(Guid userId);
}

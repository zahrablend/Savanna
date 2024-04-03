using Common.Entities;
using Common.Interfaces;

namespace Savanna.Web.Services;

public class GameService
{
    private readonly IGameRepository _gameRepository;

    public GameService(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }


    public async Task SaveGameState(GameEntity gameEntity)
    {
        await _gameRepository.SaveGame(gameEntity);
    }



    public async Task<GameEntity> LoadGameState(int id)
    {
        return await _gameRepository.LoadGame(id);
    }


    public async Task<IEnumerable<GameEntity>> GetSavedGames(Guid userId)
    {
        var games = await _gameRepository.GetGamesByUserId(userId);
        return games ?? new List<GameEntity>();
    }

}

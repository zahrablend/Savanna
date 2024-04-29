using Common.Entities;
using Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Savanna.Infrastructure;

public class GameRepository : IGameRepository
{
    private readonly GameContext _context;

    public GameRepository(GameContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<GameEntity>> GetGamesByUserId(Guid userId)
    {
        return await _context.Games.Where(game => game.UserId == userId).ToListAsync();
    }

    public async Task<IEnumerable<GameEntity>> GetSavedGames(Guid userId)
    {
        return await _context.Games.Where(game => game.UserId == userId).ToListAsync();
    }

    public async Task<GameEntity> LoadGame(int id)
    {
        return await _context.Games.FindAsync(id);
    }

    public async Task SaveGame(GameEntity gameEntity)
    {
        _context.Games.Add(gameEntity);
        await _context.SaveChangesAsync();
    }
}

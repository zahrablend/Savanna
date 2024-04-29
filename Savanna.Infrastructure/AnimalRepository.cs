using Common.Entities;
using Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Savanna.Infrastructure;

public class AnimalRepository : IAnimalRepository
{
    private readonly GameContext _context;

    public AnimalRepository(GameContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AnimalEntity>> GetAnimalsByGameId(int gameId)
    {
        return await _context.Animals.Where(animal => animal.GameId == gameId).ToListAsync();
    }

    public async Task<IEnumerable<AnimalEntity>> GetAnimalsByUserId(Guid userId)
    {
        return await _context.Animals.Where(animal => animal.Game.UserId == userId).ToListAsync();
    }

    public async Task<AnimalEntity> GetAnimal(int id)
    {
        return await _context.Animals.FindAsync(id);
    }

    public async Task SaveAnimal(AnimalEntity animalEntity)
    {
        _context.Animals.Add(animalEntity);
        await _context.SaveChangesAsync();
    }
}

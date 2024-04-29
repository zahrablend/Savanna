using Common.Entities;

namespace Common.Interfaces;

public interface IAnimalRepository
{
    Task SaveAnimal(AnimalEntity animalEntity);
    Task<AnimalEntity> GetAnimal(int id);
    Task<IEnumerable<AnimalEntity>> GetAnimalsByGameId(int gameId);
    Task<IEnumerable<AnimalEntity>> GetAnimalsByUserId(Guid userId);
}

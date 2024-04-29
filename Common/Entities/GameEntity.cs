using System.ComponentModel.DataAnnotations;

namespace Common.Entities;

public class GameEntity
{
    [Key]
    public int Id { get; set; }
    public Guid UserId { get; }
    public string? Name { get; set; }
    public string? GameState { get; set; }
    public int GameIteration { get; set; }

    public ICollection<AnimalEntity> Animals { get; set; } // Navigation property


    public GameEntity()
    {
    }


    public GameEntity(Guid userId)
    {
        UserId = userId;
    }
}

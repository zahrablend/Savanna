using System.ComponentModel.DataAnnotations;

namespace Common.Entities;

public class AnimalEntity : GameEntity
{
    [Key]
    public int AnimalId { get; set; }
    public string? Species { get; set; }
    public int Age { get; set; }
    public double Health { get; set; }
    public int Offspring { get; set; }

    public int GameId { get; set; } // Foreign key property
    public GameEntity Game { get; set; } // Navigation property

    public AnimalEntity(string? species, double health, int offspring)
    {
        Species = species;
        Health = health;
        Offspring = offspring;
    }
}

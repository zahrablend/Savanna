namespace Common.Entities;

public class GameEntity
{
    public int Id { get; set; }
    public Guid UserId { get; }
    public string? Name { get; set; }
    public string? GameState { get; set; }
    public int GameIteration { get; set; }
    

    public GameEntity()
    {
    }


    public GameEntity(Guid userId)
    {
        UserId = userId;
    }
}

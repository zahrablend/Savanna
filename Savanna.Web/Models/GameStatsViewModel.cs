namespace Savanna.Web.Models
{
    public class GameStatsViewModel
    {
        public int GameId { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string GameState { get; set; }
        public int GameIteration { get; set; }
        public ICollection<AnimalStatsViewModel> Animals { get; set; }
    }
}

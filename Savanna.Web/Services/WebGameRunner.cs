using CodeLibrary.Interfaces;
using CodeLibrary;

namespace Savanna.Web.Services
{
    public class WebGameRunner : IGameRunner
    {
        public async Task Run(Game game)
        {
            await game.Run();
        }
    }
}

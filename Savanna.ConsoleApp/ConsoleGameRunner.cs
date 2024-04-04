using CodeLibrary.Interfaces;
using CodeLibrary;
using Common.Interfaces;

namespace Savanna.ConsoleApp
{
    public class ConsoleGameRunner : IGameRunner
    {
        public async Task Run(Game game)
        {
            while (game.IsRunning)
            {
                await game.Run();
            }
        }
    }
}

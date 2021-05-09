using RogueSharpTutorial.Core;
using SadConsoleGame.Systems;

namespace SadConsoleGame.Interfaces
{
    public interface IBehaviour
    {
        bool Act(Monster monster, CommandSystem commandSystem);
    }
}
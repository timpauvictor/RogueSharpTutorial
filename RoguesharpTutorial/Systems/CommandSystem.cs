
using RogueSharpTutorial.Core;

namespace RogueSharpTutorial.Systems;

public class CommandSystem
{
    public bool MovePlayer(Direction direction)
    {
        int x = Program.Player.X;
        int y = Program.Player.Y;

        switch (direction)
        {
            case Direction.Up:
                {
                    y = Program.Player.Y - 1;
                    break;
                }
            case Direction.Down:
                {
                    y = Program.Player.Y + 1;
                    break;
                }
            case Direction.Left:
                {
                    x = Program.Player.X - 1;
                    break;
                }
            case Direction.Right:
                {
                    x = Program.Player.X + 1;
                    break;
                }
            default:
            {
                return false;
            }
        }

        if (Program.DungeonMap.setActorPosition(Program.Player, x, y))
        {
            return true;
        }

        return false;
    }
}
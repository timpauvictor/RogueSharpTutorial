using RogueSharpTutorial.Core;

namespace SadConsoleGame.Systems
{
    public class CommandSystem
    {
        public bool MovePlayer(Directions direction)
        {
            int playerX = Game.player.X;
            int playerY = Game.player.Y;


            switch (direction)
            {
                case Directions.Up:
                {
                    playerY = Game.player.Y - 1;
                    break;
                }
                case Directions.Down:
                {
                    playerY = Game.player.Y + 1;
                    break;
                }
                case Directions.Left:
                {
                    playerX = Game.player.X - 1;
                    break;
                }
                case Directions.Right:
                {
                    playerX = Game.player.X + 1;
                    break;
                }
                default:
                    return false;
            }

            if (Game.DungeonMap.SetActorPosition(Game.player, playerX, playerY))
            {
                return true;
            }

            return false;
        }
    }
}
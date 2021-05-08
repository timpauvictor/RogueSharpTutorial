using RogueSharpTutorial.Core;

namespace SadConsoleGame.Systems
{
    public class CommandSystem
    {
        public bool MovePlayer(Directions direction)
        {
            int playerX = Game.Player.X;
            int playerY = Game.Player.Y;


            switch (direction)
            {
                case Directions.Up:
                {
                    playerY = Game.Player.Y - 1;
                    break;
                }
                case Directions.Down:
                {
                    playerY = Game.Player.Y + 1;
                    break;
                }
                case Directions.Left:
                {
                    playerX = Game.Player.X - 1;
                    break;
                }
                case Directions.Right:
                {
                    playerX = Game.Player.X + 1;
                    break;
                }
                default:
                    return false;
            }

            if (Game.DungeonMap.SetActorPosition(Game.Player, playerX, playerY))
            {
                return true;
            }

            return false;
        }
    }
}
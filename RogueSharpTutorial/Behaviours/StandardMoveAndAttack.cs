using RogueSharp;
using RogueSharpTutorial.Core;
using SadConsoleGame.Interfaces;
using SadConsoleGame.Systems;

namespace SadConsoleGame.Behaviours
{
    public class StandardMoveAndAttack: IBehaviour
    {
        public bool Act(Monster monster, CommandSystem commandSystem)
        {
            DungeonMap dungeonMap = Game.DungeonMap;
            Player player = Game.Player;
            FieldOfView<DungeonCell> monsterFov = new FieldOfView<DungeonCell>(dungeonMap);
            
           //if the monster has not been alerted, compute a field of view
           //use the monsters awareness value for the distance in the fov check
           //if the player is in the monsters fov then alert it
           //add a message to the message log regarding this alerted status
           if (!monster.TurnsAlerted.HasValue)
           {
               monsterFov.ComputeFov(monster.X, monster.Y, monster.Awareness, true);
               if (monsterFov.IsInFov(player.X, player.Y))
               {
                   Game.MessageLog.Add($"{monster.Name} is eager to fight {player.Name}");
                   monster.TurnsAlerted = 1;
               }
           }

           if (monster.TurnsAlerted.HasValue)
           {
               //before we find a path, make sure to make the monster and player cells walkable
               dungeonMap.SetIsWalkable(monster.X, monster.Y, true);
               dungeonMap.SetIsWalkable(player.X, player.Y, true);
               PathFinder<DungeonCell> pathFinder = new PathFinder<DungeonCell>(dungeonMap);
               Path path = null;

               try
               {
                   path = pathFinder.ShortestPath(
                       dungeonMap.GetCell(monster.X, monster.Y),
                       dungeonMap.GetCell(player.X, player.Y)
                   );
               }
               catch (PathNotFoundException)
               {
                   //the monster can see a player but cannot find a path to him
                   Game.MessageLog.Add($"{monster.Name} waits for a turn");
               }
                
               dungeonMap.SetIsWalkable(monster.X, monster.Y, false);
               dungeonMap.SetIsWalkable(player.X, player.Y, false);
               
               //in the case that there was a path, tell the command system to move the monster
               if (path != null)
               {
                   try
                   {
                      commandSystem.MoveMonster(monster, path.StepForward()); 
                   }
                   catch (NoMoreStepsException)
                   {
                       Game.MessageLog.Add($"{monster.Name} growls in frustration");
                   }
                  
               }

               monster.TurnsAlerted++;
               
               //lose alerted status after 15 turns
               if (monster.TurnsAlerted > 15)
               {
                   monster.TurnsAlerted = null;
               }
           }

           return true;
        }
    }
}
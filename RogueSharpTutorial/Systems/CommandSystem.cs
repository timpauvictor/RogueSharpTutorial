using System.Text;
using RogueSharp.DiceNotation;
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
            
            //if the player wasn't able to just move there check if there is a monster in the location
            Monster monster = Game.DungeonMap.GetMonsterAt(playerX, playerY);
            if (monster != null)
            {
                Attack(Game.Player, monster);
                return true;
            }

            return false;
        }

        public void Attack(Actor attacker, Actor defender)
        {
            StringBuilder attackMessage = new StringBuilder();
            StringBuilder defenseMessage = new StringBuilder();
            
            int hits = ResolveAttack(attacker, defender, attackMessage);
            int blocks = ResolveDefense(defender, hits, attackMessage, defenseMessage);
            
            Game.MessageLog.Add(attackMessage.ToString());
            if (!string.IsNullOrEmpty(defenseMessage.ToString()))
            {
                Game.MessageLog.Add(defenseMessage.ToString());
            }
            
            int damage = hits - blocks;
            
            ResolveDamage(defender, damage);
        }
        
        //the attacker rolls based on his stats to see if he gets any hits
        private int ResolveAttack(Actor attacker, Actor defender, StringBuilder attackMessage)
        {
            int hits = 0;

            attackMessage.AppendFormat("{0} attacks {1} and rolls: ", attacker.Name, defender.Name);
            
            //roll a number of 100 sided dice equal to the attack value of the attacking attacker
            DiceExpression attackDice = new DiceExpression().Dice(attacker.Attack, 100);
            DiceResult attackResult = attackDice.Roll();

            foreach (TermResult termResult in attackResult.Results)
            {
                attackMessage.Append(termResult.Value + ",");
                //compare the value to 100 minus the attack chance and add a hit if it's greater
                if (termResult.Value >= 100 - attacker.AttackChance)
                {
                    hits++;
                }
            }

            return hits;
        }
        
        //the defender rolls based on his stats to see if he blocks any of the hits from the attacker
        private int ResolveDefense(Actor defender, int hits, StringBuilder attackMessage, StringBuilder defenseMessage)
        {
            int blocks = 0;

            if (hits > 0)
            {
                attackMessage.AppendFormat("scoring {0} hits.", hits);
                defenseMessage.AppendFormat(" {0} defends and rolls:", defender.Name);

                //roll a number of 100-sided dice equal to the defense value of the defending actor
                DiceExpression defenseDice = new DiceExpression().Dice(defender.Defense, 100);
                DiceResult defenseRoll = defenseDice.Roll();

                //look at the result of each die that was rolled
                foreach (TermResult termResult in defenseRoll.Results)
                {
                    defenseMessage.Append(termResult.Value + ",");
                    //compare the value  to 100 minus the defense chance and add a block if it's greater
                    if (termResult.Value >= 100 - defender.DefenseChance)
                    {
                        blocks++;
                    }
                }

                defenseMessage.AppendFormat("resulting in {0} blocks", blocks);
            }
            else
            {
                attackMessage.Append("and misses completely");
            }

            return blocks;
        }

        private void ResolveDamage(Actor defender, int damage)
        {
            if (damage > 0)
            {
                defender.Health = defender.Health - damage;

                Game.MessageLog.Add($"{defender.Name} was hit got {damage} damage");
                if (defender.Health <= 0)
                {
                    ResolveDeath(defender);
                }
            }
            else
            {
                Game.MessageLog.Add($"{defender.Name} blocked all damage");
            }
        }

        private void ResolveDeath(Actor defender)
        {
            if (defender is Player)
            {
                Game.MessageLog.Add($"{defender.Name} was killed. GAME OVER");
            }
            else if (defender is Monster)
            {
                Game.DungeonMap.RemoveMonster((Monster) defender);
                
                Game.MessageLog.Add($"{defender.Name} died and dropped {defender.Gold} gold");
            }
        }
    }
}
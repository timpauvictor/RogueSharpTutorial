using RLNET;
using RogueSharp;
using SadConsoleGame.Interfaces;

namespace RogueSharpTutorial.Core
{
    public class Actor: IActor, IDrawable
    {
        // IActor
        private int _attack;
        private int _attackChance;
        private int _awareness;
        private int _defense;
        private int _defenseChance;
        private int _gold;
        private int _health;
        private int _maxHealth;
        private string _name;
        private int _speed;
 
        public int Attack
        {
            get
            {
                return _attack;
            }
            set
            {
                _attack = value;
            }
        }
 
        public int AttackChance
        {
            get
            {
                return _attackChance;
            }
            set
            {
                _attackChance = value;
            }
        }
 
        public int Awareness
        {
            get
            {
                return _awareness;
            }
            set
            {
                _awareness = value;
            }
        }
 
        public int Defense
        {
            get
            {
                return _defense;
            }
            set
            {
                _defense = value;
            }
        }
 
        public int DefenseChance
        {
            get => _defenseChance;
            set => _defenseChance = value;
        }
 
        public int Gold
        {
            get => _gold;
            set => _gold = value;
        }
 
        public int Health
        {
            get => _health;
            set => _health = value;
        }
 
        public int MaxHealth
        {
            get
            {
                return _maxHealth;
            }
            set
            {
                _maxHealth = value;
            }
        }
 
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            } 
        }
 
        public int Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                _speed = value;
            }
        }
        
        //IDrawable
        public RLColor ActorColor { get; set; }
        public char Symbol { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public void Draw(RLConsole in_console, DungeonMap in_map)
        {
            //don't draw actors in cells that haven't been explored
            if (!in_map.GetCell(X, Y).IsExplored)
            {
                return;
            }
            
            //only draw the actor when in fov
            if (in_map.IsInFov(X, Y))
            {
                in_console.Set(X, Y, ActorColor, Colors.FloorBackgroundFov, Symbol);
            }
            else
            {
                in_console.Set(X, Y, ActorColor, Colors.FloorBackgroundFov, '.');
            }
        }
    }
}
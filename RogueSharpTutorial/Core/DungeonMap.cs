using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RLNET;
using RogueSharp;
using SadConsoleGame;

namespace RogueSharpTutorial.Core
{
    public class DungeonCell : Cell
    {
        public bool IsExplored
        {
            get;
            set;
        }
    }
    
    public class DungeonMap : Map<DungeonCell>
    {

        private readonly FieldOfView<DungeonCell> _fieldOfView;
        public List<Rectangle> Rooms;
        private readonly List<Monster> _monsters;

        public DungeonMap()
        {
            _fieldOfView = new FieldOfView<DungeonCell>(this);
            Rooms = new List<Rectangle>();
            _monsters = new List<Monster>();
        }

        public bool IsExplored(int x, int y)
        {
            return this[x, y].IsExplored;
        }

        public void SetCellProperties(int x, int y, bool in_isTransparent, bool in_isWalkable, bool in_isExplored)
        {
            this[x, y].IsTransparent = in_isTransparent;
            this[x, y].IsWalkable = in_isWalkable;
            this[x, y].IsExplored = in_isExplored;
        }

        public void AddPlayer(Player in_player)
        {
            Game.Player = in_player;
            SetIsWalkable(in_player.X, in_player.Y, false);
            updatePlayerFieldOfView();
        }

        public void updatePlayerFieldOfView()
        {
            Player player = Game.Player;
            //compute fov based on player awareness and location
            ComputeFov(player.X, player.Y, player.Awareness, true);
            //mark all cells in field of view as having been explored
            foreach (DungeonCell cell in GetAllCells())
            {
                if (IsInFov(cell.X, cell.Y))
                {
                    SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
                }
            }
            
        }

        public bool SetActorPosition(Actor actor, int in_x, int in_y)
        {
            DungeonCell x = GetCell(in_x, in_y);
            if (GetCell(in_x, in_y).IsWalkable)
            {
                SetIsWalkable(actor.X, actor.Y, true);
                //update actor position
                actor.X = in_x;
                actor.Y = in_y;
                //the new cell the actor is on is now not walkable
                SetIsWalkable(actor.X, actor.Y, false);
                //don't forget to update the field of view if we just repositioned the player
                if (actor is Player)
                {
                    updatePlayerFieldOfView();
                }

                return true;
            }

            return false;
        }

        public void SetIsWalkable(int x, int y, bool isWalkable)
        {
            DungeonCell cell = GetCell(x, y);
            SetCellProperties(cell.X, cell.Y, cell.IsTransparent, isWalkable, cell.IsExplored);
        }

        public bool IsInFov(int x, int y)
        {
            return _fieldOfView.IsInFov(x, y);
        }

        public ReadOnlyCollection<DungeonCell> ComputeFov(int in_xOrigin, int in_yOrigin, int in_radius, bool in_lightWalls)
        {
            return _fieldOfView.ComputeFov(in_xOrigin, in_yOrigin, in_radius, in_lightWalls);
        }
        
        public void Draw(RLConsole in_mapConsole, RLConsole in_statConsole)
        {
            //the draw method will be called each time the map is updated
            //it will render all of the symbols/colors for each cell to the subconsole
            in_mapConsole.Clear();
            foreach (DungeonCell cell in GetAllCells())
            {
                SetConsoleSymbolForCell(in_mapConsole, cell);
            }

            int i = 0;
            foreach (var monster in _monsters)
            {
                monster.Draw(in_mapConsole, this);

                if (IsInFov(monster.X, monster.Y))
                {
                    monster.DrawStats(in_statConsole, i);
                    Game.guiRedraw = true;
                    i++;
                }
            }
        }
        
        private void SetConsoleSymbolForCell( RLConsole in_console, DungeonCell in_cell)
        {
            //When we haven't explored a cell yet we don't want to draw anything
            if (!in_cell.IsExplored)
            {
                return;
            }
            
            //When a cell is currently in the FOV it should be drawn with lighter colours
            if (IsInFov( in_cell.X, in_cell.Y))
            {
                //Choose the symbol to draw based on if the cell is walkable or not
                if (in_cell.IsWalkable)
                {
                    in_console.Set(in_cell.X, in_cell.Y, Colors.FloorFov, Colors.FloorBackgroundFov, '.');
                }
                else
                {
                    in_console.Set(in_cell.X, in_cell.Y, Colors.WallFov, Colors.WallBackgroundFov, '#');
                }
            }
            //if it's not in the field of view
            else 
            {
                if (in_cell.IsWalkable)
                {
                    in_console.Set(in_cell.X, in_cell.Y, Colors.Floor, Colors.FloorBackground, '.');
                }
                else
                {
                    in_console.Set(in_cell.X, in_cell.Y, Colors.Wall, Colors.WallBackground, '.');
                }
            }
        }

        public void AddMonster(Monster monster)
        {
            _monsters.Add(monster);
            SetIsWalkable(monster.X, monster.Y, false);
        }

        public void RemoveMonster(Monster in_monster)
        {
            _monsters.Remove(in_monster);
            SetIsWalkable(in_monster.X, in_monster.Y, true);
        }

        public Monster GetMonsterAt(int searchX, int searchY)
        {
            return _monsters.FirstOrDefault(m => m.X == searchX && m.Y == searchY);
        }
        
        //look for a random location that is inside a room and is walkable
        public Point GetRandomWalkableLocationInRoom(Rectangle room)
        {
            if (DoesRoomHaveWalkableSpace(room))
            {
                for (int i = 0; i < 100; i++)
                {
                    int x = Game.Random.Next(1, room.Width - 2) + room.X;
                    int y = Game.Random.Next(1, room.Height - 2) + room.Y;
                    if (IsWalkable(x, y))
                    {
                        return new Point(x, y);
                    }
                }
            }
            //if we didn't find a walkable location in the room we should return null
            return new Point(-1, -1);
        }
        
        //iterate through each cell in a room and return true if any are walkable
        public bool DoesRoomHaveWalkableSpace(Rectangle room)
        {
            for (int x = 1; x <= room.Width - 2; x++) //- 2 for one wall on each side
            {
                for (int y = 1; y <= room.Height - 2; y++)
                {
                    if (IsWalkable(x + room.X, y + room.Y))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
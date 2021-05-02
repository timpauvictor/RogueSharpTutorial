using System.Collections.ObjectModel;
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

        public DungeonMap()
        {
            _fieldOfView = new FieldOfView<DungeonCell>(this);
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

        public void updatePlayerFieldOfView()
        {
            Player player = Game.player;
            //compute fov based on player awareness and location
            ComputeFov(player.X, player.Y, player.Awareness, true);
            //mark all cells in field of view as having been explored
            foreach (DungeonCell cell in GetAllCells())
            {
                if (IsInFov(cell.X, cell.Y))
                {
                    SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, cell.IsExplored);
                }
            }
            
        }

        public bool IsInFov(int x, int y)
        {
            return _fieldOfView.IsInFov(x, y);
        }

        public ReadOnlyCollection<DungeonCell> ComputeFov(int in_xOrigin, int in_yOrigin, int in_radius, bool in_lightWalls)
        {
            return _fieldOfView.ComputeFov(in_xOrigin, in_yOrigin, in_radius, in_lightWalls);
        }
        
        public void Draw(RLConsole in_mapConsole)
        {
            //the draw method will be called each time the map is updated
            //it will render all of the symbols/colors for each cell to the subconsole
            in_mapConsole.Clear();
            foreach (DungeonCell cell in GetAllCells())
            {
                SetConsoleSymbolForCell(in_mapConsole, cell);
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
    }
    
}
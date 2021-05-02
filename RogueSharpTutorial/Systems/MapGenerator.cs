using RogueSharpTutorial.Core;

namespace SadConsoleGame.Systems
{
    public class MapGenerator
    {
        private readonly int _width;
        private readonly int _height;

        private readonly DungeonMap _map;
        
        //Constructing a new mapgenerator requires the dimensions of the maps it will create
        public MapGenerator(int in_width, int in_height)
        {
            _width = in_width;
            _height = in_height;
            _map = new DungeonMap();
        }
        
        //Generate a new dungeon map that is a simple open floor with walls all around the outside
        public DungeonMap CreateMap()
        {
            //init every cell in the map by setting walkable, transparency and explored to true
            _map.Initialize(_width, _height);
            foreach (DungeonCell cell in _map.GetAllCells())
            {
                _map.SetCellProperties(cell.X, cell.Y, true, true, true);
            }

            //set the first and last rows in the map to not be transparent or walkable
            foreach (DungeonCell cell in _map.GetCellsInRows(0, _height - 1))
            {
                _map.SetCellProperties(cell.X, cell.Y, false, false, true);
            }

            //set the first and last columns to not be transparent or walkable
            foreach (DungeonCell cell in _map.GetCellsInColumns(0, _width - 1))
            {
                _map.SetCellProperties(cell.X, cell.Y, false, false, true);
            }

            return _map;
        }
    }
}
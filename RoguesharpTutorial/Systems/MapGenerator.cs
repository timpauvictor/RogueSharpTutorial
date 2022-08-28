using RogueSharp;
using RogueSharpTutorial.Core;

namespace RogueSharpTutorial.Systems;

public class MapGenerator
{
    private readonly int _width;
    private readonly int _height;

    private readonly DungeonMap _map;

    public MapGenerator(int width, int height)
    {
        _width = width;
        _height = height;
        _map = new DungeonMap();
    }

    //generate a new map is a simple open floor with walls
    public DungeonMap CreateMap()
    {
        _map.Initialize(_width, _height);

        foreach (Cell cell in _map.GetAllCells())
        {
            //setting walkable, transparent, and explored to true for all cells
            _map.SetCellProperties(cell.X, cell.Y, true, true, true);
        }

        //set the first and last rows in the map to not be walkable or transparent
        foreach (Cell cell in _map.GetCellsInRows(0, _height - 1))
        {
            _map.SetCellProperties(cell.X, cell.Y, false, false, true);
        }
        
        //set the first and last columns in the map to not be walkable or transparent
        foreach (Cell cell in _map.GetCellsInColumns(0, _width - 1))
        {
            _map.SetCellProperties(cell.X, cell.Y, false, false, true);
        }
        return _map;
    }
}

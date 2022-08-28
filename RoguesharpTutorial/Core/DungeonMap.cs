using RLNET;
using RogueSharp;

namespace RogueSharpTutorial.Core;

public class DungeonMap : Map
{
    public void Draw(RLConsole mapConsole)
    {
        mapConsole.Clear();
        foreach (Cell cell in GetAllCells())
        {
            SetConsoleSymbolForCell(mapConsole, cell);
        }
    }

    private void SetConsoleSymbolForCell(RLConsole console, Cell cell)
    {
        if (!cell.IsExplored)
        {
            return;
        }

        //choose the cell based on what is walkable or not
        // . for floors
        // # for walls
        if (IsInFov(cell.X, cell.Y))
        {
            if (cell.IsWalkable)
            {
                console.Set(cell.X, cell.Y, Colors.FloorFov, Colors.FloorBackgroundFov, '.');
            }
            else
            {
                console.Set(cell.X, cell.Y, Colors.WallFov, Colors.WallBackgroundFov, '#');
            }
        }
        //when we are outside of FOV draw with the darker colours
        else
        {
            if (cell.IsWalkable)
            {
                console.Set(cell.X, cell.Y, Colors.Floor, Colors.FloorBackground, '.');
            }
            else
            {
                console.Set(cell.X, cell.Y, Colors.Wall, Colors.WallBackground, '#');
            }
        }
    }
    
    public void UpdatePlayerFieldOfView()
    {
        Player player = Program.Player;
        ComputeFov(player.X, player.Y, player.Awareness, true);
        foreach (Cell cell in GetAllCells())
        {
            if (IsInFov(cell.X, cell.Y))
            {
                SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
            }
        }
    }
}
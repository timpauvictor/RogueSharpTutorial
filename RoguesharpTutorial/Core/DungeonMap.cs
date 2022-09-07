using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using RLNET;
using RogueSharp;

namespace RogueSharpTutorial.Core;

public class DungeonMap : Map
{

    public List<Rectangle> Rooms;
    
    private readonly List<Monster> _monsters;


    public DungeonMap()
    {
        Rooms = new List<Rectangle>();
        
        _monsters = new List<Monster>();
    }

    public void AddPlayer(Player player)
    {
        Program.Player = player;
        SetIsWalkable(player.X, player.Y, false);
        UpdatePlayerFieldOfView();
    }
    
    public void AddMonster(Monster monster)
    {
        _monsters.Add(monster);
        SetIsWalkable(monster.X, monster.Y, false); // Set the cell the monster is on to not walkable
    }

    public Point GetRandomWalkableLocationInRoom(Rectangle room)
    {
        if (DoesRoomHaveWalkableSpace(room))
        {
            for (int i = 0; i < 100; i++)
            {
                int x = Program.Random.Next(1, room.Width -2) + room.X;
                int y = Program.Random.Next(1, room.Height -2) + room.Y;
                if (GetCell(x, y).IsWalkable)
                {
                    return new Point(x, y);
                }
            }
        }
        
        //if no walkable space is found, return null
        return null;
    }
    
    public bool DoesRoomHaveWalkableSpace(Rectangle room)
    {
        for (int x = 1; x < room.Width - 1; x++)
        {
            for (int y = 1; y < room.Height - 1; y++)
            {
                if (GetCell(room.X + x, room.Y + y).IsWalkable)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void Draw(RLConsole mapConsole)
    {
        mapConsole.Clear();
        foreach (Cell cell in GetAllCells())
        {
            SetConsoleSymbolForCell(mapConsole, cell);
        }

        foreach (Monster monster in _monsters)
        {
            monster.Draw(mapConsole, this);
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

    public bool setActorPosition(Actor actor, int x, int y)
    {
        //only allow actor placement if the cell is walkable
        if (GetCell(x, y).IsWalkable)
        {
            //the cell we just walked on is now walkable again
            SetIsWalkable(actor.X, actor.Y, true);
            actor.X = x;
            actor.Y = y;
            SetIsWalkable(x, y, false);
            
            //don't forget to update player fov
            
            if (actor is Player)
            {
                UpdatePlayerFieldOfView();
            }

            return true;
        }

        return false;
    }

    public void SetIsWalkable(int x, int y, bool isWalkable)
    {
        Cell cell = GetCell(x, y);
        SetCellProperties(x, y, cell.IsTransparent, isWalkable, cell.IsExplored);
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
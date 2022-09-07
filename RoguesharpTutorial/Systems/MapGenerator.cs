using RogueSharp;
using RogueSharpTutorial.Core;
using RogueSharpTutorial.Monsters;

namespace RogueSharpTutorial.Systems;

public class MapGenerator
{
    private readonly int _width;
    private readonly int _height;

    private readonly DungeonMap _map;
    private readonly int _maxRooms;
    private readonly int _roomMaxSize;
    private readonly int _roomMinSize;

    public MapGenerator(int width, int height,
        int maxRooms, int roomMaxSixe, int roomMinSize)
    {
        _width = width;
        _height = height;
        _maxRooms = maxRooms;
        _roomMaxSize = roomMaxSixe;
        _roomMinSize = roomMinSize;
        _map = new DungeonMap();
    }
    
    //generate a new map is a simple open floor with walls
    public DungeonMap CreateMap()
    {
        _map.Initialize(_width, _height);


        for (int r = _maxRooms; r > 0; r--)
        {
            //get a random width and height for the room
            int roomWidth = Program.Random.Next(_roomMinSize, _roomMaxSize);
            int roomHeight = Program.Random.Next(_roomMinSize, _roomMaxSize);
            
            //get a random position for the room that fits on the map
            int roomXPosition = Program.Random.Next(0, _width - roomWidth - 1);
            int roomYPosition = Program.Random.Next(0, _height - roomHeight - 1);
            
            //rep this room as a rectangle class
            Rectangle room = new Rectangle(roomXPosition, roomYPosition, roomWidth, roomHeight);

            //check to see if the room intersects with any other rooms
            bool newRoomIntersects = _map.Rooms.Any(m => m.Intersects(room)); //any returns true if any of the rooms intersect with the new room
            
            //if it doesn't intersect, add it to the list of rooms
            if (!newRoomIntersects)
            {
                _map.Rooms.Add(room);
            }
        }
        
        //iterate through each room and connect them with tunnels
        foreach (Rectangle room in _map.Rooms)
        {
            CreateRoom(room);
        }
        
        //now that every room is create, make hallways between them
        //start at 1 because we don't want to connect the first room to a nonexistent room
        for (int r = 1; r < _map.Rooms.Count; r++)
        {
            //get the center of the room
            int currentRoomCenterX = _map.Rooms[r].Center.X;
            int currentRoomCenterY = _map.Rooms[r].Center.Y;
            
            //get the center of the previous room
            int previousRoomCenterX = _map.Rooms[r - 1].Center.X;
            int previousRoomCenterY = _map.Rooms[r - 1].Center.Y;
            
            //give a 5050 chance of which direction to connect the rooms in first
            if (Program.Random.Next(1, 2) == 1)
            {
                //connect the rooms in the x direction
                CreateHorizontalTunnel(previousRoomCenterX, currentRoomCenterX, previousRoomCenterY);
                CreateVerticalTunnel(previousRoomCenterY, currentRoomCenterY, currentRoomCenterX);
            }
            else
            {
                //connect the rooms in the y direction
                CreateVerticalTunnel(previousRoomCenterY, currentRoomCenterY, previousRoomCenterX);
                CreateHorizontalTunnel(previousRoomCenterX, currentRoomCenterX, currentRoomCenterY);
            }
        }

        PlacePlayer();
        PlaceMonsters();
        return _map;
    }
    
    private void CreateRoom(Rectangle room)
    {
        for (int x = room.Left + 1; x < room.Right; x++)
        {
            for (int y = room.Top + 1; y < room.Bottom; y++)
            {
                _map.SetCellProperties(x, y, true, true);
            }
        }
    }

    private void CreateHorizontalTunnel(int xStart, int xEnd, int yPosition)
    {
        for (int x = Math.Min(xStart, xEnd); x <= Math.Max(xStart, xEnd); x++)
        {
            _map.SetCellProperties(x, yPosition, true, true);
        }
    }
    
    private void CreateVerticalTunnel(int yStart, int yEnd, int xPosition)
    {
        for (int y = Math.Min(yStart, yEnd); y <= Math.Max(yStart, yEnd); y++)
        {
            _map.SetCellProperties(xPosition, y, true, true);
        }
    }


    private void PlacePlayer()
    {
        Player player = Program.Player;
        if (player == null)
        {
            player = new Player();
        }
        
        player.X = _map.Rooms[0].Center.X;
        player.Y = _map.Rooms[0].Center.Y;
        
        _map.AddPlayer(player);
    }

    private void PlaceMonsters()
    {
        foreach (Rectangle room in _map.Rooms)
        {
            //each room has a 60% chance of having a monster
            if (Program.Random.Next(1, 10) <= 6)
            {
                var numberOfMonsters = Program.Random.Next(1, 4);
                for (int i = 0; i < numberOfMonsters; i++)
                {
                    //get a random location in the room
                    Point randomRoomLocation = _map.GetRandomWalkableLocationInRoom(room);
                    
                    //if the location is not already occupied
                    if (randomRoomLocation != null)
                    {
                        var monster = Kobold.Create(1);
                        monster.X = randomRoomLocation.X;
                        monster.Y = randomRoomLocation.Y;
                        _map.AddMonster(monster);
                    }
                }
            }
        }
    }
}

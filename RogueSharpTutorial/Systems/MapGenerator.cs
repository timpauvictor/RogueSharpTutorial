using System;
using System.Linq;
using OpenTK.Graphics.ES20;
using RogueSharp;
using RogueSharpTutorial.Core;
using SadConsole.Effects;

namespace SadConsoleGame.Systems
{
    public class MapGenerator
    {
        private readonly int _width;
        private readonly int _height;
        private readonly int _maxRooms;
        private readonly int _roomMaxSize;
        private readonly int _roomMinSize;

        private readonly DungeonMap _map;
        
        //Constructing a new mapgenerator requires the dimensions of the maps it will create
        public MapGenerator(int in_width, int in_height,
            int in_maxRooms, int in_roomMaxSize, int in_roomMinSize)
        {
            _width = in_width;
            _height = in_height;
            _map = new DungeonMap();
            _maxRooms = in_maxRooms;
            _roomMaxSize = in_roomMaxSize;
            _roomMinSize = in_roomMinSize;
        }
        
        //Generate a new dungeon map that is a simple open floor with walls all around the outside
        public DungeonMap CreateMap()
        {
            _map.Initialize(_width, _height);

            for (int currentRoom = 0; currentRoom < _maxRooms; currentRoom++)
            {
                int roomWidth = Game.Random.Next(_roomMinSize, _roomMaxSize);
                int roomHeight = Game.Random.Next(_roomMinSize, _roomMaxSize);
                int roomXPosition = Game.Random.Next(0, _width - roomWidth - 1);
                int roomYPosition = Game.Random.Next(0, _height - roomHeight - 1);

                var newRoom = new Rectangle(roomXPosition, roomYPosition, roomWidth, roomHeight);

                bool newRoomIntersects = _map.Rooms.Any(room => newRoom.Intersects(room));

                if (!newRoomIntersects)
                {
                    _map.Rooms.Add(newRoom);
                }
            }
            
            //iterate through each room that we wanted placed

            foreach (Rectangle room in _map.Rooms)
            {
                CreateRoom(room);
            }
            
            //start at r = 1 because we don't want to do anything to the first room
            //this is because we connect the second room to the origin of the first room
            //on the first pass
            for (int r = 1; r < _map.Rooms.Count; r++)
            {
                int previousRoomX = _map.Rooms[r - 1].Center.X;
                int previousRoomY = _map.Rooms[r - 1].Center.Y;

                int currentRoomX = _map.Rooms[r].Center.X;
                int currentRoomY = _map.Rooms[r].Center.Y;
                
                //give a 50/50 chance of which direction to start with (vertical or horizontal)
                if (Game.Random.Next(1, 2) == 1)
                {
                    CreateHorizontalTunnel(previousRoomX, currentRoomX, previousRoomY);
                    
                    CreateVerticalTunnel(previousRoomY, currentRoomY, previousRoomX);
                }
                else
                {
                    CreateVerticalTunnel(previousRoomY, currentRoomY, previousRoomX);
                    CreateHorizontalTunnel(previousRoomX, currentRoomX, previousRoomY);
                }
            }
            
            PlacePlayer();
            return _map;
        }

        private void PlacePlayer()
        {
            Player player = Game.Player;
            if (player == null)
            {
                player = new Player();
            }

            player.X = _map.Rooms[0].Center.X;
            player.Y = _map.Rooms[0].Center.Y;
            
            _map.AddPlayer( player );
        }

        private void CreateVerticalTunnel(int yStart, int yEnd, int xPosition)
        {
            for (int y = Math.Min(yStart, yEnd); y <= Math.Max(yStart, yEnd); y++)
            {
                _map.SetCellProperties(xPosition, y, true, true, false);
            }
        }
        private void CreateHorizontalTunnel(int xStart, int xEnd, int yPosition)
        {
            for (int x = Math.Min(xStart, xEnd); x <= Math.Max(xStart, xEnd); x++)
            {
                _map.SetCellProperties(x, yPosition, true, true, false);
            }
        }

        private void CreateRoom(Rectangle room)
        {
            for (int x = room.Left + 1; x < room.Right; x++)
            {
                for (int y = room.Top + 1; y < room.Bottom; y++)
                {
                    _map.SetCellProperties(x, y, true, true, false);
                }
            }
        }
    }
}
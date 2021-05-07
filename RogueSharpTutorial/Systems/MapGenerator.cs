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
                createRoom(room);
            }

            return _map;
        }

        private void createRoom(Rectangle room)
        {
            for (int x = room.Left + 1; x < room.Right; x++)
            {
                for (int y = room.Top + 1; y < room.Bottom; y++)
                {
                    _map.SetCellProperties(x, y, true, true, true);
                }
            }
        }
    }
}
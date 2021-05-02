﻿using OpenTK.Graphics.ES10;
using OpenTK.Input;
using RLNET;
using RogueSharpTutorial.Core;
using SadConsole;
using SadConsoleGame.Systems;

namespace SadConsoleGame
{
    public class Game
    {
        private static bool renderRequired = false;
        
        // The screen height and width are in number of tiles
        private static readonly int _screenWidth = 100;
        private static readonly int _screenHeight = 70;
        private static RLRootConsole _rootConsole;
 
        // The map console takes up most of the screen and is where the map will be drawn
        private static readonly int _mapWidth = 80;
        private static readonly int _mapHeight = 48;
        private static RLConsole _mapConsole;
 
        // Below the map console is the message console which displays attack rolls and other information
        private static readonly int _messageWidth = 80;
        private static readonly int _messageHeight = 11;
        private static RLConsole _messageConsole;
 
        // The stat console is to the right of the map and display player and monster stats
        private static readonly int _statWidth = 20;
        private static readonly int _statHeight = 70;
        private static RLConsole _statConsole;
 
        // Above the map is the inventory console which shows the players equipment, abilities, and items
        private static readonly int _inventoryWidth = 80;
        private static readonly int _inventoryHeight = 11;
        private static RLConsole _inventoryConsole;
        
        enum _gameStates
        {
            MainMenu,
            InGame,
            Paused
        }


        private static bool guiRedraw = false;
        private static bool mapRedraw = false;
        private static _gameStates _currentState;
        private Logger _logger;

        public static void Main()
        {
            //this must be the exact name of the bitmap font file we are using
            var fontFileName = "terminal8x8.png";
            //this title will appear at the top of the console window
            var consoleTitle = "Roguesharp Tutorial - Level 1";
            _rootConsole = new RLRootConsole(fontFileName, _screenWidth, _screenHeight, 8, 8, 1f, consoleTitle);
            
            player = new Player();
            MapGenerator mapGenerator = new MapGenerator(_mapWidth, _mapHeight);
            DungeonMap = mapGenerator.CreateMap();
            DungeonMap.updatePlayerFieldOfView();

            
            
            _mapConsole = new RLConsole( _mapWidth, _mapHeight );
            _messageConsole = new RLConsole( _messageWidth, _messageHeight );
            _statConsole = new RLConsole( _statWidth, _statHeight );
            _inventoryConsole = new RLConsole( _inventoryWidth, _inventoryHeight );
            // Initialize the sub consoles that we will Blit to the root console
            _mapConsole = new RLConsole(_mapWidth, _mapHeight);
            _messageConsole = new RLConsole(_messageWidth, _messageHeight);
            //set up a handler for the update event
            _rootConsole.Update += OnRootConsoleUpdate;
            _rootConsole.Render += OnRootConsoleRender;

            _currentState = _gameStates.MainMenu;
            renderRequired = true;
            
            _rootConsole.Run();
        }

        public static Player player
        {
            get;
            private set;
        }

        public static DungeonMap DungeonMap
        {
            get;
            private set;
        }

        static void InitConsole()
        {
            Console console = new Console(200, 140);
            // console.FillWithRandomGarbage();
        }

        private static void OnRootConsoleUpdate(object sender, UpdateEventArgs e)
        {
            bool didUnitAct = false;
            bool didMenuUpdate = false;
            RLKeyPress keyPress = _rootConsole.Keyboard.GetKeyPress();
            
            if (_currentState == _gameStates.MainMenu)
            {
                _mapConsole.SetBackColor(0, 0, _mapWidth, _mapHeight, RLColor.Black);
                _mapConsole.Print(1, 1, "Press G to start a new game", Colors.TextHeading);
                renderRequired = true;

                if (keyPress != null)
                {
                    if (keyPress.Key == RLKey.G)
                    {
                        _currentState = _gameStates.InGame;
                        //on a state change we need to redraw the interface
                        guiRedraw = true;
                    }
                }

                //draw menu
            } else if (_currentState == _gameStates.InGame)
            {
                // Set background color and text for each console 
                // so that we can verify they are in the correct positions
                
                //draw gui if not drawn
                if (guiRedraw)
                {
                    _mapConsole.Clear();
                    _mapConsole.SetBackColor( 0, 0, _mapWidth, _mapHeight, Colors.FloorBackground );
                    _mapConsole.Print(1, 1, "Map", Colors.TextHeading);
                    mapRedraw = true;
 
                    _messageConsole.SetBackColor( 0, 0, _messageWidth, _messageHeight, Swatch.DbDeepWater );
                    _messageConsole.Print( 1, 1, "Messages", Colors.TextHeading );
 
                    _statConsole.SetBackColor( 0, 0, _statWidth, _statHeight, Swatch.DbOldStone );
                    _statConsole.Print( 1, 1, "Stats", Colors.TextHeading );
 
                    _inventoryConsole.SetBackColor( 0, 0, _inventoryWidth, _inventoryHeight, Swatch.DbWood );
                    _inventoryConsole.Print( 1, 1, "Inventory", Colors.TextHeading );
                    
                    renderRequired = true;
                    guiRedraw = false;
                }

                if (mapRedraw)
                {
                    DungeonMap.Draw(_mapConsole);
                    player.Draw(_mapConsole, DungeonMap);
                    renderRequired = true;
                    mapRedraw = false;
                }
            }

        }

        private static void OnRootConsoleRender(object sender, UpdateEventArgs e)
        {

            if (renderRequired)
            {
                // Blit the sub consoles to the root console in the correct locations
                RLConsole.Blit( _mapConsole, 0, 0, _mapWidth, _mapHeight, 
                    _rootConsole, 0, _inventoryHeight );
                RLConsole.Blit( _statConsole, 0, 0, _statWidth, _statHeight, 
                    _rootConsole, _mapWidth, 0 );
                RLConsole.Blit( _messageConsole, 0, 0, _messageWidth, _messageHeight, 
                    _rootConsole, 0, _screenHeight - _messageHeight );
                RLConsole.Blit( _inventoryConsole, 0, 0, _inventoryWidth, _inventoryHeight, 
                    _rootConsole, 0, 0 );

                // Tell RLNET to draw the console that we set
                _rootConsole.Draw();

                renderRequired = false;
            }
        }
    }
}
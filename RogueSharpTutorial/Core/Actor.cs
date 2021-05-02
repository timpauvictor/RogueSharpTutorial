using RLNET;
using RogueSharp;
using SadConsoleGame.Interfaces;

namespace RogueSharpTutorial.Core
{
    public class Actor: IActor, IDrawable
    {
        //IActor
        public string Name { get; set; }
        public int Awareness { get; set; }
        
        //IDrawable
        public RLColor Color { get; set; }
        public char Symbol { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public void Draw(RLConsole in_console, DungeonMap in_map)
        {
            //don't draw actors in cells that haven't been explored
            if (!in_map.GetCell(X, Y).IsExplored)
            {
                return;
            }
            
            //only draw the actor when in fov
            if (in_map.IsInFov(X, Y))
            {
                in_console.Set(X, Y, Color, Colors.FloorBackgroundFov, Symbol);
            }
            else
            {
                in_console.Set(X, Y, Color, Colors.FloorBackgroundFov, '.');
            }
        }
    }
}
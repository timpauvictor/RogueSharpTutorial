using RLNET;
using RogueSharp;
using RogueSharpTutorial.Interfaces;

namespace RogueSharpTutorial.Core;

public class Actor: IActor, IDrawable
{
    public string Name { get; set; }
    public int Awareness { get; set; }
    public RLColor Color { get; set; }
    public char Symbol { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public void Draw(RLConsole console, IMap map)
    {
        //Don't draw actors in unexplored cells
        if (!map.GetCell(X, Y).IsExplored)
        {
            return; //do nothing
        }
        
        //only draw the actor with the color and symbol when they are in FOV
        if (map.IsInFov(X, Y))
        {
            console.Set(X, Y, Color, Colors.FloorBackgroundFov, Symbol);
        }
        else
        {
            console.Set(X, Y, Colors.Floor, Colors.FloorBackground, '.');
        }
    }
}
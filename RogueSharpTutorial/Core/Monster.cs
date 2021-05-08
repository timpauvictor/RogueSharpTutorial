using System;
using RLNET;

namespace RogueSharpTutorial.Core
{
    public class Monster : Actor
    {
        public void DrawStats(RLConsole statConsole, int position)
        {
            //start at y=13 which is below the below stats
            //multiply the position by 2 to leave a space between each stat
            int yPosition = 13 + (position * 2);
            
            //begin the line by printing the symbol of the monster in the appropriate colour
            statConsole.Print(1, yPosition, Symbol.ToString(), ActorColor);
            
            //figure out the width of the health bar by dividing current health by max health
            int width = Convert.ToInt32((double) Health / (double) MaxHealth * 16.0);
            int remainingWidth = 16 - width;
            
            //set the background colors of the health bar to show how damage the monster is
            statConsole.SetBackColor(3, yPosition, width, 1, Swatch.PrimaryLightest);
            statConsole.SetBackColor(3 + width, yPosition, remainingWidth, 1, Swatch.PrimaryDarkest);
            
            //print the monsters name over top of the health bar
            statConsole.Print(2, yPosition, $": {Name}", Swatch.DbLight);
        }
    }
}
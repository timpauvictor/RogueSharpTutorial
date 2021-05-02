﻿namespace RogueSharpTutorial.Core
{
    public class Player : Actor
    {
        public Player()
        {
            Awareness = 15;
            Name = "Rogue";
            Color = Colors.Player;
            Symbol = '@';
            X = 10;
            Y = 10;
        }
    }
}
using RLNET;

namespace RogueSharpTutorial.Core;

public class Player: Actor
{
    public Player()
    {
        Awareness = 15;
        Name = "Rogue";
        Color = Colors.Player;
        Symbol = '@';
        X = 10;
        Y = 10;

        Attack = 2;
        AttackChance = 50;
        Awareness = 15;
        Defense = 2;
        DefenseChance = 50;
        Gold = 100;
        Health = 100;
        MaxHealth = 100;
        Speed = 10;
    }
    
    public void DrawStats(RLConsole statConsole)
    {
        statConsole.Print(1, 1, $"Name: {Name}", Colors.TextHeading);
        statConsole.Print(1, 3, $"Health: {Health}/{MaxHealth}", Colors.TextHeading);
        statConsole.Print(1, 4, $"Attack: {Attack}", Colors.TextHeading);
        statConsole.Print(1, 5, $"Defense: {Defense}", Colors.TextHeading);
        statConsole.Print(1, 6, $"Gold: {Gold}", Colors.TextHeading);
    }
}
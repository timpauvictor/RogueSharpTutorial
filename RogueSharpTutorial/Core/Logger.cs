using System;

namespace RogueSharpTutorial.Core
{
    public class Logger
    {
        public void info(string in_message)
        {
            Console.WriteLine("INFO: " + in_message);
        }

        public void debug(string in_message)
        {
            Console.WriteLine("DEBUG: " + in_message);
        }
    }
}
using System.Collections.Generic;
using RLNET;

namespace SadConsoleGame.Systems
{
    //represents a queue of messages that can be added to
    public class MessageLog
    {
        private static readonly int _maxlines = 9;

        private readonly Queue<string> _lines;

        public MessageLog()
        {
            _lines = new Queue<string>();
        }
        
        //add a line to the message log queue
        public void Add(string message)
        {
            _lines.Enqueue(message);
            
            //when exceeding the maximum number of lines, remove the last one
            if (_lines.Count > _maxlines)
            {
                _lines.Dequeue();
            }
        }
        
        //draw each line of the queue to the console
        public void Draw(RLConsole console)
        {
            console.Clear();
            string[] lines = _lines.ToArray();
            for (int i = 0; i < lines.Length; i++)
            {
                console.Print(1, i + 1, lines[i], RLColor.White);
            }
        }
    }
}
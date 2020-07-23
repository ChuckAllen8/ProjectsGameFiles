using System;
using System.Collections.Generic;
using System.Text;

namespace HangMan
{
    class ConsoleManager
    {
        private int consoleHeight, consoleWidth;
        private char[,] consoleBuffer;
        public ConsoleManager()
        {
            consoleBuffer = new char[0,0];
        }

        public void ConsoleResize(int height, int width)
        {
            consoleBuffer = new char[height, width];
            consoleHeight = height;
            consoleWidth = width;
            Console.SetWindowSize(width+2, height+2);
        }

        public string GetInput()
        {
            return Console.ReadLine();
        }

        public void Print(string output)
        {
            Console.CursorVisible = false;
            Console.WriteLine(output);
        }

        public void ConsoleReset()
        {
            Console.Clear();
            Console.CursorVisible = false;
            consoleBuffer = new char[consoleHeight, consoleWidth];
        }

        public void SetInputPosition()
        {
            Console.SetCursorPosition(0, consoleHeight);
            Console.Write(new string(' ', consoleWidth));
            Console.SetCursorPosition(0, consoleHeight);
        }

        public void ConsoleUpdate(char[,] display)
        {
            Console.CursorVisible = false;
            for (int i = 0; i < consoleHeight; i++)
            {
                for(int j = 0; j < consoleWidth; j++)
                {
                    try
                    {
                        if (consoleBuffer[i, j] != display[i, j])
                        {
                            Console.SetCursorPosition(j, i);
                            Console.Write(display[i, j]);
                            consoleBuffer[i, j] = display[i, j];
                        }
                    }
                    catch
                    {
                        Console.SetCursorPosition(j, i);
                        Console.Write(' ');
                    }
                }
            }
            SetInputPosition();
        }
    }
}

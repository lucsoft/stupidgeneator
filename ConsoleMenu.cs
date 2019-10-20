using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ConsoleApp1
{
    internal class ConsoleMenu
    {

        private string title = "No Title";
        private string padleft = "    ";
        private string padTop = "\n\n";
        public int maxWidth = 29;
        private int selectedIndex = 0;
        private List<ConsoleEntry> index = new List<ConsoleEntry>();
        private Thread ConsoleThread;
        private bool Update = false;
        public ConsoleMenu()
        {
        }

        public void AddElement(string v, Action p)
        {
            index.Add(new ConsoleEntry() { text = v, action = p });
        }

        public void SetTitle(string v)
        {
            title = v;
        }

        public void Start()
        {
            Console.Title = title;
            Update = true;
            ConsoleThread = new Thread(() => {
                while(Update)
                {
                    var key = Console.ReadKey().Key;
                    if (key == ConsoleKey.DownArrow)
                    {
                        if (selectedIndex == index.Count - 1)
                            selectedIndex = 0;
                        else
                            selectedIndex++;
                    } else if(key == ConsoleKey.UpArrow)
                    {
                        if (selectedIndex == 0)
                            selectedIndex = index.Count - 1;
                        else
                            selectedIndex--;
                    }
                    if (key == ConsoleKey.Enter)
                    {
                        index[selectedIndex].action();
                    } else
                    {
                        render();
                    }
                }
            });

            render();
            ConsoleThread.Start();
            
        }

        public void Stop()
        {
            Update = false;
            ConsoleThread = null;
            Console.Clear();
        }
        private void render()
        {
            Console.SetCursorPosition(0, 0);
            Console.Write(padTop);
            Console.Write(padleft + "╔");
            for (int i = 0; i < maxWidth; i++)
            {
                Console.Write("═");
            }
            Console.WriteLine("╗");
            Console.WriteLine(padleft + "║".PadRight((maxWidth / 2) - (title.Length / 2) + 1) + title + "║".PadLeft((maxWidth / 2) - (title.Length / 2) + 1));

            Console.Write(padleft + "╠");
            for (int i = 0; i < maxWidth; i++)
            {
                Console.Write("═");
            }
            Console.WriteLine("╣");
            for (int i = 0; i < index.Count; i++)
            {
                var item = index[i];
                if (i == selectedIndex)
                {
                    Console.Write(padleft + "║ » ");
                }
                else
                {
                    Console.Write(padleft + "║   ");
                }
                Console.Write(item.text);
                for (int e = 0; e < maxWidth - item.text.Length - 3; e++)
                {
                    Console.Write(" ");
                }
                Console.WriteLine("║");
            }

            Console.Write(padleft + "╚");
            for (int i = 0; i < maxWidth; i++)
            {
                Console.Write("═");
            }
            Console.WriteLine("╝");
            Console.CursorVisible = false;
        }
    }

    internal class ConsoleEntry
    {
        public string text { get; internal set; }
        public Action action { get; internal set; }
    }
}
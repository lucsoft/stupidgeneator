using System;
using System.Diagnostics;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleMenu cmenu = new ConsoleMenu();
            TextGen tgen = new TextGen();
            if (!Directory.Exists("exports"))
                Directory.CreateDirectory("exports");
            if (!Directory.Exists("imports"))
                Directory.CreateDirectory("imports");

            
            cmenu.SetTitle("Dump Word Gen");
            cmenu.AddElement("Compile...", () => {
                cmenu.Stop();
                ConsoleMenu compie = new ConsoleMenu();
                compie.SetTitle("Select text to Compile");
                compie.AddElement("Dicker (Anrede) [Wikipedia]", () =>
                {
                    
                    File.WriteAllText("exports/dicker.json", tgen.Compile(Properties.Resources.wiki_digga));

                    compie.Stop();
                    cmenu.Start();
                });
                foreach (var item in Directory.GetFiles("imports", "*.txt"))
                {
                    compie.AddElement(item.Replace(@"imports\", "").Replace(".txt", "") + " [Import]", () =>
                    {
                        File.WriteAllText(item.Replace("imports", "exports").Replace(".txt", ".json"), tgen.Compile(File.ReadAllText(item)));
                        compie.Stop();
                        cmenu.Start();
                    });
                }
                compie.AddElement("Go Back", () =>
                {
                    compie.Stop();
                    cmenu.Start();
                });
                compie.maxWidth = 40;
                
                compie.AddElement("Exit", () =>
                {
                    Environment.Exit(0);
                });

                compie.Start();
            });

            cmenu.AddElement("Generate...", () => {
                cmenu.Stop();

                ConsoleMenu compie = new ConsoleMenu();
                compie.SetTitle("Select File to Generate text from");

                foreach (var item in Directory.GetFiles("exports", "*.json"))
                {
                    compie.AddElement(item.Replace(@"exports\", "").Replace(".txt", "") + " [Export]", () =>
                    {
                        Console.WriteLine(tgen.Generate(File.ReadAllText(item), 5));
                        
                    });
                }
                compie.AddElement("Go Back", () =>
                {
                    compie.Stop();
                    cmenu.Start();
                });
                compie.maxWidth = 41;

                compie.AddElement("Exit", () =>
                {
                    Environment.Exit(0);
                });
                compie.Start();
            });
            cmenu.AddElement("Open Exports", () =>
            {
                Process.Start("explorer", @".\exports");
            });
            cmenu.AddElement("Open Imports", () =>
            {
                Process.Start("explorer", @".\imports");
            });
            
            cmenu.AddElement("Exit", () => {
                Environment.Exit(0);
            });
            cmenu.Start();
        }
    }
}

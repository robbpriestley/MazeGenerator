using System;
using System.Text;
using DigitalWizardry.Maze;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Maze maze = new Maze(25);
			StringBuilder output = new StringBuilder();
			output.AppendLine(maze.VisualizeAsText());
			output.AppendLine(maze.BuildStats() + Environment.NewLine);
			Console.WriteLine(output);
        }
    }
}

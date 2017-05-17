using System;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DigitalWizardry.MazeGenerator
{
	[Route("maze")]
	public class MazeGeneratorApiController : Controller
	{
		public Secrets Secrets { get; set; }
		
		public MazeGeneratorApiController(IOptions<Secrets> secrets)
		{
			Secrets = secrets.Value;
		}

		[HttpGet]
		[Route("generate")]
		public IActionResult GenerateMaze()
		{
			if (!BasicAuthentication.Authenticate(Secrets, Request))
			{
				return new UnauthorizedResult();
			}
			
			Maze maze = new Maze(15, 15);
			StringBuilder output = new StringBuilder();
			output.AppendLine(maze.VisualizeAsText());
			output.AppendLine(maze.BuildStats() + Environment.NewLine);

			return new ObjectResult(output.ToString());
		}

		[HttpGet]
		[Route("test")]
		public IActionResult Test()
		{
			if (!BasicAuthentication.Authenticate(Secrets, Request))
			{
				return new UnauthorizedResult();
			}
			
			StringBuilder output = new StringBuilder();

			for (int i = 0; i < 100; i++)
			{
				Maze maze = new Maze(15, 15);
				output.AppendLine(maze.VisualizeAsText());
				output.AppendLine(maze.BuildStats() + Environment.NewLine);
				Console.WriteLine("Iteration: " + i.ToString());
			}

			return new ObjectResult(output.ToString());
		}
	}
}
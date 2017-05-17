using Microsoft.AspNetCore.Mvc;

namespace DigitalWizardry.MazeGenerator
{
    public class IndexController : Controller
    {
        public IActionResult Index()
        {
    		return View();
        }

		public IActionResult MazeView()
		{
			Maze maze = new Maze(15, 15);
    		return Json(maze.MazeView);
		}
    }
}
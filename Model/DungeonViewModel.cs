namespace DigitalWizardry.MazeGenerator
{
	public class MazeViewModel
	{
		public int X { get; set; }
		public int Y { get; set; }
		public string CssName { get; set; }
		public string CssLocation { get; set; }

		public MazeViewModel(int x, int y)
		{
			X = x;
			Y = y;
		}
	}
}

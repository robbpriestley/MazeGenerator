namespace DigitalWizardry.Maze
{	
	public class Coords
	{	
		public int X { get; set; }
		public int Y { get; set; }
		public bool AdjacentEdgeUp { get; set; }
		public bool AdjacentEdgeDown { get; set; }
		public bool AdjacentEdgeLeft { get; set; }
		public bool AdjacentEdgeRight { get; set; }

		public Direction Direction;  // A direction is needed for convenience sometimes.

		public Coords(){}
		
		public Coords(int x, int y)
		{
			X = x;
			Y = y;
			Direction = Direction.NoDir;

			if (X == 0) 
			{
				AdjacentEdgeLeft = true;
			} 
			
			if (Y == 0) 
			{
				AdjacentEdgeDown = true;
			}
			
			if (X + 1 == Grid.Width) 
			{
				AdjacentEdgeRight = true;
			}
			
			if (Y + 1 == Grid.Height) 
			{
				AdjacentEdgeUp = true;
			}
		}

		// Copy constructor. Creates a deep copy clone of the source.
		public Coords(Coords source) : this()
		{		
			X = source.X;
			Y = source.Y;
			AdjacentEdgeUp = source.AdjacentEdgeUp;
			AdjacentEdgeDown = source.AdjacentEdgeDown;
			AdjacentEdgeLeft = source.AdjacentEdgeLeft;
			AdjacentEdgeRight = source.AdjacentEdgeRight;
		}

		bool IsOppositeDirTo(Coords otherCoords)
		{
			return
			
			(Direction == Direction.Up && otherCoords.Direction == Direction.Down) ||
			(Direction == Direction.Down && otherCoords.Direction == Direction.Up) ||
			(Direction == Direction.Left && otherCoords.Direction == Direction.Right) ||
			(Direction == Direction.Right && otherCoords.Direction == Direction.Left) ||
			
			(Direction == Direction.UpLeft && otherCoords.Direction == Direction.DownRight) ||
			(Direction == Direction.DownLeft && otherCoords.Direction == Direction.UpRight) ||
			(Direction == Direction.UpRight && otherCoords.Direction == Direction.DownLeft) ||
			(Direction == Direction.DownRight && otherCoords.Direction == Direction.DownLeft);
		}
	}
}
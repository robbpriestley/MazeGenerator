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
		
		public Coords(int x, int y, int size)
		{
			this.X = x;
			this.Y = y;
			this.Direction = Direction.NoDir;

			if (this.X == 0) 
			{
				this.AdjacentEdgeLeft = true;
			} 
			
			if (this.Y == 0) 
			{
				this.AdjacentEdgeDown = true;
			}
			
			if (this.X + 1 == size) 
			{
				this.AdjacentEdgeRight = true;
			}
			
			if (this.Y + 1 == size) 
			{
				this.AdjacentEdgeUp = true;
			}
		}

		// Copy constructor. Creates a deep copy clone of the source.
		public Coords(Coords source) : this()
		{		
			this.X = source.X;
			this.Y = source.Y;
			this.AdjacentEdgeUp = source.AdjacentEdgeUp;
			this.AdjacentEdgeDown = source.AdjacentEdgeDown;
			this.AdjacentEdgeLeft = source.AdjacentEdgeLeft;
			this.AdjacentEdgeRight = source.AdjacentEdgeRight;
		}

		bool IsOppositeDirTo(Coords otherCoords)
		{
			return
			
			(this.Direction == Direction.Up && otherCoords.Direction == Direction.Down) ||
			(this.Direction == Direction.Down && otherCoords.Direction == Direction.Up) ||
			(this.Direction == Direction.Left && otherCoords.Direction == Direction.Right) ||
			(this.Direction == Direction.Right && otherCoords.Direction == Direction.Left) ||
			
			(this.Direction == Direction.UpLeft && otherCoords.Direction == Direction.DownRight) ||
			(this.Direction == Direction.DownLeft && otherCoords.Direction == Direction.UpRight) ||
			(this.Direction == Direction.UpRight && otherCoords.Direction == Direction.DownLeft) ||
			(this.Direction == Direction.DownRight && otherCoords.Direction == Direction.DownLeft);
		}
	}
}
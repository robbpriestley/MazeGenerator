namespace DigitalWizardry.MazeGenerator
{	
	public class Cell
	{
		public int X { get; set; }
		public int Y { get; set; }
		public int Sequence { get; set; }             // Sequence number used when solving the maze.
		public bool Visited { get; set; }             // For use when traversing the maze.
		public bool AttachBlocked { get; set; }        // There are frequent cases where the cell has an available connection point, but nothing can be attached there. In those cases, attachBlocked is set to true.
		public int AvailableConnections { get; set; }  // Records number of available connection points;
		public CellType Type { get; set; }
		
		public Cell(){}
		
		public Cell(int x, int y, CellType type)
		{
				X = x;
				Y = y;
				Type = type;
				AvailableConnections = Type.InitialAvailableConnections;
		}

		// Copy constructor. Creates a deep copy clone of the source.
		public Cell(Cell source) : this()
		{
			X = source.X;
			Y = source.Y;
			Sequence = source.Sequence;
			Visited = source.Visited;
			AttachBlocked = source.AttachBlocked;
			AvailableConnections = source.AvailableConnections;
			Type = source.Type;    // This does not require deep copy.
		}
	}
}
namespace DigitalWizardry.Maze
{	
	public class Cell
	{
		public int X;
		public int Y;
		public bool HasKey;
		public int Sequence;             // Sequence number used when solving the maze.
		public bool Visited;             // For use when traversing the maze.
		public bool ExitImpossible;
		public Coords SourceCoords;       // For use when solving shortest-path.
		public bool AttachBlocked;        // There are frequent cases where the cell has an available connection point, but nothing can be attached there. In those cases, attachBlocked is set to true.
		public int AvailableConnections;  // Records number of available connection points;
		public int DescrWeight;           // Essentially a percentage, used to determine how "sticky" the description is.
		public Type Type;
		
		public Cell(){}
		
		public Cell(int x, int y, Type type)
		{
				this.X = x;
				this.Y = y;
				this.Type = type;
				this.AvailableConnections = Type.InitialAvailableConnections;
		}

		// Copy constructor. Creates a deep copy clone of the source.
		public Cell(Cell source) : this()
		{
			this.X = source.X;
			this.Y = source.Y;
			this.HasKey = source.HasKey;
			this.Sequence = source.Sequence;
			this.Visited = source.Visited;
			this.ExitImpossible = source.ExitImpossible;
			this.SourceCoords = source.SourceCoords == null ? null : new Coords(source.SourceCoords);
			this.AttachBlocked = source.AttachBlocked;
			this.AvailableConnections = source.AvailableConnections;
			this.DescrWeight = source.DescrWeight;
			this.Type = source.Type;    // This does not require deep copy.
		}
	}
}
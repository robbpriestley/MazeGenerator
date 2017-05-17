using System;
using System.Text;
using System.Collections.Generic;

namespace DigitalWizardry.MazeGenerator
{	
	public enum Direction
	{
		Up, Down, Left, Right, UpLeft, UpRight, DownLeft, DownRight, NoDir
	}

	public static class Grid
	{
		public static int Width { get; set; }
		public static int Height { get; set; }
	}
	
	public class Maze
	{
		private int _width;                       // Width of the grid (X dimension).
		private int _height;                      // Height of the grid (Y dimension).
		private Cell[,] _grid;                    // Master grid data structure, a 2-D array of cells.
		private Random _r;                        // Re-usable random number generator.
		private Cell _emptyCell;                  // This empty cell instance is re-used everywhere. It exists outside of "normal space" because its coords are -1,-1.
		private Coords _startCoords;               // Coords where the first cell is added to the maze grid.
		private int _sequenceNumber;               // Used when solving the maze to "stamp" cells with a sequence number.
		private int _iterations;                   // Number of discarded attempts before arriving at a completed maze.
		private TimeSpan _elapsedTime;             // How long in total did it take to build this maze?

		public Maze(int width, int height)
		{			
			_width = width;
			_height = height;
			Grid.Width = width;
			Grid.Height = height;

			_r = new Random();
			_emptyCell = new Cell(-1, -1, CellTypes.EmptyCell);

			Start();
		}

		private void Start()
		{
			bool mazeComplete = false;
			DateTime start = DateTime.Now;

			do
			{
				try 
				{
					_iterations++;
					
					Initialize();
					GenerateMaze();
					MazeSolve();
					mazeComplete = true;  // i.e. No exceptions...
				}
				catch (MazeGenerateException) 
				{
					mazeComplete = false;  // Try again.
				}

			} while (!mazeComplete);

			_elapsedTime = DateTime.Now - start;
		}

		private void Initialize()
		{
			_grid = new Cell[_width, _height];

			// Fill in each cell with the "empty cell" object.
			for (int y = 0; y < _height; y++)
			{
				for (int x = 0; x < _width; x++)
				{
					_grid[x, y] = _emptyCell;
				}
			}

			_startCoords = new Coords(0, 0);
			List<CellType> types = CellTypes.GetTypes(_startCoords);
 			CellType newType = RandomCellType(types);
 			Cell newCell = new Cell(_startCoords.X, _startCoords.Y, newType);
 			SetCellValue(_startCoords.X, _startCoords.Y, newCell);
		}

		private void GenerateMaze()
		{    
			Cell cell;
			bool modified = false;
			
			// As long as the maze is not considered complete, keep adding cells to it.
			do 
			{
				modified = false;
				
				for (int y = 0; y < _height; y++) 
				{
					for (int x = 0; x < _width; x++) 
					{
						cell = _grid[x, y];
						
						if (!cell.Type.IsEmpty && !cell.AttachBlocked && cell.AvailableConnections > 0)
						{
							// Attach a new random cell to current cell, if possible. If the cell has 
							// available connections but nothing can be added to it, consider it blocked.
							if (AttachNewCell(cell)) 
							{
								modified = true;
							}
							else  
							{
								cell.AttachBlocked = true;
							}
						}
					}
				}
			} while (!CompleteCheck(modified));
		}

		private bool AttachNewCell(Cell cell)
		{
			bool attachSuccessful = false;
			Coords coords = RandomAttachCoords(cell);
			
			if (coords != null)
			{
				// Get a disposable array of constructed corridor cell types.
				List<CellType> types = CellTypes.GetTypes(coords);
				
				// Choose a new cell type to attach.
				Cell newCell = null;
				CellType newType = null;
				
				while (newCell == null) 
				{
					if (types.Count == 0) 
					{
						return false;  // There are no more possibilities.
					}
					
					newType = RandomCellType(types);
				
					// The new cell needs to be compatible with each adjacent cell.
					if (TypeCompatibleWithAdjacentCells(newType, coords))
					{
						newCell = new Cell(coords.X, coords.Y, newType);
					}
				}
				
				SetCellValue(coords.X, coords.Y, newCell);
				attachSuccessful = true;
			}
			
			return attachSuccessful;
		}

		// When a new cell is placed in the maze, "record" it as such by decrementing the 
		// availableConnections count of each adjacent, non-empty cell that connects to it.
		// Also, decrement the availableConnections count of the new cell accordingly.
		private void RecordNewAttachment(Cell cell)
		{
			Cell cellUp, cellDown, cellLeft, cellRight;
			
			if (cell.Y + 1 < _height)
			{
				cellUp = _grid[cell.X, cell.Y + 1];

				if (cell.Type.ConnectsTo(cellUp.Type, Direction.Up))
				{
					cell.AvailableConnections--;
					cellUp.AvailableConnections--;
				}
			}
			
			if (cell.Y - 1 >= 0)
			{
				cellDown = _grid[cell.X, cell.Y - 1];

				if (cell.Type.ConnectsTo(cellDown.Type, Direction.Down))
				{
					cell.AvailableConnections--;
					cellDown.AvailableConnections--;
				}
			}
			
			if (cell.X - 1 >= 0)
			{
				cellLeft = _grid[cell.X - 1, cell.Y];

				if (cell.Type.ConnectsTo(cellLeft.Type, Direction.Left))
				{
					cell.AvailableConnections--;
					cellLeft.AvailableConnections--;
				}
			}
			
			if (cell.X + 1 < _width)
			{
				cellRight = _grid[cell.X + 1, cell.Y];

				if (cell.Type.ConnectsTo(cellRight.Type, Direction.Right))
				{
					cell.AvailableConnections--;
					cellRight.AvailableConnections--;
				}
			}
		}

		#region Randomization

		// Find which locations adjacent to the current cell could be populated with a new attaching cell. 
		// Then, from those locations, select one at random and return the coords for it. If no such 
		// location exists, return nil.
		private Coords RandomAttachCoords(Cell cell)
		{
			List<Coords> coordPotentials = new List<Coords>();
			
			// Check each direction for an adjacent cell. Obviously, an adjacent cell has to be within the 
			// coordinate bounds of the maze. If the current cell has the capability to join with a cell
			// in the adjacent location, and the cell in the adjacent location is empty, add the coords for 
			// each such location to the coordPotentials array. Then choose one of those potential coords at 
			// random and return it. If there are no potentials at all, return null.
			
			// Cell above.
			if (cell.Type.ConnectsUp && cell.Y + 1 < _height)
			{
				if (_grid[cell.X, cell.Y + 1].Type.IsEmpty)
				{
					coordPotentials.Add(new Coords(cell.X, cell.Y + 1));
				}
			}
			
			// Cell below.
			if (cell.Type.ConnectsDown && cell.Y - 1 >= 0)
			{
				if (_grid[cell.X, cell.Y - 1].Type.IsEmpty)
				{
					coordPotentials.Add(new Coords(cell.X, cell.Y - 1));
				}
			}
		
			// Cell left.
			if (cell.Type.ConnectsLeft && cell.X - 1 >= 0)
			{
				if (_grid[cell.X - 1, cell.Y].Type.IsEmpty)
				{
					coordPotentials.Add(new Coords(cell.X - 1, cell.Y));
				}
			}
			
			// Cell right.
			if (cell.Type.ConnectsRight && cell.X + 1 < _width)
			{
				if (_grid[cell.X + 1, cell.Y].Type.IsEmpty)
				{
					coordPotentials.Add(new Coords(cell.X + 1, cell.Y));
				}
			}
		
			if (coordPotentials.Count == 0)
			{
				return null;
			}
			else
			{
				int randomIndex = _r.Next(coordPotentials.Count);
				return coordPotentials[randomIndex];
			}
		}

		private CellType RandomCellType(List<CellType> types)
		{
			// Pick a cell type randomly, and also eliminate it as a candidate for the current cell to avoid 
			// re-testing it in the future if it is rejected. Types have weights, so some are more likely to 
			// be picked than others.
			
			int total = 0;
			
			foreach (CellType type in types)
			{
				total += type.Weight;
			}
			
			int threshold = _r.Next(total);
			
			CellType selected = null;

			foreach (CellType type in types)
			{
				selected = type;
				threshold -= type.Weight;

				if (threshold < 0)
				{
					types.Remove(type);
					break;
				}
			}
			
			return selected;
		}

		private Cell RandomForceGrowthCell(List<Cell> forceGrowthCells)
		{
			// Pick a cell randomly, and also eliminate it as a future candidate...
			Cell cell = forceGrowthCells[_r.Next(forceGrowthCells.Count)];
			forceGrowthCells.Remove(cell);
			return cell;
		}
			
		#endregion

		#region Checks

		// With "coords" representing the new, empty maze location, check that each of the adjacent cells 
		// is compatible with the proposed (randomly determined) new cell type.
		private bool TypeCompatibleWithAdjacentCells(CellType newCellType, Coords coords)
		{
			// This is an innocent-until-proven guilty scenario. However, if any of the cells is proven to be
			// incompatible, that's enough to eliminate it as a prospect.
			
			Cell cellUp, cellDown, cellLeft, cellRight;
			
			if (coords.Y + 1 < _height)
			{
				cellUp = _grid[coords.X, coords.Y + 1];
				if (!newCellType.CompatibleWith(cellUp.Type, Direction.Up))
				{
					return false;
				}
			}
			
			if (coords.Y - 1 >= 0)
			{
				cellDown = _grid[coords.X, coords.Y - 1];
				if (!newCellType.CompatibleWith(cellDown.Type, Direction.Down))
				{
					return false;
				}
			}
			
			if (coords.X - 1 >= 0)
			{
				cellLeft = _grid[coords.X - 1, coords.Y];
				if (!newCellType.CompatibleWith(cellLeft.Type, Direction.Left))
				{
					return false;
				}
			}
			
			if (coords.X + 1 < _width)
			{
				cellRight = _grid[coords.X + 1, coords.Y];
				if (!newCellType.CompatibleWith(cellRight.Type, Direction.Right))
				{
					return false;
				}
			}
			
			return true;
		}

		// If the maze was modified on the last pass, it cannot yet be considered complete. If it was not 
		// modified on the last pass, check to see if the maze is filled to completion. If it is not, modify 
		// a cell to allow the maze to grow some more.
		bool CompleteCheck(bool modified)
		{
			bool complete = false;
			
			if (!modified)
			{
				int percentFilled = CalcPercentFilled();
				
				if (percentFilled >= 100)
				{
					complete = true;
				}
				else
				{
					if (!ForceGrowth())  // Try to modify random cells to allow more growth.
					{
						throw new MazeGenerateException();
					}
				}
			}

			return complete;
		}
			
		#endregion
		#region Accessors

		private void SetCellValue(int x, int y, Cell cell)
		{
			_grid[x, y] = cell;
			RecordNewAttachment(cell);
		}

		// Returns a random cell from the maze. If empty == true, then the random cell will be empty. 
		// If empty == false, then the random cell will be occupied.
		private Coords RandomCell(bool empty)
		{
			Coords coords = null;
			
			while (coords == null) 
			{
				int x = _r.Next(_width);
				int y = _r.Next(_height);
				
				Cell cell = _grid[x, y];
				
				if (empty)  // Cell must be occupied.
				{
					if (cell.Type.IsEmpty) 
					{
						coords = new Coords(x, y);
					}
				}
				else  // Cell must be occupied.
				{
					if (!cell.Type.IsEmpty) 
					{
						coords = new Coords(x, y);
					}
				}
			}
			
			return coords;
		}
			
		#endregion
		#region Force Growth
			
		// Replace a random, non-empty, compatible cell with a different cell to see if that makes the
		// maze grow any bigger. But, don't waste any cycles doing it, it could be a lost cause...
		private bool ForceGrowth()
		{
			bool success = false;
			bool typeMatch = false;
			
			CellType newType = null;
			List<Cell> cells = ForceGrowthCells();
			
			while (!success && cells.Count > 0) 
			{
				Cell cell = RandomForceGrowthCell(cells);
				Coords coords = new Coords(cell.X, cell.Y);
				
				// Attempt to replace it from the standard types.
				List<CellType> types = CellTypes.GetTypes(coords);
				types.Remove(cell.Type);  // ...but replace it with something different.
				
				while (!typeMatch) 
				{
					if (types.Count == 0) 
					{
						break;  // If nothing replaces it, start over.
					}
					
					newType = RandomCellType(types);  // Candidate new cell type.
					
					// The new cell needs to be compatible with each adjacent cell.
					if (TypeCompatibleWithAdjacentCells(newType, coords))
					{
						// It is? Cool.
						typeMatch = true;
					}
				}
				
				if (typeMatch) 
				{
					// Now set the new cell.
					Cell newCell = new Cell(coords.X, coords.Y, newType);
					SetCellValue(coords.X, coords.Y, newCell);
					success = true;
				}
			}

			return success;
		}

		// Gets all the cells in the maze that are not empty, and forceGrowthCompatible.
		private List<Cell> ForceGrowthCells()
		{
			List<Cell> cells = new List<Cell>();
			
			foreach (Cell cell in _grid)
			{
				if (cell.Type.ForceGrowthCompatible && !cell.Type.IsEmpty) 
				{
					cells.Add(cell);
				}
			}
			
			return cells;
		}

		#endregion

		#region Solve
		
		// Ensure that every cell in the maze is "reachable". If not, start over.
		private void MazeSolve()
		{
			_sequenceNumber = 0;
			
			Solve(_grid[_startCoords.X, _startCoords.Y]);
			
			for (int y = 0; y < _height; y++) 
			{
				for (int x = 0; x < _width; x++)
				{
					if (!_grid[x, y].Visited)
					{
						throw new MazeGenerateException();
					}
				}
			}
		}

		// Recursive solve algorithm.
		private void Solve(Cell cell)
		{
			_sequenceNumber++;

			cell.Visited = true;
			cell.Sequence = _sequenceNumber;
			
			// Cell above.
			if (cell.Type.TraversableUp && cell.Y + 1 < _height)
			{
				Cell cellAbove = _grid[cell.X, cell.Y + 1];
				
				if (!cellAbove.Visited)
				{
					Solve(cellAbove);
				}
			}

			// Cell below.
			if (cell.Type.TraversableDown && cell.Y - 1 >= 0)
			{
				Cell cellBelow = _grid[cell.X, cell.Y - 1];
				
				if (!cellBelow.Visited)
				{
					Solve(cellBelow);
				}
			}
			
			// Cell left.
			if (cell.Type.TraversableLeft && cell.X - 1 >= 0)
			{
				Cell cellLeft = _grid[cell.X - 1, cell.Y];
				
				if (!cellLeft.Visited)
				{
					Solve(cellLeft);
				}
			}
			
			// Cell right.
			if (cell.Type.TraversableRight && cell.X + 1 < _width)
			{
				Cell cellRight = _grid[cell.X + 1, cell.Y];
				
				if (!cellRight.Visited)
				{
					Solve(cellRight);
				}
			}
		}

		#endregion
		#region Utility

		private int CalcPercentFilled()
		{
			int filledCellCount = 0;
			
			for (int x = 0; x < _width; x++)
			{
				for (int y = 0; y < _height; y++) 
				{
					if (!_grid[x, y].Type.IsEmpty)
					{
						filledCellCount++;
					}
				}
			}
			
			return (filledCellCount * 100) / (_width * _height);
		}
		
		public string VisualizeAsText()
		{
			int x;
			Cell cell;
			string padding;
			StringBuilder line, grid = new StringBuilder();
	
			// Because it is console printing, start with the "top" of the maze, and work down.
			for (int y = _height - 1; y >= 0; y--) 
			{
				line = new StringBuilder();
				
				for (x = 0; x < _width * 2; x++) 
				{
					cell = _grid[x / 2, y];
					line.Append(x % 2 == 0 ? cell.Type.TextRep : cell.Type.TextRep2);
				}
				
				padding = (y < 10) ? "0" : "";  // For co-ordinate printing.
				
				grid.AppendLine(padding + y + line.ToString());
			}
			
			// Now print X co-ordinate names at the bottom.
			
			grid.Append("  ");
			
			for (x = 0; x < _width * 2; x++) 
			{
				if (x % 2 == 0)
					grid.Append((x/2)/10);
				else
					grid.Append(" "); 
			}
			
			grid.Append("\n  ");
			
			for (x = 0; x < _width * 2; x++) 
			{
				if (x % 2 == 0)
					grid.Append((x/2) % 10);
				else
					grid.Append(" "); 
			}

			return grid.ToString();
		}

		public string BuildStats()
		{
			return Environment.NewLine +
			       "Iterations: " + _iterations.ToString() + Environment.NewLine +
				   "Elapsed Time: " + _elapsedTime.ToString();
		}

		#endregion
	}

	public class MazeGenerateException : Exception
	{
		public MazeGenerateException() : base() {}
	}
}
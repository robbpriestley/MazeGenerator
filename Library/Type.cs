using System.Collections.Generic;

namespace DigitalWizardry.Maze
{	
	public class Type
	{
		// *** BEGIN CONNECTS MEMBERS ***
		// Connects: determines if one cell is capable of being mated to another.
		public bool ConnectsUp { get; set; }
		public bool ConnectsDown { get; set; }
		public bool ConnectsLeft { get; set; }
		public bool ConnectsRight { get; set; }
		// *** END CONNECTS MEMBERS ***
		// *** BEGIN TRAVERSABLE MEMBERS ***
		// Traversable: determines if it would be possible to move from one cell to another.
		public bool TraversableUp { get; set; }
		public bool TraversableDown { get; set; }
		public bool TraversableLeft { get; set; }
		public bool TraversableRight { get; set; }
		// *** END TRAVERSABLE MEMBERS ***
		// *** BEGIN DESCRIPTIVE MEMBERS ***
		public bool IsEmpty { get; set; }
		public bool IsJunction { get; set; }
		public bool IsDeadEnd { get; set; }
		public bool ForceGrowthCompatible { get; set; }       // Used when a cell is substituted for another to increase maze fill.
		// *** END DESCRIPTIVE MEMBERS ***
		// *** BEGIN UTILITY MEMBERS ***
		public int Weight { get; set; }                       // Influences the selection when types are being randomly determined.
		public string TextRep { get; set; }                   // Primary character used to represent a type in text.
		public string TextRep2 { get; set; }                  // Used for better rendering of the text representation, which appears "squished" in the horizontal dimension.
		public int InitialAvailableConnections { get; set; }  // Used when generating the maze to determine if other cells can be attached to a target cell.
		// *** END UTILITY MEMBERS ***

		public Type()
		{
        	ForceGrowthCompatible = true;  // Attention! This is the only bool member which is initialized to true.
		}

		public bool ConnectsTo(Type otherCell, Direction direction)
		{    
			if (otherCell.IsEmpty)
			{
				return false;
			}
			else if (direction == Direction.Up && ConnectsUp && otherCell.ConnectsDown)
			{
				return true;
			}
			else if (direction == Direction.Down && ConnectsDown && otherCell.ConnectsUp)
			{
				return true;
			}
			else if (direction == Direction.Left && ConnectsLeft && otherCell.ConnectsRight)
			{
				return true;
			}
			else if (direction == Direction.Right && ConnectsRight && otherCell.ConnectsLeft)
			{
				return true;
			}
			
			return false;
		}

		public bool CompatibleWith(Type otherCell, Direction direction)
		{
			/*
				Another cell is compatible with the current cell if:
				a) it is empty, or
				b) in the same direction, either both cells connect, or both do not connect.
				   (In other words if in the same direction one connects but the other does not, that's bad).
			*/
					
			if (otherCell.IsEmpty)
			{
				return true;
			}
			else if (direction == Direction.Up)
			{
				if ((ConnectsUp && otherCell.ConnectsDown) || 
					(!ConnectsUp && !otherCell.ConnectsDown))
				{
					return true;
				}
			}
			else if (direction == Direction.Down)
			{
				if ((ConnectsDown && otherCell.ConnectsUp) || 
					(!ConnectsDown && !otherCell.ConnectsUp))
				{
					return true;
				}
			}
			else if (direction == Direction.Left)
			{
				if ((ConnectsLeft && otherCell.ConnectsRight) || 
					(!ConnectsLeft && !otherCell.ConnectsRight))
				{
					return true;
				}
			}
			else if (direction == Direction.Right)
			{
				if ((ConnectsRight && otherCell.ConnectsLeft) || 
					(!ConnectsRight && !otherCell.ConnectsLeft))
				{
					return true;
				}
			}

			return false;
		}
	}

	public class Types
	{
		// *** BEGIN FIELD DECLARATIONS ***
		private static readonly Type _emptyCell;
		private static readonly Type _vert;
		private static readonly Type _horiz;
		private static readonly Type _inter;
		private static readonly Type _juncULR;
		private static readonly Type _juncUDR;
		private static readonly Type _juncDLR;
		private static readonly Type _juncUDL;
		private static readonly Type _elbUR;
		private static readonly Type _elbDR;
		private static readonly Type _elbDL;
		private static readonly Type _elbUL;
		private static readonly Type _deadU;
		private static readonly Type _deadD;
		private static readonly Type _deadL;
		private static readonly Type _deadR;
		private static readonly Type _deadexU;
		private static readonly Type _deadexD;
		private static readonly Type _deadexL;
		private static readonly Type _deadexR;
		// *** END FIELD DECLARATIONS ***
		// *** BEGIN PROPERTY DECLARATIONS ***
		public static Type EmptyCell { get { return _emptyCell; } }  // Empty, i.e. unused.
		public static Type Vert { get { return _vert; } }            // Vertical Corridor            
		public static Type Horiz { get { return _horiz; } }          // Horizontal Corridor           
		public static Type Inter { get { return _inter; } }          // Intersection                 
		public static Type JuncULR { get { return _juncULR; } }      // Junction Up Left Right       
		public static Type JuncUDR { get { return _juncUDR; } }      // Junction Up Down Right       
		public static Type JuncDLR { get { return _juncDLR; } }      // Junction Down Left Right     
		public static Type JuncUDL { get { return _juncUDL; } }      // Junction Up Down Left        
		public static Type ElbUR { get { return _elbUR; } }          // Elbow Up Right               
		public static Type ElbDR { get { return _elbDR; } }          // Elbow Down Right             
		public static Type ElbDL { get { return _elbDL; } }          // Elbow Down Left              
		public static Type ElbUL { get { return _elbUL; } }          // Elbow Up Left                
		public static Type DeadU { get { return _deadU; } }          // Dead End Up                  
		public static Type DeadD { get { return _deadD; } }          // Dead End Down                
		public static Type DeadL { get { return _deadL; } }          // Dead End Left                
		public static Type DeadR { get { return _deadR; } }          // Dead End Right 
		public static Type DeadexU { get { return _deadexU; } }      // Dead End Exit Up                  
		public static Type DeadexD { get { return _deadexD; } }      // Dead End Exit Down                
		public static Type DeadexL { get { return _deadexL; } }      // Dead End Exit Left                
		public static Type DeadexR { get { return _deadexR; } }      // Dead End Exit Right 
		// *** END PROPERTY DECLARATIONS ***
		
		static Types()
		{
			_emptyCell = new Type();
			_vert = new Type();
			_horiz = new Type();
			_inter = new Type();
			_juncULR = new Type();
			_juncUDR = new Type();
			_juncDLR = new Type();
			_juncUDL = new Type();
			_elbUR = new Type();
			_elbDR = new Type();
			_elbDL = new Type();
			_elbUL = new Type();
			_deadU = new Type();
			_deadD = new Type();
			_deadL = new Type();
			_deadR = new Type();
			_deadexU = new Type();
			_deadexD = new Type();
			_deadexL = new Type();
			_deadexR = new Type();

			Initialize();  // Static class initialize.
		}

		public static void Initialize()
		{
			// *** BEGIN SPECIAL CELLS ***
			// These cell are only ever placed in specific, known circumstances. They are 
			// never randomly assigned. Therefore, their Weight values are 0.
			// For simplicity, the empty cell is considered to connect in every direction.

			EmptyCell.Weight = 0;
			EmptyCell.IsEmpty = true;
			EmptyCell.ConnectsUp = true;
			EmptyCell.ConnectsDown = true;
			EmptyCell.ConnectsLeft = true;
			EmptyCell.ConnectsRight = true;
			EmptyCell.TextRep = @" ";
			EmptyCell.TextRep2 = @" ";
			EmptyCell.ForceGrowthCompatible = false;
			EmptyCell.InitialAvailableConnections = 4;
			
			// *** END SPECIAL CELLS ***
			
			// *** BEGIN CORRIDOR CELLS ***
			
			Vert.Weight = 100;
			Vert.ConnectsUp = true;
			Vert.ConnectsDown = true;
			Vert.TraversableUp = true;
			Vert.TraversableDown = true;
			Vert.TextRep = @"║";
			Vert.TextRep2 = @" ";
			Vert.InitialAvailableConnections = 2;
			
			Horiz.Weight = 100;
			Horiz.ConnectsLeft = true;
			Horiz.ConnectsRight = true;
			Horiz.TraversableLeft = true;
			Horiz.TraversableRight = true;
			Horiz.TextRep = @"═";
			Horiz.TextRep2 = @"═";
			Horiz.InitialAvailableConnections = 2;
			
			Inter.Weight = 20;
			Inter.ConnectsUp = true;
			Inter.ConnectsDown = true;
			Inter.ConnectsLeft = true;
			Inter.ConnectsRight = true;
			Inter.TraversableUp = true;
			Inter.TraversableDown = true;
			Inter.TraversableLeft = true;
			Inter.TraversableRight = true;
			Inter.TextRep = @"╬";
			Inter.TextRep2 = @"═";
			Inter.IsJunction = true;
			Inter.InitialAvailableConnections = 4;
			
			JuncULR.Weight = 20;
			JuncULR.ConnectsUp = true;
			JuncULR.ConnectsLeft = true;
			JuncULR.ConnectsRight = true;
			JuncULR.TraversableUp = true;
			JuncULR.TraversableLeft = true;
			JuncULR.TraversableRight = true;
			JuncULR.TextRep = @"╩";
			JuncULR.TextRep2 = @"═";
			JuncULR.IsJunction = true;
			JuncULR.InitialAvailableConnections = 3;
			
			JuncUDR.Weight = 20;
			JuncUDR.ConnectsUp = true;
			JuncUDR.ConnectsDown = true;
			JuncUDR.ConnectsRight = true;
			JuncUDR.TraversableUp = true;
			JuncUDR.TraversableDown = true;
			JuncUDR.TraversableRight = true;
			JuncUDR.TextRep = @"╠";
			JuncUDR.TextRep2 = @"═";
			JuncUDR.IsJunction = true;
			JuncUDR.InitialAvailableConnections = 3;
			
			JuncDLR.Weight = 20;
			JuncDLR.ConnectsDown = true;
			JuncDLR.ConnectsLeft = true;
			JuncDLR.ConnectsRight = true;
			JuncDLR.TraversableDown = true;
			JuncDLR.TraversableLeft = true;
			JuncDLR.TraversableRight = true;
			JuncDLR.TextRep = @"╦";
			JuncDLR.TextRep2 = @"═";
			JuncDLR.IsJunction = true;
			JuncDLR.InitialAvailableConnections = 3;
			
			JuncUDL.Weight = 20;
			JuncUDL.ConnectsUp = true;
			JuncUDL.ConnectsDown = true;
			JuncUDL.ConnectsLeft = true;
			JuncUDL.TraversableUp = true;
			JuncUDL.TraversableDown = true;
			JuncUDL.TraversableLeft = true;
			JuncUDL.TextRep = @"╣";
			JuncUDL.TextRep2 = @" ";
			JuncUDL.IsJunction = true;
			JuncUDL.InitialAvailableConnections = 3;
			
			ElbUR.Weight = 20;
			ElbUR.ConnectsUp = true;
			ElbUR.ConnectsRight = true;
			ElbUR.TraversableUp = true;
			ElbUR.TraversableRight = true;
			ElbUR.TextRep = @"╚";
			ElbUR.TextRep2 = @"═";
			ElbUR.InitialAvailableConnections = 2;
			
			ElbDR.Weight = 20;
			ElbDR.ConnectsDown = true;
			ElbDR.ConnectsRight = true;
			ElbDR.TraversableDown = true;
			ElbDR.TraversableRight = true;
			ElbDR.TextRep = @"╔";
			ElbDR.TextRep2 = @"═";
			ElbDR.InitialAvailableConnections = 2;
			
			ElbDL.Weight = 20;
			ElbDL.ConnectsDown = true;
			ElbDL.ConnectsLeft = true;
			ElbDL.TraversableDown = true;
			ElbDL.TraversableLeft = true;
			ElbDL.TextRep = @"╗";
			ElbDL.TextRep2 = @" ";
			ElbDL.InitialAvailableConnections = 2;
			
			ElbUL.Weight = 20;
			ElbUL.ConnectsUp = true;
			ElbUL.ConnectsLeft = true;
			ElbUL.TraversableUp = true;
			ElbUL.TraversableLeft = true;
			ElbUL.TextRep = @"╝";
			ElbUL.TextRep2 = @" ";
			ElbUL.InitialAvailableConnections = 2;

			// *** END CORRIDOR CELLS ***
			// *** BEGIN DEAD END CELLS ***
			
			DeadU.Weight = 1;
			DeadU.ConnectsUp = true;
			DeadU.TraversableUp = true;
			DeadU.TextRep = @"╨";
			DeadU.TextRep2 = @" ";
			DeadU.IsDeadEnd = true;
			DeadU.InitialAvailableConnections = 1;
			
			DeadD.Weight = 1;
			DeadD.ConnectsDown = true;
			DeadD.TraversableDown = true;
			DeadD.TextRep = @"╥";
			DeadD.TextRep2 = @" ";
			DeadD.IsDeadEnd = true;
			DeadD.InitialAvailableConnections = 1;
			
			DeadL.Weight = 1;
			DeadL.ConnectsLeft = true;
			DeadL.TraversableLeft = true;
			DeadL.TextRep = @"╡";
			DeadL.TextRep2 = @" ";
			DeadL.IsDeadEnd = true;
			DeadL.InitialAvailableConnections = 1;
			
			DeadR.Weight = 1;
			DeadR.ConnectsRight = true;
			DeadR.TraversableRight = true;
			DeadR.TextRep = @"╞";
			DeadR.TextRep2 = @"═";
			DeadR.IsDeadEnd = true;
			DeadR.InitialAvailableConnections = 1;
			
			DeadexU.ConnectsUp = true;
			DeadexU.TraversableUp = true;
			DeadexU.TextRep = @"╨";
			DeadexU.TextRep2 = @" ";
			
			DeadexD.ConnectsDown = true;
			DeadexD.TraversableDown = true;
			DeadexD.TextRep = @"╥";
			DeadexD.TextRep2 = @" ";
			
			DeadexL.ConnectsLeft = true;
			DeadexL.TraversableLeft = true;
			DeadexL.TextRep = @"╡";
			DeadexL.TextRep2 = @" ";

			DeadexR.ConnectsRight = true;
			DeadexR.TraversableRight = true;
			DeadexR.TextRep = @"╞";
			DeadexR.TextRep2 = @"═";

			// *** END DEAD END CELLS ***
		}

		// Edge: meaning the extreme edge of the maze's grid.
		public static List<Type> GetTypes(Coords coords)
		{
			List<Type> types = new List<Type>();
			
			if (coords.AdjacentEdgeUp && !coords.AdjacentEdgeLeft && !coords.AdjacentEdgeRight) 
			{
				types.Add(Horiz);
				types.Add(JuncDLR);
				types.Add(ElbDR);
				types.Add(ElbDL);
				types.Add(DeadD);
				types.Add(DeadL);
				types.Add(DeadR);
			} 
			else if (coords.AdjacentEdgeDown && !coords.AdjacentEdgeLeft && !coords.AdjacentEdgeRight) 
			{
				types.Add(Horiz);
				types.Add(JuncULR); 
				types.Add(ElbUR);
				types.Add(ElbUL);
				types.Add(DeadU);
				types.Add(DeadL);
				types.Add(DeadR);
			}
			else if (coords.AdjacentEdgeLeft && !coords.AdjacentEdgeUp && !coords.AdjacentEdgeDown) 
			{
				types.Add(Vert);
				types.Add(JuncUDR);
				types.Add(ElbUR);
				types.Add(ElbDR);
				types.Add(DeadU);
				types.Add(DeadD);
				types.Add(DeadR);
			}
			else if (coords.AdjacentEdgeRight && !coords.AdjacentEdgeUp && !coords.AdjacentEdgeDown) 
			{
				types.Add(Vert);
				types.Add(JuncUDL);
				types.Add(ElbDL);
				types.Add(ElbUL);
				types.Add(DeadU);
				types.Add(DeadD);
				types.Add(DeadL);
			}
			else if (coords.AdjacentEdgeUp && coords.AdjacentEdgeLeft) 
			{
				types.Add(ElbDR);
				types.Add(DeadD);
				types.Add(DeadR);
			}
			else if (coords.AdjacentEdgeUp && coords.AdjacentEdgeRight) 
			{
				types.Add(ElbDL);
				types.Add(DeadD);
				types.Add(DeadL);
			} 
			else if (coords.AdjacentEdgeDown && coords.AdjacentEdgeLeft) 
			{ 
				types.Add(ElbUR);
				types.Add(DeadU);
				types.Add(DeadR);
			}
			else if (coords.AdjacentEdgeDown && coords.AdjacentEdgeRight) 
			{
				types.Add(ElbUL);
				types.Add(DeadU);
				types.Add(DeadL);
			}
			else  // Standard (non-edge) types.
			{
				types.Add(Vert);
				types.Add(Horiz);
				types.Add(Inter);
				types.Add(JuncULR); 
				types.Add(JuncUDR);
				types.Add(JuncDLR);
				types.Add(JuncUDL);
				types.Add(ElbUR);
				types.Add(ElbDR);
				types.Add(ElbDL);
				types.Add(ElbUL);
				types.Add(DeadU);
				types.Add(DeadD);
				types.Add(DeadL);
				types.Add(DeadR);
			}
			
			return types;
		}
	}
}
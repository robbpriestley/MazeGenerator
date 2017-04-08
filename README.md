# MazeGenerator

#### Overview

**MazeGenerator** is a project demonstrating the maze generation algorithm I designed and built using **C# [.NET Core 1.1](https://www.microsoft.com/net/download/core)** and [**Visual Studio Code**](https://code.visualstudio.com/) (running on a **Mac**). The project compiles a **console application** which generates a single maze and then outputs it to text for visualization. The text representation uses **unicode** characters to approximate the appearance of the maze.

The maze generation algorithm is configurable and produces **fully-random** mazes that completely occupy  grids with variable dimensions. The demonstration involves a **25 x 25** grid. The algorithm completes the maze generation task for a grid this size on a modest Mac laptop in about **50 milliseconds**. Larger grids will require more time and computing power to generate.

This project demonstrates my software design and coding abilities. I like to produce very clean, maintainable, **self-documenting** code that includes helpful comments explaining the more difficult concepts. The single best example of code in this project for review purposes is `Library/Maze.cs`, although the project is presented here in its entirety and should be easy to run using [**Visual Studio Code**](https://code.visualstudio.com/).

#### Example Output

![Example 25 x 25 Maze Output](http://www.digitalwizardry.ca/wp-content/themes/one-pager-genesis-master/images/utility/maze.png)

#### Organic Growth

Every maze is **unique**, and evolves during the generation process as growth patterns converge and combine in unexpected ways. Starting with a single cell in the grid that receives a random maze segment, the algorithm **iteratively** adds to the structure by attaching additional maze segments **randomly** selected from lists of compatible segment types.

#### Problem Recovery

The algorithm **adapts**, recovering from common problems that prevent full growth. However, **irrecoverable** problems frequently demand that entire mazes are abandoned mid-generation. Thus, only about one in five mazes are actually completed. The algorithm simply **persists** until it succeeds, although the time to successfully complete a maze is therefore variable.

#### Solving Mazes

Each maze successfully completed must undergo an **analysis** to ensure that each cell in the maze is reachable from every other cell in the maze. This is called <a href="https://en.wikipedia.org/wiki/Maze_solving_algorithm" target="_blank">**solving**</a> the maze, and it prevents "island" portions of the maze from developing independently from one another without any link between them. Solving is accomplished using a [**recursive**](https://en.wikipedia.org/wiki/Recursion_(computer_science)) subroutine.

#### Potential Applications

This algorithm is suitable for use in video game applications. It was originally developed as an aspect of an **iPhone** video game I produced in 2013 called **Robot Attack Maze** (see screenshot below). In 2017, I ported that code from the original **Objective-C** to **C#** and have also separately extended the algorithm to include rooms and other special features that produce richly detailed **standalone** game levels for a future RPG game or similar concept.

#### Further Information

For further information or for business inquiries, please contact me via my website at [www.digitalwizardry.ca](http://www.digitalwizardry.ca).

![Robot Attack Maze iPhone Game Screenshot](http://www.digitalwizardry.ca/wp-content/themes/one-pager-genesis-master/images/utility/RobotAttackMaze.jpg)
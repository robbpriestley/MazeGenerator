# MazeGenerator

#### Overview

**MazeGenerator** is a project showcasing the maze generation algorithm I designed and built using **.NET Core version 1.1** and **C#**. This demonstration project involves a **console application** generating a single maze and then outputting it to text for visualization. The text representation uses **unicode** characters to approximate the appearance of the maze.

The maze generation algorithm is configurable and produces **fully-random** mazes that completely occupy square grids of any size, although larger grids will require more computing power to generate. The demonstration involves a **25 x 25** grid. The algorithm completes the maze generation task for a grid this on a modest Mac laptop in about **50 milliseconds**.

#### Example

![](http://www.digitalwizardry.ca/maze.png)

#### Organic Growth

Every maze is **unique** and evolves during the generation process as growth patterns converge and combine in unexpected ways. Starting with a single cell in the grid that receives a random maze segment, the algorithm **iteratively** adds to the structure by adding additional maze segments **randomly** selected from lists of compatible segment types.

#### Problem Recovery

The algorithm **adapts**, recovering from common problems that prevent full growth. However, **irrecoverable** problems frequently demand that entire mazes are abandoned mid-generation. Thus, only about one in five mazes are actually completed. The algorithm simply **persists** until it succeeds, although the time to successfully complete a maze is therefore variable.

#### Solving Mazes

Each maze successfully completed must undergo an **analysis** to ensure that each cell in the maze is reachable from every other cell in the maze. This is called **solving** the maze, and it prevents "island" portions of the maze from developing independently from one another without any link between them. Solving is accomplished using a **recursive** subroutine.

#### Potential Applications

This algorithm is suitable for use in video game applications. It was originally developed as an aspect of an iPhone video game I produced in 2013 called **Robot Attack Maze**. I have also separately extended the algorithm to include rooms and other special features that produce fully **standalone** game levels for a variety of gaming purposes.
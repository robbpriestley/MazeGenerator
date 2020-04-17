# MazeGenerator

### Functional Demo

[https://dwiz-mazegenerator.herokuapp.com/](https://dwiz-mazegenerator.herokuapp.com/)

### Overview

**MazeGenerator** is a project demonstrating the maze generation algorithm I designed and built using **C# ASP.NET Core MVC** on **Visual Studio Code** for **Mac**. The maze generation algorithm is part of a library that the web application uses to produce maze data. The maze data model is packaged up and delivered to the view by a controller which responds to an **AJAX** request from the index page when it loads, or when a button is clicked.

Supporting **JavaScript** for the web application is transpiled into runnable code from a **TypeScript** source file. The script accepts the maze data in **JSON** format, deserializes it into an object, and assigns **CSS** references to &lt;div&gt; elements in the grid area above in order to place graphical tiles in the proper order to render the maze visually. The **PNG** graphics are part of a **sprite sheet** for faster rendering.

The maze generation algorithm is configurable and produces **fully-random** mazes that completely occupy  grids with variable dimensions. The demonstration involves a **15 x 15** grid. The algorithm completes the maze generation task for a grid this size on a server in just a few milliseconds. Larger grids will require more time and computing power to generate.

This project demonstrates my software design and coding abilities. I like to produce very clean, maintainable, **self-documenting** code that includes helpful **comments** explaining the more difficult concepts. The single best example of code in this project for review purposes is `Library/Maze.cs`, although the project is presented here in its entirety and should be easy to run using [**Visual Studio Code**](https://code.visualstudio.com/).

The entire web application is packaged into a **Docker** container and is hosted in clous services such as AWS or Heroku.

### About the Maze Generation Algorithm

**Sample 25 x 25 Maze in Text Format**

![Example 25 x 25 Maze Output](http://www.digitalwizardry.ca/wp-content/themes/one-pager-genesis-master/images/utility/maze.png)

**Organic Growth**

Every maze is **unique**, and evolves during the generation process as growth patterns converge and combine in unexpected ways. Starting with a single cell in the grid that receives a random maze segment, the algorithm **iteratively** adds to the structure by attaching additional maze segments **randomly** selected from lists of compatible segment types.

**Problem Recovery**

The algorithm **adapts**, recovering from common problems that prevent full growth. However, **irrecoverable** problems frequently demand that entire mazes are abandoned mid-generation. Thus, only about one in five mazes are actually completed. The algorithm simply **persists** until it succeeds, although the time to successfully complete a maze is therefore variable.

**Solving Mazes**

Each maze successfully completed must undergo an **analysis** to ensure that each cell in the maze is reachable from every other cell in the maze. This is called **solving** the maze, and it prevents "island" portions of the maze from developing independently from one another without any link between them. Solving is accomplished using a **recursive** subroutine.

**Potential Applications**

This algorithm is suitable for use in video game applications. It was originally developed as part of an **iPhone** video game I produced in 2013 called **Robot Attack Maze** ([**screenshot**](http://www.digitalwizardry.ca/wp-content/themes/one-pager-genesis-master/images/utility/RobotAttackMaze.jpg)). In 2017, I ported that code from the original **Objective-C** to **C#** and have also separately extended the algorithm to include rooms and other special features that produce richly detailed **standalone** game levels for a future RPG game or similar concept.
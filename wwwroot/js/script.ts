"use strict";

let dungeon : object = null;

function Reset() : void
{
	var spinner = SpinnerSetup();
	spinner.spin($('#grid')[0]);
	
	$.ajax
	({
		type: 'GET',
		dataType: 'json',
		contentType: 'application/json',
		url: '/Index/MazeView',
		success: function (result) 
		{
			dungeon = [JSON.parse(result)];  // As this is a reset, assign the JSON to element 0 of the dungeon array.
			PresentMaze(dungeon[0]);
			spinner.stop();
		}
	});
}

// This function is going to iterate through each div in the grid and determine its location. It will then
// strip it of any existing CSS classes. This also removes the "native" classes the div needs ("tile" and 
// it's location class). Those classes are added back on, as well as the tile name class that allows the 
// div to display a graphical tile from the tile sprite sheet.
function PresentMaze(dungeonLevel : object) : void
{
	for (var x = 0; x <= 14; x++) 
	{
		for (let y = 0; y <= 14; y++)
		{
			let gridReference : string = GridReference(x, y);
			let gridId : string = "#" + gridReference;       // Prepend the "#" because jQuery needs it for the id attribute.
			$(gridId).removeClass();                         // Remove all classes on the div.
			$(gridId).addClass("tile");                      // Add "native" class.
			$(gridId).addClass(gridReference);               // Add "native" class.
			$(gridId).addClass(dungeonLevel[x][y].CssName);  // Add tile name class.
		}
	}
}

// Synthesize a grid reference given X and Y coords.
function GridReference(x : number, y : number) : string
{
	let xs = x < 10 ? "0" + x.toString() : x.toString();
	let ys = y < 10 ? "0" + y.toString() : y.toString();
	return "g" + xs + "-" + ys;
}

// *** BEGIN UTILITY ***

function SpinnerSetup() : Spinner
{
	var opts = 
	{
		lines: 9,
		length: 0,
		width: 13,
		radius: 21,
		color: '#fff',
		opacity: 0.25,
		rotate: 33
	}

	return new Spinner(opts);
}

// *** END UTILITY ***
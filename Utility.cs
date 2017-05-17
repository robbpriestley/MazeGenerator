using System;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace DigitalWizardry.MazeGenerator
{	
	public class Utility
	{
		public static string Json(Object obj)
		{
			string json = 
			
			JsonConvert.SerializeObject
			(
				obj, 
				Formatting.Indented, 
				new JsonSerializerSettings 
				{ 
					ReferenceLoopHandling = ReferenceLoopHandling.Ignore
				}
			);
			
			return json;
		}
		
		public static ObjectResult JsonObjectResult(Object obj)
		{		
			return new ObjectResult(Json(obj));
		}
	}
}
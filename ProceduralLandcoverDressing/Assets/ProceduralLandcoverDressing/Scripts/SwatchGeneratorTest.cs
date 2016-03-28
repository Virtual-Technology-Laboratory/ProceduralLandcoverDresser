/*
 * Copyright (c) 2015, Jacob Cooper (coop3683@outlook.com)
 * Date: 03/25/2016
 * License: BSD (3-clause license)
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VTL.SwatchGenerator{

	[System.Serializable]
	public class SwatchGeneratorTest : MonoBehaviour 
	{
		[Range(2, 64)]
		public int resolution = 16;
		public string savePath = "/Textures/Swatches";
		public List<Color> colors = new List<Color> ();

		public void PushOut () 
		{
			//make sure each alpha is white
			for(int i = 0; i < colors.Count; i++)
			{
				colors [i] += new Color (0, 0, 0, 1f);
			}

			if (colors.Count == 1)
				SwatchGenerator.Generate (colors [0], resolution, savePath);
			if (colors.Count > 1)
				SwatchGenerator.Generate (colors.ToArray (), resolution, savePath);

			print ("PushOut finished");
		}
	}
}

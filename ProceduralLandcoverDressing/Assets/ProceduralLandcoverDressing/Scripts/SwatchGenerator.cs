/*
 * Copyright (c) 2015, Jacob Cooper (coop3683@outlook.com)
 * Date: 03/25/2016
 * License: BSD (3-clause license)
 * 
 */

using UnityEngine;
using System.Collections;

namespace VTL.SwatchGenerator
{

	public static class SwatchGenerator 
	{
		public static void Generate (Color[] colorArr, int resolution, string destinationPath)
		{
			for (int i = 0; i < colorArr.Length; i++) 
			{
				Texture2D swatch = new Texture2D (resolution, resolution);
				swatch.anisoLevel = 1;
				swatch.filterMode = FilterMode.Point;

				for (int y = 0; y < resolution; y++) 
				{
					for (int x = 0; x < resolution; x++) 
					{
						swatch.SetPixel (x, y, colorArr [i]);
					}
				}
				Debug.Log ("Saving: " + i);
				byte[] bytes = swatch.EncodeToPNG ();
				try
				{
					System.IO.Directory.CreateDirectory(Application.dataPath + destinationPath);
				}
				catch(System.Exception e)
				{
					Debug.Log (e);
				}

				System.IO.File.WriteAllBytes (Application.dataPath + string.Format("{0}/Swatch_r{1}_g{2}_b{3}.png", destinationPath, (int)(colorArr[i].r * 255), (int)(colorArr[i].g * 255), (int)(colorArr[i].b * 255)), bytes);
			}


		}//- end Generate (array)

		public static void Generate (Color col, int resolution, string destinationPath)
		{
			Texture2D swatch = new Texture2D (resolution, resolution);
			swatch.anisoLevel = 1;
			swatch.filterMode = FilterMode.Point;

			for (int y = 0; y < resolution; y++) 
			{
				for (int x = 0; x < resolution; x++) 
				{
					swatch.SetPixel (x, y, col);
				}
			}
				
			byte[] bytes = swatch.EncodeToPNG ();
			try
			{
				System.IO.Directory.CreateDirectory(Application.dataPath + destinationPath);
			}
			catch(System.Exception e)
			{
				Debug.Log (e);
			}

			System.IO.File.WriteAllBytes (Application.dataPath + string.Format("{0}/Swatch_r{1}_g{2}_b{3}.png", destinationPath, (int)(col.r * 255), (int)(col.g * 255), (int)(col.b * 255)), bytes);

		}//- end Generate (single)

		//Color32

		public static void Generate (Color32[] colorArr, int resolution, string destinationPath)
		{
			for (int i = 0; i < colorArr.Length; i++) 
			{
				Texture2D swatch = new Texture2D (resolution, resolution);
				swatch.anisoLevel = 1;
				swatch.filterMode = FilterMode.Point;

				for (int y = 0; y < resolution; y++) 
				{
					for (int x = 0; x < resolution; x++) 
					{
						swatch.SetPixel (x, y, colorArr [i]);
					}
				}
				Debug.Log ("Saving: " + i);
				byte[] bytes = swatch.EncodeToPNG ();
				try
				{
					System.IO.Directory.CreateDirectory(Application.dataPath + destinationPath);
				}
				catch(System.Exception e)
				{
					Debug.Log (e);
				}

				System.IO.File.WriteAllBytes (Application.dataPath + string.Format("{0}/Swatch_r{1}_g{2}_b{3}.png", destinationPath, (int)(colorArr[i].r * 255), (int)(colorArr[i].g * 255), (int)(colorArr[i].b * 255)), bytes);
			}


		}//- end Generate (array)

		public static void Generate (Color32 col, int resolution, string destinationPath)
		{
			Texture2D swatch = new Texture2D (resolution, resolution);
			swatch.anisoLevel = 1;
			swatch.filterMode = FilterMode.Point;

			for (int y = 0; y < resolution; y++) 
			{
				for (int x = 0; x < resolution; x++) 
				{
					swatch.SetPixel (x, y, col);
				}
			}

			byte[] bytes = swatch.EncodeToPNG ();
			try
			{
				System.IO.Directory.CreateDirectory(Application.dataPath + destinationPath);
			}
			catch(System.Exception e)
			{
				Debug.Log (e);
			}

			System.IO.File.WriteAllBytes (Application.dataPath + string.Format("{0}/Swatch_r{1}_g{2}_b{3}.png", destinationPath, (int)(col.r * 255), (int)(col.g * 255), (int)(col.b * 255)), bytes);

		}//- end Generate (single)

	}//- end class
}
/*
 * Copyright (c) 2015, Jacob Cooper (coop3683@outlook.com)
 * Date: 2/17/2015
 * License: BSD (3-clause license)
 */ 

using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace VTL.ProceduralLandcoverDresser
{
	[System.Serializable]

	public class ProceduralLandcoverDresserCSVReader : MonoBehaviour
	{
		public ProceduralLandcoverDresser pld;
		public string fileName;
		public int dataRows;
		public int dataColumns;
		public int skipRows = 0;
		public int skipColumns = 0;
		public bool imageKeyReconciliation;

		public void ReadFile(int output)
		{
			if (ErrorCheck())
				return;

			TextAsset fileData = Resources.Load (fileName) as TextAsset;

			List <Color32> newKeyColors = new List<Color32> ();

			string[] fileDataLines = fileData.text.Split(new string[]{"\n"}, System.StringSplitOptions.None);
			string[,] sortedData = new string[dataRows,dataColumns];
			int i,col;
			float x, y, z;

			for (i = 0; i < sortedData.GetLength(0); i++)
			{
				string[] line = fileDataLines [i].Split (new char[]{ ',' }, System.StringSplitOptions.None);
				for(col = 0; col < line.Length; col++)
				{
					sortedData[i,col] = line[col];
				}
			}

			for (i = skipRows; i < dataRows; i++)
			{
				bool xx = float.TryParse (sortedData[i, skipColumns], out x);
				bool yy = float.TryParse (sortedData[i, skipColumns+1], out y);
				bool zz = float.TryParse (sortedData[i, skipColumns+2], out z);
				if (xx && yy && zz) 
				{
					x = Mathf.Round (x * 1000) / 1000f;
					y = Mathf.Round (y * 1000) / 1000f;
					z = Mathf.Round (z * 1000) / 1000f;
					newKeyColors.Add (new Color32 ((byte)(x * 255f), (byte)(y * 255f), (byte)(z * 255f), (byte)255f));
				}
				else 
				{
					Debug.Log ("There was an error reading line " + i);
					return;
				}
					
			}

			print ("New keyKeys: " + newKeyColors.Count);

			switch(output)
			{
			case 1:
				if (imageKeyReconciliation) 
					VTL.SwatchGenerator.SwatchGenerator.Generate (ReconcileColors (newKeyColors).ToArray(), 16, "/Textures/Swatches");
				else 
					VTL.SwatchGenerator.SwatchGenerator.Generate (newKeyColors.ToArray(), 16, "/Textures/Swatches");
				break;

			default:
				if (imageKeyReconciliation) 
					pld.ImageColorKey = ReconcileColors (newKeyColors);
				else 
					pld.ImageColorKey = newKeyColors;
				break;

			}
			//ReconcileColors (newKeyColors);
			//print (newKeyColors [0]);
			//print (newKeyColors [3]+" _z_ " +newKeyColors[3].z);
		}

		List<Color32> ReconcileColors(List<Color32> keys)
		{
			Color32[] imgColors = pld.keyImage.GetPixels32 ();

			imgColors = imgColors.Distinct ().ToArray (); // Remove duplicate colors

			print(imgColors.Length + " colors to sort through");

			int i,r,g,b,index;
			bool foundSimilar;
			int matches = 0, existing = 0;

			for(i = 0; i < imgColors.Length; i++)
			{
				Color32 px;

				if (keys.Exists (k => imgColors[i].Equals(k)))
				{
					existing++;
					continue;
				} 
				else 
				{
					foundSimilar = false;

					for(r = -1; r < 2; r++)
					{
						for(g = -1; g < 2; g++)
						{
							for(b = -1; b < 2; b++)
							{
								if (r == 0 && g == 0 && b == 0)
									continue;
								else 
								{									
									px = new Color32 ((byte)(imgColors[i].r + r), (byte)(imgColors[i].g + g), (byte)(imgColors[i].b + b), imgColors[i].a);
									index = IndexOfColor32 (keys, px);

									if (index != -1) 
									{
										keys [index] = imgColors [i];
										foundSimilar = true;
										matches++;
									}
								}									
							}
						}
					}

					if (!foundSimilar)
						Debug.Log ("No match found for " + imgColors [i]);
				}
			}

			print (string.Format ("{0} color matches made, {1} existing matches found", matches, existing));

			return keys;

		}//- end Reconcile Colors

		int IndexOfColor32 (List<Color32> list, Color32 col)
		{
			for (int i = 0; i < list.Count; i++) 
			{
				if (list [i].Equals (col)) 
				{
					return i;
				}
			}

			return -1;
		}//- end IndexOfColor32

		bool ErrorCheck()
		{
			if (pld == null) 
			{
				Debug.LogError ("No ProceduralLandcoverDresser script assigned to Reader");
				return true;
			}
			if (fileName == "") 
			{
				Debug.LogError ("No File Name provided for CSV file");
				return true;
			}
			return false;
		}
	}
}
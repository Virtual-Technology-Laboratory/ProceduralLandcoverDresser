/*
 * Copyright (c) 2015, Jacob Cooper (coop3683@outlook.com)
 * Date: 2/17/2015
 * License: BSD (3-clause license)
 */ 

using UnityEngine;
using UnityEditor;
using System.Collections;

namespace VTL.ProceduralLandcoverDresser
{
	[System.Serializable]

	[CustomEditor(typeof(ProceduralLandcoverDresserCSVReader))]
	public class PLDCSVReaderEditor : Editor 
	{

		public override void OnInspectorGUI()
		{

			DrawDefaultInspector ();

			ProceduralLandcoverDresserCSVReader PLDReader = (ProceduralLandcoverDresserCSVReader)target;
			if (GUILayout.Button ("Add ImageKey to PLD")) 
			{
				PLDReader.ReadFile (0);
			}

			if (GUILayout.Button ("Create Swatches")) 
			{
				PLDReader.ReadFile (1);
			}
		}

	}//- end Class
}
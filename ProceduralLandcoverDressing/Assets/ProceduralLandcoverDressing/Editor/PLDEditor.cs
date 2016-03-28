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

	[CustomEditor(typeof(ProceduralLandcoverDresser))]
	public class PLDEditor : Editor 
	{

		public override void OnInspectorGUI()
		{

			DrawDefaultInspector ();

			GUILayout.Space (20);

			ProceduralLandcoverDresser PLD = (ProceduralLandcoverDresser)target;

			//Insert ReadOnly Vars

			GUILayout.Label ("Key Image Resolution:");
			GUILayout.Label ("Tree Total:");
			GUILayout.Label ("Detail Total:");

			GUILayout.Space (20);

			if (GUILayout.Button ("Populate Textures")) 
			{
				PLD.StartPopulateTextures();
			}

			if (GUILayout.Button ("Populate Trees")) 
			{
				PLD.StartPopulateTrees();
			}

			if (GUILayout.Button ("Populate Details")) 
			{
				PLD.StartPopulateDetails();
			}

			if (GUILayout.Button ("Populate All")) 
			{
				PLD.StartAll ();
			}

			GUILayout.Space (20);

			string title = "Clear Confirmation";
			string message = "Are you sure you wish to clear this aspect of the terrain data?";

			if (GUILayout.Button ("ClearTextures")) 
			{
				if (EditorUtility.DisplayDialog (title, message, "Apply", "Cancel")) 
				{
					PLD.ClearTextures ();
				}
			}

			if (GUILayout.Button ("ClearTrees")) 
			{
				if (EditorUtility.DisplayDialog (title, message, "Apply", "Cancel")) 
				{
					PLD.ClearTrees ();
				}
			}

			if (GUILayout.Button ("ClearDetails")) 
			{
				if (EditorUtility.DisplayDialog (title, message, "Apply", "Cancel")) 
				{
					PLD.ClearDetails ();
				}
			}

			if (GUILayout.Button ("ClearAll")) 
			{
				if (EditorUtility.DisplayDialog (title, "Clear textures, trees, and details from the terrain data?", "Apply", "Cancel")) 
				{
					PLD.ClearAll ();
				}
			}

			GUILayout.Space (20);

		}//- end OnInspectorGUI

	}//- end Class
}
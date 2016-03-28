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

	[CustomEditor(typeof(ProceduralLandcoverDresserTerrainAssetInitializer))]
	public class PLDTerrainAssetInitializerEditor : Editor 
	{

		public override void OnInspectorGUI()
		{

			DrawDefaultInspector ();

			GUILayout.Space (20);

			ProceduralLandcoverDresserTerrainAssetInitializer PLDTAI = (ProceduralLandcoverDresserTerrainAssetInitializer)target;

			if (GUILayout.Button ("Initialize Assets")) 
			{
				PLDTAI.AddObjects ();
			}

			GUILayout.Space (20);

		}//- end OnInspectorGUI

	}//- end Class
}

/*
 * Copyright (c) 2015, Jacob Cooper (coop3683@outlook.com)
 * Date: 03/25/2016
 * License: BSD (3-clause license)
 * 
 */

using UnityEditor;
using UnityEngine;
using System.Collections;

namespace VTL.SwatchGenerator 
{
	[CustomEditor(typeof(SwatchGeneratorTest))]
	public class SwatchGeneratorTestEditor : Editor 
	{
		private SwatchGeneratorTest SGT;

		public override void OnInspectorGUI () 
		{
			SGT = (SwatchGeneratorTest)target;
			DrawDefaultInspector ();

			if (GUILayout.Button ("Generate Swatches")) 
			{
				SGT.PushOut ();
			}
		}
	}
}

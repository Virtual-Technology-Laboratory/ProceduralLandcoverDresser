/*
 * Copyright (c) 2015, Jacob Cooper (coop3683@outlook.com)
 * Date: 2/17/2015
 * License: BSD (3-clause license)
 */ 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VTL.ProceduralLandcoverDresser 
{
	[System.Serializable]
	public class ProceduralLandcoverDresserTerrainAssetInitializer : MonoBehaviour 
	{
		public List<Texture2D> addTextures = new List<Texture2D>();
		public List<Transform> addTrees = new List<Transform>();
		public List<Texture2D> addGrasses = new List<Texture2D>();
		public List<Transform> addDetailMeshes = new List<Transform>();
		[Space(10)]

		public bool addObjectsOverwrite;
		[Space(10)]

		private TerrainData terrainData;

		void Start()
		{
			terrainData = GetComponent<Terrain> ().terrainData;
		}

		public void AddObjects ()
		{
			int obj;

			List<SplatPrototype> newSplats = new List<SplatPrototype> ();
			List<TreePrototype> newTrees = new List<TreePrototype> ();
			List<DetailPrototype> newDetails = new List<DetailPrototype> ();

			if (!addObjectsOverwrite) 
			{
				foreach (SplatPrototype sp in terrainData.splatPrototypes) 
				{
					newSplats.Add (sp);
				}
				foreach (TreePrototype tp in terrainData.treePrototypes) 
				{
					newTrees.Add (tp);
				}
				foreach (DetailPrototype dp in terrainData.detailPrototypes) 
				{
					newDetails.Add (dp);
				}
			}

			for (obj = 0; obj < addTextures.Count; obj++) 
			{
				SplatPrototype nSplat = new SplatPrototype ();
				nSplat.texture = addTextures [obj];
				nSplat.tileSize = Vector2.one * 15f;
				nSplat.tileOffset = Vector2.zero;

				newSplats.Add (nSplat);
			}

			terrainData.splatPrototypes = newSplats.ToArray ();

			for (obj = 0; obj < addTrees.Count; obj++) 
			{
				TreePrototype nTree = new TreePrototype ();
				nTree.prefab = addTrees [obj].gameObject;
				nTree.bendFactor = 1.0f;

				newTrees.Add (nTree);
			}

			terrainData.treePrototypes = newTrees.ToArray ();

			for (obj = 0; obj < addGrasses.Count; obj++) 
			{
				DetailPrototype nDetail = new DetailPrototype ();
				nDetail.usePrototypeMesh = false;
				nDetail.prototypeTexture = addGrasses [obj];
				nDetail.bendFactor = 1f;
				nDetail.healthyColor = new Color32 (67, 249, 42, 255);
				nDetail.dryColor = new Color32 (205, 188, 26, 255);
				nDetail.noiseSpread = 0.1f;
				nDetail.minHeight = 1f;
				nDetail.maxHeight = 2f;
				nDetail.minWidth = 1f;
				nDetail.maxWidth = 2f;
				nDetail.renderMode = DetailRenderMode.GrassBillboard;

				newDetails.Add (nDetail);
			}

			for (obj = 0; obj < addDetailMeshes.Count; obj++) 
			{
				DetailPrototype nDetail = new DetailPrototype ();
				nDetail.usePrototypeMesh = true;
				nDetail.prototype = addDetailMeshes[obj].gameObject;
				nDetail.bendFactor = 1f;
				nDetail.healthyColor = Color.white;
				nDetail.dryColor = Color.white;
				nDetail.noiseSpread = 0.1f;
				nDetail.minHeight = 0.5f;
				nDetail.maxHeight = 1f;
				nDetail.minWidth = 0.5f;
				nDetail.maxWidth = 1f;
				nDetail.renderMode = DetailRenderMode.VertexLit;

				newDetails.Add (nDetail);
			}

			terrainData.detailPrototypes = newDetails.ToArray ();

		}//- end AddObjects

	}//- end class
}

/*
 * Copyright (c) 2015, Jacob Cooper (coop3683@outlook.com)
 * Date: 2/17/2015
 * License: BSD (3-clause license)
 */ 

using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace VTL.ProceduralLandcoverDresser 
{
	[System.Serializable]

	public class ProceduralLandcoverDresser : MonoBehaviour 
	{
		public TerrainData terrainData;
		public LayerMask terrainLayer;
		public Texture2D keyImage;
		public List<Color32> ImageColorKey = new List<Color32>();
		[Space(10)]

		public bool textureBlur;
		[Range(1, 8)]
		public int textureBlurSize = 2;
		public List<bool> textureBlurExclusions = new List<bool> ();
		[Space(10)]

		public float treeSpacing;				// literal spacing in meters
		[Range(0f, 1f)]
		public float treeJitter = 0.5f;			// decimal percentage of treespacing
		[Range(0f, 1f)]
		public float treeDeath = 0f;			// decimal percentage chance of a tree not to spawn
		public bool treeDensityNoise;
		public Texture2D treeDensityAlphaMap; 	//White = treeDeath, Black = 1
		[Space(10)]

		public int detailPixelSpacing;
		[Range(0f, 1f)]
		public float detailJitter = 0.5f;
		[Space(10)]

		//User defined lists of asset indicies which correlate to the LCI Key index
		//Trees and details groups are comma deliniated (e.g. 0,1,4,5,18) *no spaces
		public List<int> textureAssets = new List<int>();
		public List<string> treeAssets = new List<string> ();
		public List<string> detailAssets = new List<string> ();
		[Space(10)]

		// Weights of the assets found in the terrainData, all will be populated @ 1.0f if empty
		public List<float> treeWeights = new List<float>();
		public List<float> detailWeights = new List<float>();
		[Space(10)]

		[HideInInspector]
		public int treeCount;
		[HideInInspector]
		public int detailCount;

		private bool pldReady = false;
		private bool treeEmpty = true;
		private bool detailEmpty = true;

		private int treeDensityMapResolution = 512;

		private List<List<List<int>>> assetList = new List<List<List<int>>>(); // Format: assetList[key][group][index][weight]

		void Start () 
		{
			if (ErrorCheck())
				return;

			if (ImageColorKey.Count == 0) 
			{
				Debug.Log ("No Landcover Image Key data found!");
				return;
			}

			//Initialize
			StartCoroutine (ProcessLists());

		}//- end Start

		IEnumerator ProcessLists()
		{
			// Front load the assetList
			for (int i = 0; i < ImageColorKey.Count; i++) 
			{
				assetList.Add (new List<List<int>> ());

				List<int> newTreeAssetList = new List<int> ();
				List<int> newDetailAssetList = new List<int> ();

				string[] assetSubstring;

				if (treeAssets[i] != "") 
				{	
					int treePeak = 0;
					assetSubstring = treeAssets[i].Split(new string[] { "," }, System.StringSplitOptions.None);

					foreach (string s in assetSubstring) 
					{
						int assetIndex = 0;
						try 
						{
							assetIndex = int.Parse (s);
						} 
						catch (System.FormatException e) 
						{
							Debug.Log ("FormatException at: " + i + " of treeAssets - " + e);
							yield break;
						}
						newTreeAssetList.Add (assetIndex);
						treeEmpty = false;

						if (assetIndex > treePeak)
							treePeak = assetIndex;
					}

					//find the difference between the highest asset index used and check if it has a weight, if not then build weights to the index
					if (treePeak >= treeWeights.Count) 
					{
						int m = treePeak - (treeWeights.Count - 1);
						for (int n = 0; n < m; n++) 
						{
							treeWeights.Add (1.0f);
						}
					}
				}

				if (detailAssets [i] != "") 
				{
					int detailPeak = 0;
					assetSubstring = detailAssets [i].Split (new string[] { "," }, System.StringSplitOptions.None);

					foreach (string s in assetSubstring) 
					{
						int assetIndex = 0;
						try 
						{
							assetIndex = int.Parse (s);
						} 
						catch (System.FormatException e) 
						{
							Debug.Log ("FormatException at: " + i + " of treeAssets - " + e);
							yield break;
						}
						newDetailAssetList.Add (assetIndex);
						detailEmpty = false;

						if (assetIndex > detailPeak)
							detailPeak = assetIndex;
					}

					//find the difference between the highest asset index used and check if it has a weight, if not then build weights to the index
					if (detailPeak >= detailWeights.Count) 
					{
						int m = detailPeak - (detailWeights.Count - 1);
						for (int n = 0; n < m; n++) 
						{
							detailWeights.Add (1.0f);
						}
					}
				}
				assetList [i].Add (newTreeAssetList);
				assetList [i].Add (newDetailAssetList);

			}

			int tbeDiff = terrainData.splatPrototypes.Length - textureBlurExclusions.Count;
			if(tbeDiff > 0)
			{				
				for (; tbeDiff > 0; tbeDiff--) 
				{
					textureBlurExclusions.Add (false);
				}
			}

			if (treeDensityNoise && treeDensityAlphaMap == null) 
			{
				Texture2D texture;
				texture = new Texture2D (treeDensityMapResolution, treeDensityMapResolution, TextureFormat.RGB24, true);
				texture.name = "treeDensityAlphaMap";
				texture.wrapMode = TextureWrapMode.Clamp;
				texture.filterMode = FilterMode.Trilinear;
				texture.anisoLevel = 9;

				Vector3 point00 = transform.TransformPoint(new Vector3 (-0.5f, -0.5f));
				Vector3 point10 = transform.TransformPoint(new Vector3 (0.5f, -0.5f));
				Vector3 point01 = transform.TransformPoint(new Vector3 (-0.5f, 0.5f));
				Vector3 point11 = transform.TransformPoint(new Vector3 (0.5f, 0.5f));

				NoiseGenerator.NoiseMethod method = NoiseGenerator.NoiseGenerator.noiseMethods[1][2];//Perlin3D

				float stepSize = 1f / treeDensityMapResolution;
				for (int y = 0; y < treeDensityMapResolution; y++) 
				{
					Vector3 point0 = Vector3.Lerp (point00, point01, (y + 0.5f) * stepSize);
					Vector3 point1 = Vector3.Lerp (point10, point11, (y + 0.5f) * stepSize);

					for (int x = 0; x < treeDensityMapResolution; x++) 
					{
						Vector3 point = Vector3.Lerp (point0, point1, (x + 0.5f) * stepSize);
						//float sample = method (point, frequency);
						float sample = NoiseGenerator.NoiseGenerator.Generate(method, point, 8, 2, 2, 0.5f);
						sample = sample * 0.5f + 0.5f; // move -1 to 1 range into 0 to 1 for visibility
						texture.SetPixel (x, y, Color.white * sample);
					}
				}
				texture.Apply ();
				treeDensityAlphaMap = texture;
			}

			pldReady = true;
			yield break;

		}//- end ProcessLists

		IEnumerator PopulateTextures()
		{
			//float[,,] maps = terrainData.GetAlphamaps (0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight);
			float[,,] maps = new float[terrainData.alphamapWidth,terrainData.alphamapHeight,terrainData.alphamapLayers];

			long timer = DateTime.Now.Ticks;

			print ("Populate Textures Begin...");
			for (int x = 0; x < terrainData.alphamapHeight; x++) 
			{
				for (int y = 0; y < terrainData.alphamapWidth; y++) 
				{

					float normalX = (float)x / (float)terrainData.alphamapWidth;
					float normalY = (float)y / (float)terrainData.alphamapHeight;
					Color32 keyColor = keyImage.GetPixel ((int)((float)keyImage.width * normalX), (int)((float)keyImage.height * normalY));
					int key = ImageColorKey.FindIndex(k => k.Equals(keyColor));

					if (key == -1) 
					{
						Debug.LogError (string.Format ("Color ({0}) not recognized in ImageColorKey", keyColor));
						continue;
					}

					try
					{
						maps [x, y, textureAssets [key]] = 1f;
					}
					catch(System.IndexOutOfRangeException e)
					{
						Debug.LogError ( string.Format("{0} index is out of range at texturesAssets index {1}, check your indicies to make sure they are < the count of textures. {2}", textureAssets[key], key, e));
						continue;
					}
					catch(System.ArgumentOutOfRangeException e)
					{
						Debug.LogError ( string.Format("{0} index is out of range at texturesAssets index {1}, check your indicies to make sure they are < the count of textures. {2}", textureAssets[key], key, e));
					}
				}
			}
				
			if (textureBlur) 
			{
				maps = Blur (maps);
			}

			// roughly 300ms operation
			terrainData.SetAlphamaps (0, 0, maps);
			terrainData.RefreshPrototypes ();

			print (string.Format("Populate Textures End, {0}ms", (DateTime.Now.Ticks-timer)/10000));

			yield break;

		}//- end PopulateTextures

		IEnumerator PopulateTrees()
		{
			if(!treeEmpty)
			{
				List<TreeInstance> treeList = new List<TreeInstance> ();

				int x, y, treeNoHit = 0;
				Vector3 offset = transform.position;
				Vector2 pxSize = new Vector2 (terrainData.size.x / (float)keyImage.width, terrainData.size.z / (float)keyImage.height);
				Vector2 treeTotal = new Vector2 (terrainData.size.x / treeSpacing, terrainData.size.z / treeSpacing);

				for (y = 0; y < treeTotal.y; y++) 
				{
					for (x = 0; x < treeTotal.x; x++) 
					{
						Vector3 treePos = new Vector3 ((float)x * treeSpacing, 0, (float)y * treeSpacing);

						//treeDeath can be modified by adding probability of death based on the dark area in an alpha map
						float treeDeathModifier = 1f;
						if (treeDensityNoise) 
							treeDeathModifier = treeDensityAlphaMap.GetPixel ((int)(treePos.x / pxSize.x), (int)(treePos.z / pxSize.y)).grayscale;

						if (UnityEngine.Random.value < (treeDeath + ((1f - treeDeath) * (1f - treeDeathModifier))))
							continue;	

						Color32 keyColor = keyImage.GetPixel ((int)(treePos.x / pxSize.x), (int)(treePos.z / pxSize.y));
						int key = ImageColorKey.FindIndex (k => k.Equals (keyColor));

						if (key == -1) 
						{
							Debug.LogError (string.Format ("Color ({0}) not recognized in ImageColorKey", keyColor));
							continue;
						}

						if (treeAssets [key] != "") 
						{
							RaycastHit hit;
							Vector2 jitterPos = new Vector2 (treePos.z + (UnityEngine.Random.Range (0, treeJitter) * treeSpacing), treePos.x + (UnityEngine.Random.Range (0, treeJitter) * treeSpacing));
							Vector3 raycastVector = new Vector3 (Mathf.Clamp (jitterPos.x, 0, terrainData.size.x - 1) + offset.x, 2000 + offset.y, Mathf.Clamp (jitterPos.y, 0, terrainData.size.z - 1) + offset.z);
							if (Physics.Raycast (raycastVector, -Vector3.up, out hit, 4000f, terrainLayer)) 
							{
								treePos = new Vector3 ((hit.point.x - offset.x) / terrainData.size.x, (hit.point.y - offset.y) / terrainData.size.y, (hit.point.z - offset.z) / terrainData.size.z);
							} 
							else 
							{
								treeNoHit++;
								//Debug.Log ("No Hit at: " + raycastVector);
								continue;
							}

							int p = GetPrototype (key, 0);
							TreeInstance tree = new TreeInstance ();
							tree.color = Color.white;
							tree.rotation = UnityEngine.Random.value * 6.26f;
							tree.heightScale = UnityEngine.Random.Range (0.8f, 1.2f);
							tree.lightmapColor = Color.white;
							tree.position = treePos;
							try 
							{
								tree.prototypeIndex = assetList [key] [0] [p];
							} 
							catch (ArgumentOutOfRangeException e) 
							{
								Debug.Log (string.Format ("{0} is out of range in key element {1}! {2}", p, key, e));
								continue;
							}
							tree.widthScale = 1.0f;
							treeList.Add (tree);
							treeCount++;

							/* Debug - print location of Nth tree
							if (treeCount % 2000 == 0)
								Debug.Log ("Tree " + treeCount + " @ " + treePos);
							*/
						
						} 
						else 
						{
							treeNoHit++;
						}
					}
				}

				print ("Trees Generated: " + treeCount);
			
				terrainData.treeInstances = treeList.ToArray ();

			}
			else
			{
				terrainData.treeInstances = new List<TreeInstance> ().ToArray ();
				Debug.Log("No Tree assets have been designated in inspector! Cannot spawn trees. Existing Trees Cleared!");	
			}

			yield break;
		}//- end PopulateTrees

		IEnumerator PopulateDetails()
		{
			//clear Details
			for (int layer = 0; layer < terrainData.detailPrototypes.Length; layer++) 
			{
				terrainData.SetDetailLayer (0, 0, layer, new int[terrainData.detailHeight, terrainData.detailWidth]);
			}
				
			List<int[,]> detailMaps = new List<int[,]> ();

			if(!detailEmpty)
			{
				for (int layer = 0; layer < terrainData.detailPrototypes.Length; layer++) 
				{
					detailMaps.Add(terrainData.GetDetailLayer (0, 0, terrainData.detailWidth, terrainData.detailHeight, layer));
				}
				print ("Detail Layers:" + detailMaps.Count);
				for (int y = 0; y < terrainData.detailHeight; y++) 
				{
					for (int x = 0; x < terrainData.detailWidth; x++) 
					{
						float normalX = (float)x / (float)terrainData.detailWidth;
						float normalY = (float)y / (float)terrainData.detailHeight;
						Color keyColor = keyImage.GetPixel ((int)((float)keyImage.width * normalX), (int)((float)keyImage.height * normalY));

						int key = ImageColorKey.FindIndex(k => k == keyColor);

						if (key == -1) 
						{
							Debug.LogError (string.Format ("Color ({0}) not recognized in ImageColorKey", keyColor));
							continue;
						}

						if (detailAssets [key] != "") 
						{
							int p = GetPrototype (key, 1);

							if (p == 999)
								continue;

							detailMaps [p] [x, y] = 1;
							detailCount++;
						}
					}
				}

				for (int layer = 0; layer < terrainData.detailPrototypes.Length; layer++) 
				{

					terrainData.SetDetailLayer (0, 0, layer, detailMaps[layer]);	
				}

				print("Details Generated: " + detailCount);
			}
			else
			{
				for (int layer = 0; layer < terrainData.detailPrototypes.Length; layer++) 
				{
					terrainData.SetDetailLayer (0, 0, layer, new int[terrainData.detailHeight, terrainData.detailWidth]);
				}
				Debug.Log("No Detail assets have been designated in inspector! Cannot spawn Details. Existing Details Cleared!");	
			}
			yield break;

		}//- end PopulateDetails

		int GetPrototype(int k, int g)
		{
			//check key and group, if multiple instances then select randomly by relative weight within key/group
			int i,test = 0;
			float weightTotal = 0; 
			float check = 0;
			float rnd = UnityEngine.Random.value;
			float[] weights = (g == 0) ? treeWeights.ToArray() : detailWeights.ToArray();

			//is there only one instance in key?
			if (assetList [k] [g].Count < 2) 
			{
				if (g == 0)
					return test;
				else if (UnityEngine.Random.value < detailWeights [assetList [k] [g] [test]])
					return test;
			}

			// total the weights to establish weight ratios
			for(i = 0; i < assetList[k][g].Count; i++)
			{
				weightTotal += weights [assetList [k] [g] [i]];
			}				

			// return the first index whos relative weight is lower than the rnd value
			for (i = 0; i < assetList [k] [g].Count; i++) 
			{
				check += weights [assetList [k] [g] [i]] / weightTotal;
				if (rnd <= check)
					return i;
			}					

			return 999;

		}//- end GetPrototype

		int FindKey (Color test, Vector2 loc)
		{
			for (int i = 0; i < ImageColorKey.Count; i++) 
			{
				if (test == ImageColorKey [i])
					return i;
			}

			Debug.Log ("No match found for pixel: " + test + " @ " + loc);
				
			return 999;
		}//- end FindKey

		float[,,] Blur(float[,,] processed)
		{
			int iW = processed.GetLength(0);
			int iH = processed.GetLength(1);
			int iL = processed.GetLength(2);
			int xx, yy, x, y;
			int avgCount = 0;
			float avg = 0f;

			for (int layer = 0; layer < iL; layer++) 
			{
				if (textureBlurExclusions [layer])
					continue;
				
				//Horizontal Pass
				for (yy = 0; yy < iH; yy++) 
				{
					for (xx = 0; xx < iW; xx++) 
					{
						//Reset
						avg = 0;
						avgCount = 0;

						//Collect right
						for (x = xx; (x < xx + textureBlurSize && x < iW); x++) 
						{
							avg += processed [x, yy, layer];
							avgCount++;
						}

						//Collect left
						for (x = xx; (x > xx - textureBlurSize && x > 0); x--) 
						{
							avg += processed [x, yy, layer];
							avgCount++;
						}
							
						avg = avg / avgCount;

						for (x = xx; x < xx + textureBlurSize && x < iW; x++) 
						{
							processed [x, yy, layer] = avg;
						}
					}
				}

				//Vertical Pass
				for (xx = 0; xx < iW; xx++) 
				{
					for (yy = 0; yy < iH; yy++) 
					{
						//Reset
						avg = 0;
						avgCount = 0;

						//Over pixel

						for (y = yy; (y < yy + textureBlurSize && y < iH); y++) 
						{
							avg += processed [xx,y,layer];
							avgCount++;
						}
						//Under pixel

						for (y = yy; (y > yy - textureBlurSize && y > 0); y--) 
						{
							avg += processed [xx,y,layer];
							avgCount++;
						}

						avg = avg / avgCount;

						for (y = yy; y < yy + textureBlurSize && y < iH; y++) 
						{
							processed [xx, y, layer] = avg;
						}
					}
				}
			}
			return processed;
		}//- end Blur

		public void StartPopulateTextures()
		{
			if (pldReady)
				StartCoroutine (PopulateTextures ());
			else
				print ("Must be in play mode to initialize PLD script");
		}

		public void StartPopulateTrees()
		{
			if(pldReady)
				StartCoroutine (PopulateTrees ());
			else
				print ("Must be in play mode to initialize PLD script");
		}

		public void StartPopulateDetails()
		{
			if(pldReady)
				StartCoroutine (PopulateDetails ());
			else
				print ("Must be in play mode to initialize PLD script");
		}

		public void StartAll()
		{
			StartPopulateTextures ();
			StartPopulateTrees ();
			StartPopulateDetails ();
		}

		public void ClearTextures()
		{
			float[,,] maps = new float[terrainData.alphamapWidth,terrainData.alphamapHeight,terrainData.alphamapLayers];

			terrainData.SetAlphamaps (0, 0, maps);
			terrainData.RefreshPrototypes ();

			Debug.Log ("Textures Cleared!");
		}

		public void ClearTrees()
		{
			terrainData.treeInstances = new List<TreeInstance> ().ToArray ();
			Debug.Log ("Trees Cleared!");
		}

		public void ClearDetails()
		{
			for (int layer = 0; layer < terrainData.detailPrototypes.Length; layer++) 
			{
				terrainData.SetDetailLayer (0, 0, layer, new int[terrainData.detailHeight, terrainData.detailWidth]);
			}
			Debug.Log ("Details Cleared!");
		}

		public void ClearAll()
		{
			ClearTextures ();
			ClearTrees ();
			ClearDetails ();
		}

		bool ErrorCheck()
		{
			if (terrainData == null) 
			{
				Debug.LogError ("Set TerrainData in inspector!");
				return true;
			}

			if (keyImage == null) 
			{
				Debug.LogError ("Set LandcoverImage in inspector!");
				return true;
			}
			return false;
		}
			
// a method to find the leading int if the need arises to allow spaces or extrenuous marks in the public asset lists
//		int GetLeadingInt (string input)
//		{
//			return int.Parse (new string (input.Trim ().TakeWhile (c => char.IsDigit (c) || c == '.').ToArray));
//		}

	}//- end class
		
	//TODO
	//tryparse applications in processing method
	//Noise Generator Seamless perlin?}
	//Detail Density Maps
	//Alpha map for weights
	//Heightmap Manipulation based on keys


	//DONE
	//Added TreeDeath
	//Tree jitter rework, no longer negative
	//Tree Density now based on distance between trees (WARNING: can crash computer!)
	//Cleanup of Inspector
	//Redesign of GetPrototype and weight handling
	//break out "Add Object" to separate script
	//overhaul density based on float
	//create noise generator
	//toggle alphaMap density
	//Test GDAL process for geoTiff
	//ClearAsset Confirmation
	//ReadOnly var setups in editor {Tree Count; Key Image Resolution;


}

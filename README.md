**ProceduralLandcoverDresser//SUMMARY:**
 -This package utilizes images and color keys to populate textures, trees, and detail meshes on a Unity terrain. The script can add assets or use existing assets associated with the terrainData. The assets are "patched" to the keys through integer lists whos indicies correspond to the indicies of the color keys and whos comma separated contents correspond to the indicies of the assets present in the terrainData. 

**PLD//REQUIREMENTS:**

 -Unity5.3.xxx or later
 -Unity Terrain on an unique layer

**PLD//SETUP:**

 **Overview**
 
	1. Add Scripts
	2. Adjust Variables in PLD Script
	3. Assign Key Image
	4. Populate Lists
	5. Run Simulation
	6. Populate
	7. Save out Terrain/Script Objects
	8. CSVReader
	9. TerrainAssetInitializer
 
 **Section 1 \- Add Scripts**

	1.1 Add, create, or import a terrain object.

	1.2 Add the PLD script to a GameObject in your scene.
	- This could be the terrain object or any other object you wich to use to house the PLD scripts.

	1.3 (optional) Add the CSVReader to a GameObject in your scene.
	- If you intend to use a Comma Separated Value sheet to fill in your keys then plug it in here. -As of 0.0.0 it only reads floating point values into colors.

	1.4 (optional) Add the TerrainAssetInitializer to your terrain.
	- If you would like to save the assets you are adding to the terrain data then use the TerrainAssetInitializer script. 
	- Once setup, the GameObject housing the script can be exported as a package along with the assets used to make future importing easier. 
	- As of 0.0.0 this feature imports all assets with default settings which might require further customization in the terrain data interface.	

 **Section 2 \- Adjust Variables in PLD script**
 
	2.1 Assign Terrain Values
	- In the inspector for the GameObject housing the PLD script in your scene, link the terrain data to the script.
	
	2.2 Place Terrain in unique layer.
	- You will need to have a Layer dedicated to just the terrain which you can select from the drop down (you may need to create it).
	
**Section 3 \- Assign Key Image**

	3.1 Setup your key image texture in the Project window
	
	3.2 Assign the texture file to the 'Key Image' in the PLD inspector

**Section 4 \- Populate Lists**

	4.1 (optional) CSVReader
	- Use the CSVReader script to populate the PLD Image Color Key list from a file.
	- See Section 8 for CSVReader setup.
	
	4.2 (optional) Use TerrainAssetInitializer 
	- Use the TerrainAssetInitializer script to populate the terrain data with assets as generic prototypes
	- See Section 9 for TerrainAssetInitializaer setup
	
	4.3 Add and edit color keys in the PLD inspector

	4.4 Add TerrainData assets(e.g. textures, trees, grasses) to the terrain using the default Unity Terrain interface.
	- When adding textures, note that the first texture added (Element 0) will automatically cover the terrain.
	
	4.5 Assign prototypes to keys
	- Fill in the 'TextureAssets' list with integer values correlating to the index of the textures as they appear in the terrain data list.
	- Use only one integer per list field.
	- Each index represents the corresponding color key (e.g. TextureAsset Element4 is used wherever ImageColorKey Element4 is found).
	- The list's length should match the length of the Image Color Key list.
	
	4.6 Assign weights to prototypes.
	- Apply relative weight values to augment the distribution of Tree and Detail assets during population where there are more than one asset listed per key.
	- The length of these lists should match the number of prototypes visible in the terrain data.
	- Each element corresponds to the index of the prototype.
	- The number assigned is used for relative distribution, with the heigher number being distributed more frequently.

**Section 5 \- Run Simulation**

	5.1 Run the simulation
	- This initializes the asset lists and prepares the data to be populated
	
	5.2 Press a 'Populate' button in the PLD inspector
	- With the simulation running, choose one asset to be populated at a time
	- Once you've checked that each asset type populates as expected, you can make changes and repeat
	- For a final pass, use the 'Populate All' button (NOTE: Using the All button could take a while)

**Section 6 \- Save out Terrain/Script Objects**

**Section 8 \- CSVReader**

**Section 9 \- TerrainAssetInitializer**


Base
 -Add the Procedural Landcover Dresser script to the terrain you wish to populate.
 -In the inspector, link the terrainData and the reference image to be used.
 -Select the layer of the terrain in the attribute "Terrain Layer"
 -Assign Texture/Tree/Detail attributes according to preference (see demo)
 -Assign Landcover Image Key values which correspond to the pixel colors present in the Landcover Image. These RGB float values suppose a normalized 0-1 value. This can be populated from a CSV file (see CSVImageKeys section below).
 -If the intent is to store this setup as a template, use the "Add X" attributes to contain and assign textures,trees,grasses,and detail meshes to the scene. Once your assets have been placed in the appropriate lists, press the "Add Objects" button to push the lists into the TerrainData (does not require the simulation to be playing). The assets can then be further edited using the Unity terrain interface.
 -In the "x Assets" attributes, change the list size to reflect the number of keys in the Landcover Image Key list. Each element corresponds to a key. The contents of the elements are comma separated integers corresponding to the index of the asset to be deployed in that key. Example: to have the first two trees present in the terrainData to show up in key 4(starting from 0), the Element 4 of the Tree Assets attribute should read "0,1". This will populate the tree prototypes 0 and 1 wherever key 4 is found.

CSVImageKeys
 -If you are drawing your color key values from a CSV file, add the ProceduralLandcoverDresserCSVReader to your terrain object alongside the ProceduralLandcoverDresser script.
 -Place the CSV file you wish to parse into a "Resources" folder somewhere in the Unity Assets folder(you might have to make it). Naming of the folder is critical and case sensitive.  
 -In the inspector for the terrain object in your scene, under the CSVReader script, add the ProceduralLandcoverDresser script which is on said terrain to the "Pld" attribute.
 -Type the name of your file(without suffix) into the "File Name" attribute
 -Enter how many rows and columns of data there are
 -If rows/columns are to be skiped to get to the raw data (like skipping headers or labels) enter those in the "Skip X" attributes
 -Check the Image Key Reconciliation toggle (requires Landcover Image to be setup in ProceduralLandcoverDresser script) to ensure matching colors between the CSV and the provided key colors.
 -Press the Add ImageKey button (does not require simulation to be playing)

PopulatingAssets
 -To populate the assets onto the terrain, first play the simulation. This initializes the lists of assets and does an error check to ensure that necessary values are present. 
 -Then, press one of the "Populate X" buttons. After a brief pause, the assets should be populated onto the terrain in the appropriate key related areas.

**PLD//SAVING:**
 -If you would like to save your setup, you can add your terrain and/or script object to a prefab and export the prefab as a package. 

**PLD//NOTES:**
 -If you find that your assets are not populating in appropriate areas, add some solid color swatches to your terrain(RGBW provided in demo) and simplify your Texture Assets to just those swatch texture to get a clear picture of how your Landcover Image is being read onto the terrain. You may need to do some combination of rotations and horzontal/vertical flipping of your key image to have the data correlate to your terrain appropriately. In the demo you will notice a difference in the orientations of the key, texture, and heightmap images. The Fernan_FBFM40Key image is flipped horizontally and rotated 90deg compared to the Fernan_FBFM40Tex image(done in Photoshop). 

-------------------------------------------
This software is licensed under the BSD 3-Clause License

Copyright (c) 2015, Jacob Cooper (coop3683@outlook.com)
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, 
are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this 
   list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice, 
   this list of conditions and the following disclaimer in the documentation 
   and/or other materials provided with the distribution.

3. Neither the name of the copyright holder nor the names of its contributors 
   may be used to endorse or promote products derived from this software without 
   specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR 
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES 
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; 
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON 
ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (
INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS 
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

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
	6. Save out Terrain/Script Objects
	7. CSVReader
	8. TerrainAssetInitializer
 
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
	- See Section 7 for CSVReader setup.
	
	4.2 (optional) Use TerrainAssetInitializer 
	- Use the TerrainAssetInitializer script to populate the terrain data with assets as generic prototypes
	- See Section 8 for TerrainAssetInitializaer setup
	
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

	5.1 Run the simulation.
	- This initializes the asset lists and prepares the data to be populated.
	
	5.2 Press a 'Populate' button in the PLD inspector.
	- With the simulation running, choose one asset to be populated at a time.
	- Once you've checked that each asset type populates as expected, you can make changes and repeat.
	- For a final pass, use the 'Populate All' button (NOTE: Using the All button could take a while).
	
	5.3 Notes on the clear buttons.
	- Use the clear buttons to remove all of a single category or all terrain assets from the terrain.
	- These buttons can be pressed even if the simulation is not/has not been run.
	- There is a confirmation dialogue to prevent accedents.

**Section 6 \- Save out Terrain/Script Objects**

	6.1 Create a new prefab object to contain the modified terrain or script object.
	
	6.2 Export the prefab including any dependant assets and the terrain data.

**Section 7 \- CSVReader**

	7.1 Place the Comma Separated Value file in a case sensitive folder labeled 'Resources'.
	- The folder can exist anywhere in the project Assets folder.

	7.2 Assign variables in the inspector.
	- Create a link to the PLD script.
	- Fill in the Filename, excluding the suffix.
	- Input the total number of data rows and columns.
	- Input the number of rows and columns to skip (e.g. headers, descriptors, etc.).
	
	7.3 Press a button.
	- Use the 'Image Key Reconciliation' check to run a pass which will remove discrepencies between the Color32 list as read from the CSV file and the actual PLD key image pixel colors.
	- The 'Add ImageKey to PLD' button will push the data values into a Color32 list and copy it into the PLD script
	- The 'Create Swatches' button will create a 16px swatch texture using the colors in the color key. This is useful to visualize where key colors are when populated on the terrain.

**Section 8 \- TerrainAssetInitializer**

	8.1 Import all assets to be used into the project(must be present in Assets folder).
	- Be sure that all textures are set to 'Repeat'.
	
	8.2 Place this script on the terrain.
	- As of build 0.0.0 the script must be placed on the terrain. This is planned to be made independant.
	
	8.3 Fill the lists.
	- Add assets from the project folder to the TerrainAssetInitializer inspector.
	- All assets will be populated using generic setups. Individual modifications must be made after this script is run using the terrain inspector.
	- Assets will be added to the terrain data in the order they appear in the lists


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

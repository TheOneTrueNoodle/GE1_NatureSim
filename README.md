# Nature Simulation

Ryan O'Connor
C20409464

Erin Cooley
C20517416

# Description
Out project is designed around exploring the emergence of new environments generated from procedural generation, and how those environments can spread and grow. By combining simple behaviours with AI and state machines for the natural environment, we have attempted to simulate an infinite world that can changes based on the input of variables that affect terrain generation. 

## Video


## Screenshots
![image](https://user-images.githubusercontent.com/72693746/235679942-5e1d6824-2f53-414c-ae90-8fa9933b0ff3.png)
![image](https://user-images.githubusercontent.com/72693746/235680187-0f00855a-012f-41d6-ba9f-d773b8700b13.png)
![image](https://user-images.githubusercontent.com/72693746/235680857-16bfedef-d9c9-43ae-baec-5065cf6a12c9.png)

# Instructions
Fork and clone this repo to your computer to get access to the full project. Then you can open the scene: "Assets/Scenes/Terrain Menu Screen". When you press play you can change the settings for how the generation will be built and see what different results you can get, or if you wish to have more control over changes you can go to the "Assets/Terrain Assets" folder and edit the custom noise and terrain scriptable objects directly, as well as adding new regions or changing the Animation Curve that renders the differences in height. 

# How it works
## Terrain Generation
The Terrain Generation is a complex system spread across mulitple different scripts. The majority of the scripts directly involved in the terrain generation process is in the folder "Assets/Ryan Stuff/Scripts/New Landmass Generation" and there are some other scripts in the scripts folder that handles the element spawning. 

There are 2 major scripts that handle the Terrain generation. The R_EndlessTerrain script which procedurally generates the terrain infinitely and keeps track of all of the terrain meshes, and the R_MapGenerator script, which references several other static classes to generate each individual mesh used. If it was left simple like this however, it would have some major performance issues. These two scripts utilise multithreading to create a seperate thread for each meshes generation to keep the generation code off the main thread as it uses a lot of for loops to go through every vertice in each mesh and assign it the proper values. The R_MapGenerator Script references a number of scriptable objects which hold the data for the perlin noise generated and the terrain generation settings. 

In order to properly spawn the objects, I developed the R_NatureGenerator Script. This script has a function that is called by the R_EndlessTerrain script to assign new Elements, a custom data variable setup in the R_ElementClass script that holds the necessary information on each element, to each vertex on the mesh within set distances. It then uses random number generation and weighting to see whether something will actually spawn and what object will spawn. Before it runs it checks the specific mesh coordinate to see what type of TerrainType variable it is which then sees if this type of element can spawn at this location. This stops trees from spawning in the ocean and allows only rocks to spawn in the mountains.

## Creature AI
Erins work goes here

# List of classes/assets

| Class/asset | Source |

# References

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
For the creatures I wanted to bring them to life by rigging them with procedural animation, Using the animation rigging package allows for the monkey arms to use inverse kinematics and allows me to procedurally animate the monkey walk cycle using the IK foot solver script.

The Snake/Tower is quite simplistic compared to the monkeys, it mimics a regular tree, however if a monkey gets too close it will attempt to grab the monkey. once the monkey is grabbed the tree will bring the monkey to the very top of itself for consumption. once consumption has taken place the snake/tower will use the energy gained from the monkey to spread out its seeds and extend its life cycle. I wanted to base the Snake/Towers off lobsters so they do not age and simply die when starving.

The monkeys are much more advanced than the Snake/Tower with multiple states that also dictate their life cycle. In the beginning monkeys will appear as babies and learn to walk as they grow, visually getting much better at walking as late teens to adults. Once they reach adult-hood they have no time to waste and will  instantly attempt to find food in the "search for food state", if no food is available they will enter a wander to try and conserve hunger and enter an area with more food. If they find food however they will sit there and consume it, moving their arms to their mouth with the food. 

once they have enough energy stored they will find the closest monkey and attempt to mate (there were issues with monkeys mating with baby monkeys, this has been hard coded not to happen because that's gross) if the other monkey does not want to mate it will reject the first monkey, the first monkey will then enter a wander state to leave the second monkey alone. If BOTH monkeys decide to mate they have a chance to create offspring (25% - 0 50% - 1 25% - 2) and then will enter a wander state as they give their baby room to grow.

Due to the monkeys eating fruit and reproducing and snake/towers eating the monkeys the ecosystem will balance itself out, too many snakes and they'll have no more monkeys to eat, too many monkeys and there will be either no food left or the snake towers will start appearing and quickly explode in population

# List of classes/assets

| Class/asset  | Source | Written by |
| ------------- | ------------- | ------------- |
| R_ElementBaseState  | Self written | Ryan O'Connor  |
| R_ElementClass | Self written | Ryan O'Connor   |
| R_ElementDyingState  | Self written | Ryan O'Connor  |
| R_ElementGrowingState | Self written  | Ryan O'Connor  |
| R_ElementLivingState  | Self written  | Ryan O'Connor  |
| R_ElementNullState | Self written | Ryan O'Connor   |
| R_EndlessTerrain  | Modified from this series [Sebastian Lague](https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3)| Ryan O'Connor  |
| R_EternalRotation  | Self written | Ryan O'Connor   |
| R_FallOffGenerator  | Modified from this series [Sebastian Lague](https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3) | Ryan O'Connor   |
| R_Fruit  | Self written | Ryan O'Connor   |
| R_FruitTree  | Self written | Ryan O'Connor   |
| R_HideOnPlay  | Self written | Ryan O'Connor  |
| R_LandmassNoise  | Modified from this series [Sebastian Lague](https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3)  | Ryan O'Connor  |
| R_LoadGenerationSceneManager | Self written | Ryan O'Connor  |
| R_MainMenuVisualGenerator  | Self written | Ryan O'Connor |
| R_ManageMonkeysState | Self written | Ryan O'Connor |
| R_MapDisplay  | Modified from this series [Sebastian Lague](https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3)  | Ryan O'Connor |
| R_MapGenerator  | Modified from this series [Sebastian Lague](https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3) | Ryan O'Connor |
| R_MapGeneratorEditor | Modified from this series [Sebastian Lague](https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3)  | Ryan O'Connor |
| R_MeshGenerator  | Modified from this series [Sebastian Lague](https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3)  | Ryan O'Connor |
| R_MonkeyHouse | Self written | Ryan O'Connor |
| R_NatureGenerator  | Self written | Ryan O'Connor |
| R_NoiseData  | Modified from this series [Sebastian Lague](https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3) | Ryan O'Connor |
| R_Rock  | Self written | Ryan O'Connor |
| R_TerrainData  | Modified from this series [Sebastian Lague](https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3)  | Ryan O'Connor |
| R_TerrainReferenceData  | Self written | Ryan O'Connor |
| R_TextureGenerator | Modified from this series [Sebastian Lague](https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3)  | Ryan O'Connor |
| R_Tree | Self written | Ryan O'Connor |
| R_UpdatableData | Modified from this series [Sebastian Lague](https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3)  | Ryan O'Connor |
| R_UpdatableDataEditor | Modified from this series [Sebastian Lague](https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3) | Ryan O'Connor |
| R_UpdateSliderDisplayValue | Self written | Ryan O'Connor |
| SCR_MONKEY | Self Written | Erin Cooley |
| SCR_MonkeyAfraidState | Self Written | Erin Cooley |
| SCR_MonkeyBabyState | Self Written | Erin Cooley |
| SCR_MonkeyBaseState | Self Written | Erin Cooley |
| SCR_MonkeyDoNothing | Self Written | Erin Cooley |
| SCR_MonkeyEatState | Self Written | Erin Cooley |
| SCR_MonkeyMateState | Self Written | Erin Cooley |
| SCR_MonkeySearchForFoodState | Self Written | Erin Cooley |
| SCR_MonkeyStateManager | Self Written | Erin Cooley |
| SCR_MonkeyWanderState | Self Written | Erin Cooley |
| SCR_SnakeBaseState | Self Written | Erin Cooley |
| SCR_SnakeConsomueState | Self Written | Erin Cooley |
| SCR_SnakeEatState | Self Written | Erin Cooley |
| SCR_SnakeSeed | Self Written | Erin Cooley |
| SCR_SnakeStateManager | Self Written | Erin Cooley |
| SCR_SnakeWaitState | Self Written | Erin Cooley |
| SCR_Spectator | Self Written | Erin Cooley |
| MonkeyHead | Blender Base Mesh | Erin Cooley |
| IkFootSolver | Modified From Unity Official Procedural Animation Project | Erin Cooley |


# References
- A wonderful series on procedural terrain generation by [Sebastian Lague](https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3) which helped immensely for the terrain generation. 

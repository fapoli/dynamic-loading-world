# Dynamic Loading World
A Unity component that handles a grid of maps, and dynamically loads/unloads them as the player moves. It also fills the world with generic tiles and has tiling support for infinite worlds.

## Package Contents
The project is divided into 2 folders:

**Prefabs** contains the actual prefab (called DynamicLoadingWorld) that should be added to your scene.

**Scripts** contains the code for the component.

## Usage
Just add the DynamicLoadingWorld prefab into your scene and configure it acordingly. This are the settings:

**Locations:** You add maps and coordinates in the grid here. Be aware that the coordinates must be located within the boundaries of the defined grid (See *TileCountX* and *TileCountY*)
* **Prefab:** The GameObject that contains the level that will be positioned in the grid.
* **Position** A position in **GRID COORDINATES** for this level.

**Generic Prefabs:** You can add here some levels that are generic (for example, a forest, a forest with a lake, etc). If the *Fill Empty Locations* option is selected, the empty grid positions will be filled with one of this levels. They will even be rotated to generate more variation if the *Rotate Filled Locations* option is enabled.
* **Prefab**: The GameObject that contains the generic level that will be positioned in the grid.
* **Weight**: This is used to control the probability of aparition of this generic level.

**Generator Settings:** This controls the randomness of the level. 
* **Fill Empty Locations:** The Generic prefabs will be used to fill any space on the grid that doesn't have an assigned level.
* **Rotate Filled Locations:** The generic prefabs will be randomly rotated (by 90 degrees) in order to generate more variations.
* **Seed:** This controls the seed for the random generator. If it is 0, the generated grids will always be different. If not, they will be always the same. You can try different values to see diferent maps. The default value is *1*.

**Grid Settings:** This controls the size of the grid and spacing of the tiles.
* **Tile Count X:** The number of horizontal tiles that the grid will have.
* **Tile Count Y:** The number of vertical tiles that the grid will have.
* **Cell Size:** The size of each cell. This should match the size of your level prefabs, because it defines the spacing between them.
* **Is Infinite:** If this is enabled, the map will loop on the borders, creating the illusion of a circular infinite world.

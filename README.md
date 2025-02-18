# Wave Function Collapse Map Generation Algorithm App
This is an app that is made to implement the wave function collapse map generation algorithm. 
The user will be able to generate maps through creating a set of rules that the map generation will follow.

# The Algorithm

The algorithm works by selecting map tiles through a random selection from a set of possible tiles. Each tile type has a set of possilble tiles that can appear next to it. 
There are two maps; the potential map and the result map. The potential map is used to track which tile types can appear in which location, whereas the result map contains the final map. 
When placing a tile, the potential map is updated, so that the tiles that can appear in adjacent tiles account for the already placed tiles.

The tiles are placed in a random order, until the map has been completely filled.

The rules for which tile types can appear are stored as binary bits, and handled through binary operations.


# TODO
- Saving and loading maps
- More flexible color selection
- Some work on visuals
- See if the mobile build can be made work
- Look into weighing tile probability
  - Increase chance of a tile appearing based on adjacent tiles (Making the maps less chaotic)
- Ability to have different rules for north, south, east and west -directions

# Known Issues
- Unable to make a tile type not appear adjacent to itself

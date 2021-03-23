# Hex-Pathfinding-Prototype
This project is an A* pathfinding prototype that uses a randomly created hex map.

When running the project, it first generates a 15x15 map of hexagons. These hexes are randomly assigned a terrain type with a varying spawn chance.

Each hex can be one of 4 types:
1. Plains: Traversable at a cost of 1, Spawn chance of 50%
2. Hills: Traversable at a cost of 2, Spawn chance of 30%
3. Forrest: Traversable at a cost of 5, Spawn chance of 10%
4. Mountains: Not traversable, Spawn chance of 10%



Controls:

Select a capsule to move by either left-clicking it, or box-select it with left-click-drag.
Move any selected capsule to a non-Mountain hex by right-clicking the hex.
Move the camera with WASD.
Zoom in/out with the scroll wheel.



Preview!

![](Recordings/sample_run.gif)

CREDITS:
Custom min-heap implementation inspired by Sebastian Lague's implementation: https://github.com/SebLague/Pathfinding/tree/master/Episode%2004%20-%20heap


Built using Unity 2019.3.14f1

# Smart Mesh Combiner

This is just another tool for combine *(aka merge)* a selection of GameObjects in Unity into one.



### Why

Only for learning purposes. While I was working on the ImageLevel tool, I came with the idea of combining multiple tiles into one, for optimization. The less objects in a scene, the better will perform.



### Author

Just me.



### Current Features

- Marks the new merged object as *Static* GameObject, as we won't need to move the tilemap, right?



### Planned features

- Wizard that displays an option to override the current texture/sprite assigned in each selected tile.
- An option in the wizard to allow deletion *(or not)* of the previous selection.
- Not that many at the moment, really...



### Expansion possibilites (non-planned)

Create something similar to the native Tilemap *(Unity2D)* structure, with the *rule tile* thing, you know, the 9-Slice feature that knows exactly which texture goes to which corner of the tilemap...



### Usage

1. Select a group of GameObjects *(tiles)* you want to combine into one.
2. Use the tool from the menu to combine the selected objects. A new one will appear in the Scene.

*(**NOTE**: This tool is intended to be used with the ImageLevel tool, so expect weird results when combining other GameObjects)*

*(**WARNING**: It may break your tilemaps if they're not sharing the same exact texture for every tile within the selection)*
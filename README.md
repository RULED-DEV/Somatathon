unity game project somatathon from 2025 (completed and published). somatathon is a first person puzzle game where the player casts spells using their
fingers in an attempt to reach door 783 while 2 enemies work to stop the player. the first enemy(jozzo) casts spells to move the player away from the door
preventing their victory and the other(maurice) persistently chases the player to kill them.

requirements :
  - OS that can run unity editor.
  - unity installed on machine to run the editor.
  - a unity version to run the project in.

install instructions :
    - download repository and unzip.
    - open unity hub and click add then add project from disk.
    - navigate and select unzipped repository.
    - you will be prompted for a version to open it in, selecting missing version or latest LTS version will work best.
    - wait for editor to load and enjoy.

gameplay notes :

player can move(WASD) and look around(mouse), the player can articulate the fingers on their left and right hand(LMB/RMB) and cast a spell(space).
the player can also place and retrieve their totem(R) and open the menu/tome(tab).

casting/spells : 
  - the main gameplay feature is spells, the player has 3 fingers per hand and each finger can be curled or uncurled, depending on the total state
    of all the fingers per hand an effect or object is chosen, combining an effect and an object applies the effect to the object for example
    moving the player closer to an object or moving the object further from the player.

  - among the spells the player has access to their is 2 unique ones labelled as 'cast' and 'down' in the menu/tome, these need to be paired together
    to work and are effectively the players interact button, its primary use is that when the player reaches door 783 they are meant to use it to
    break through the door and reach jozzo where it can be used again to beat the game.

  - the player has a totem which is one of teh objects that can be targeted by the player, this is important as if the player selects an object in
    both hands (using their fingers) they will teleport one to another allowing for the player to dynamically create a point on the map to teleport to.

  - the intention of this system is that the player has access to a wide variety of methods to reach their goal, allowing players to experiment
    with what they like to complete the game.

enemies (jozzo and maurice) : 
  - maurice has a simple AI as all it does is chase the player down the corridor until it reaches and kills them, this applies pressure to the
    player preventing them from being able to just stand still all the time without repercussion.

  - jozzo has a more complex AI, it calculates how far the player has moved along the corridoor at regular intervals and determines if the player is
    making rapid progress toward door 783, if so it will either teleport the player to the otehr end of teh corridoor or just teleport the player
    away depending on proximity. if the player is not moving toward the door jozzo will teleport maurice to be closer to the player, applying more
    pressure as such.

please direct all inquiries, questions and problems to ruled.dev@gmail.com

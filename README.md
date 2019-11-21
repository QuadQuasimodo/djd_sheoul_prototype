# Sheoul Dev Doc

## Fire 

The basic Fire is stored in a prefab called "Fire", the fire that is supposed to be the Player's torch is the prefab "PlayerFire" which is a _variant_ of the original "Fire" prefab.

The Materials used for the fire are the "Blaze" and "Fire" Materials for the trails.

The particle Renderer on both the Fire and FireBlaze particle systems only renders the trails of the particles.

## Problems

* The light goes trough objects(?) it illuminates the ground outside the box if a light is put near the wall.

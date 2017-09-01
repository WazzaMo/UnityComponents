# Spawner Component

## Purpose
Makes the prefab, selected in the Inspector, spawn at the location of the GameObject
that has the Spawner component.

## Inspector Values
| Setting | Purpose                          |
|---------|----------------------------------|
|Character Prefab To Spawn | A slot that takes a game object to use as a template or Prefab instance in the project assets |

## Methods
| Method | Description                          |
|---------|----------------------------------|
| `void TriggerSpawn()` | Causes the prefab to be instantiated. |
| `GameObject SpawnCharacter()` | Makes the instance and returns it to the caller. |

### Copyright
(c) Copyright 2017 Warwick Molloy, may be used under MIT License terms.
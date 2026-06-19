# Test task "Magic Tower"

> [!NOTE]
> Project implemented and checked in **Unity 6.3**

<details>
<summary>Task details</summary>

Your task is to develop a captivating prototype of a game where a brave Magical Tower stands firm against an onslaught of relentless foes. The Tower's arsenal of magical spells is its last defense against the encroaching evil minions. Will its magical prowess be enough to withstand the ceaseless onslaught?  
   
**Core Features to implement:**

1. The game is in 3D.   
2. A **Magical Tower** with a health bar is positioned in the middle of the screen. Upon reaching zero health points, the game is over.  
3. **Enemies**. Primitive enemies that are spawning outside of the screen and walk toward the Tower. Upon reaching the Tower, they start to deal damage to it. Type of enemies to implement:  
   a. **Default Enemy**, which goes towards the tower with straight movement  
   b. **Fast Enemy**, which has the same behavior as the default one, but moves faster  
   c. **Big and Slow Enemy** is larger visually, has more health points, and moves slowly.  
4. **Enemy spawn system.** The system would allow setting the spawn rate of enemies for different time periods, enabling gradually increased game difficulty over time. Feel free to balance it as you wish.  
5. **Magic Spells**. The tower uses spells to shoot projectiles at the enemies that are approaching it. Each spell has its cooldown, damage, and projectile fly speed parameters. You would need to implement 2 spells:  
   a. **Fireball Spell**. Spawn a single Fireball projectile that flies toward a random enemy direction and upon colliding with the enemy or ground \- explodes, deals damage in the area and applies **Burning Effect,** that deals damage over time.  
   b. **Barrage Spell**. Spawn a small projectile for every enemy target visible on the screen. Each projectile flies toward its target and deals a small amount of damage to a single target. They fly in a **parabolic trajectory**.  
6. **Damage UI**. Whenever damage is dealt to enemies or the Tower, we need to spawn text with damage numbers.

**Requirements:**

1. The Enemies and Spells system should be easily **expandable and configurable.**

**Evaluation:** 

1. Visuals could be made via primitive objects and primitive particles. Having good and polished visuals **is not required** for task completion.   
2. The main focus of the evaluation would be on reviewing **the architecture and expandability** of the solution.

**Time requirements:**

1. The task should take around 2-3 hours to implement. 

**AI Usage:**  
	It is fine to use AI, but leaving any AI leftovers or slop code would be treated as a part of your decisions and codestyle and would affect the evaluation.

**References:**

1. The closest game references are “Vampire Survivor-like” games (Vampire Survivor, Soulstone Survivor, etc.).  
2. The final game can look similar to this:
<img width="616" height="350" alt="MagicalTower" src="https://github.com/user-attachments/assets/69e95c9c-3a7c-438f-90fe-1b66963b0c6a" />

Please upload the final project to a **git repository** (**GitHub**, **Bitbucket**) and share the link with us, so the reviewer can clone the project and check it out.
</details>

## 
There is  Only one scene to run: `Assets/Scenes/Main.unity`. Enemies spawn around the edge and walk in; click the
**Fireball** / **Barrage** buttons at the bottom to cast (each button shows its cooldown reloading).
The game ends when the tower's health hits zero — a panel with a **Restart** button appears.

## Disclaimer
Here was no strict requirements about way for spells casting. I decided to add UI buttons for each spell 
just because it looks for me more natural.

## Demo video
https://github.com/user-attachments/assets/1ebf1b1a-b6e2-4cfb-be1e-09e886be0169

## Core ideas
- **Zenject.** I used Zenject as composition root to manage dependencies and for object pooling.
  Zenject signals is not used, because I prefer is not to use it if possible.
- **Composition root.** `Installers/GameInstaller` (a `MonoInstaller` on the scene's Zenject `SceneContext`)
  binds every system and wires the scene references, prefabs and configs assigned in the inspector.
- **Pooling.** Enemies, both projectile types and damage numbers are recycled through Zenject
  `MonoMemoryPool`s — nothing gameplay-related is `Instantiate`/`Destroy`d at runtime.
- **Data-driven via ScriptableObjects.** Enemy types, spells, the spawn curve and tower stats are
  `*Config` assets in `Assets/Game/Configs`. Tuning or adding content means editing/creating assets.

## Systems (`Assets/Scripts/<Folder>`)
- **Core** — basic entities`Health`, `IDamageable`/`IEnemyTarget`, `DamageInfo`,
  `GameController` (Game Loop manager).
- **Enemies** — one generic `Enemy` prefab; "Default / Fast / Big & Slow" are just `EnemyConfig` assets
  (HP, speed, scale, colour, damage). Movement is a swappable `[SerializeReference] IEnemyMovement`
  strategy (`StraightMovement`). `EnemyRegistry` tracks live enemies so spells can find targets.
- **Spawning** — `SpawnConfig` is a timeline of weighted phases (difficulty ramp); `EnemySpawner`
  reads it and spawns from the enemy pool on a ring around the tower.
- **Spells** — `SpellConfig` (abstract SO) builds a runtime `ISpell`; `SpellBase` owns the cooldown and
  the ready/cast gate. `SpellCaster` advances cooldowns each frame.
- **Projectiles** — pooled `FireballProjectile` (ballistic, explodes on impact/ground) and
  `BarrageProjectile` (parabolic, homes to one target).
- **Effects** — `IStatusEffect` list ticked per enemy via `StatusEffectHandler` (`BurningEffect` DoT).
- **UI** — `SpellBarUI` spawner for SpellButtons; `SpellButtonView` casts on click and
  shows cooldown as a radial fill. `TowerHealthBarUI`, `GameOverUI`, and pooled `FloatingDamageText`
  (world-space damage numbers).

Everything spawned at runtime comes from prefabs in
`Assets/Game/Prefabs` via the pools.

## Extending it
- **New enemy:** create an `EnemyConfig` asset and add it (with a weight) to a `SpawnConfig` phase.
- **New spell:** add a `SpellConfig` subclass + a `SpellBase` subclass, then list the config in
  `GameInstaller` — its cast button appears automatically.
- **New behaviour:** implement `IEnemyMovement` (movement strategy, but now it's only dummy) or `IStatusEffect` (slow, poison, …) — no
  changes to existing types required.

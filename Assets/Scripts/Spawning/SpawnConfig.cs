using System.Collections.Generic;
using UnityEngine;
using MagicalTower.Enemies;

namespace MagicalTower.Spawning
{
    /// <summary>Weighted reference to an enemy type within a spawn phase</summary>
    [System.Serializable]
    public class EnemySpawnWeight
    {
        public EnemyConfig Enemy;
        [Min(0f)] public float Weight = 1f;
    }

    /// <summary>One segment of the difficulty curve: when it begins, how fast it spawns, and what</summary>
    [System.Serializable]
    public class SpawnPhase
    {
        public string Name = "Phase";

        [Tooltip("Elapsed seconds at which this phase becomes active.")]
        public float StartTime = 0f;

        [Tooltip("Seconds between spawns during this phase.")]
        public float SpawnInterval = 2f;

        public List<EnemySpawnWeight> Enemies = new();
    }

    /// <summary>
    /// Time-based difficulty curve
    /// </summary>
    [CreateAssetMenu(menuName = "MagicalTower/Spawn Config", fileName = "SpawnConfig")]
    public class SpawnConfig : ScriptableObject
    {
        [Tooltip("Phases ordered by StartTime")]
        public List<SpawnPhase> Phases = new();

        [Tooltip("Distance from the tower at which enemies appear")]
        public float SpawnRadius = 20f;
        
        public SpawnPhase GetPhase(float elapsed)
        {
            SpawnPhase current = null;
            for (int i = 0; i < Phases.Count; i++)
            {
                if (elapsed >= Phases[i].StartTime) 
                    current = Phases[i];
                else
                    break;
            }
            return current ?? (Phases.Count > 0 ? Phases[0] : null);
        }

        public EnemyConfig PickEnemy(SpawnPhase phase)
        {
            if (phase == null || phase.Enemies.Count == 0) 
                return null;

            float total = 0f;
            foreach (EnemySpawnWeight e in phase.Enemies) 
                total += Mathf.Max(0f, e.Weight);
            
            if (total <= 0f) 
                return phase.Enemies[0].Enemy;

            float roll = Random.Range(0f, total);
            foreach (EnemySpawnWeight e in phase.Enemies)
            {
                roll -= Mathf.Max(0f, e.Weight);
                
                if (roll <= 0f) 
                    return e.Enemy;
            }
            
            return phase.Enemies[phase.Enemies.Count - 1].Enemy;
        }
    }
}

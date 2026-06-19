using UnityEngine;

namespace MagicalTower.Towers
{
    /// <summary>
    /// Data-driven tower stats. Kept as an asset (like EnemyConfig / SpawnConfig / SpellConfig) so
    /// designers can tune the tower without touching code or the scene
    /// </summary>
    [CreateAssetMenu(menuName = "MagicalTower/Tower Config", fileName = "TowerConfig")]
    public class TowerConfig : ScriptableObject
    {
        public float MaxHealth = 150f;
    }
}

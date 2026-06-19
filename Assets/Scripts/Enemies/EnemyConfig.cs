using UnityEngine;
using MagicalTower.Enemies.Movement;

namespace MagicalTower.Enemies
{
    /// <summary>
    /// Data-driven definition of an enemy type. "Default", "Fast" and "Big &amp; Slow" are just three
    /// assets of this type — a new enemy type is a new asset, no code required.
    /// </summary>
    [CreateAssetMenu(menuName = "MagicalTower/Enemy Config", fileName = "EnemyConfig")]
    public class EnemyConfig : ScriptableObject
    {
        [Header("Identity")]
        public string DisplayName = "Enemy";

        [Header("Stats")]
        public float MaxHealth = 30f;
        public float MoveSpeed = 2f;

        [Tooltip("Damage dealt to the tower per attack once in range.")]
        public float AttackDamage = 5f;

        [Tooltip("Seconds between attacks once in range.")]
        public float AttackInterval = 1f;

        [Header("Visual")]
        public float Scale = 1f;
        public Color Color = Color.red;

        [Header("Behaviour")]
        [Tooltip("Movement strategy. Swap for any IEnemyMovement implementation in the inspector.")]
        [SerializeReference] public IEnemyMovement Movement = new StraightMovement();
    }
}

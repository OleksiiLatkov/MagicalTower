using UnityEngine;

namespace MagicalTower.Enemies.Movement
{
    /// <summary>
    /// Strategy describing how an enemy approaches its target. Selected per <see cref="EnemyConfig"/>
    /// via <c>[SerializeReference]</c>, so new movement patterns are added without touching the enemy.
    /// </summary>
    public interface IEnemyMovement
    {
        void Tick(Transform self, Vector3 target, float speed, float deltaTime);
    }
}

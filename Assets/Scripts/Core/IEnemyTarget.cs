using UnityEngine;
using MagicalTower.Effects;

namespace MagicalTower.Core
{
    /// <summary>
    /// The contract spells/projectiles use to interact with an enemy without depending on the
    /// concrete <c>Enemy</c> type. Combines targeting, damage and status-effect application.
    /// </summary>
    public interface IEnemyTarget : IDamageable
    {
        Vector3 Position { get; }
        bool IsAlive { get; }
        
        void ApplyStatus(IStatusEffect effect);
    }
}

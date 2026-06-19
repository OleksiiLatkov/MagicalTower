using MagicalTower.Core;

namespace MagicalTower.Effects
{
    /// <summary>
    /// A timed effect applied to an enemy (damage-over-time, slow, stun, ...)
    /// </summary>
    public interface IStatusEffect
    {
        /// <summary>Identity used to refresh/replace effects of the same kind instead of stacking forever</summary>
        string Id { get; }

        void OnApply(IEnemyTarget target);

        /// <summary>Advance the effect. Return <c>true</c> while still active, <c>false</c> when finished</summary>
        bool Tick(float deltaTime);
    }
}

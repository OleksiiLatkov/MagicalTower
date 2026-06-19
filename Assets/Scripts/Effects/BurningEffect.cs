using UnityEngine;
using MagicalTower.Core;

namespace MagicalTower.Effects
{
    /// <summary>
    /// Damage-over-time effect applied by the Fireball spell. Deals <c>damagePerSecond</c> spread
    /// across discrete ticks for <c>duration</c> seconds.
    /// </summary>
    public class BurningEffect : IStatusEffect
    {
        public string Id => "burning";

        private readonly float _damagePerSecond;
        private readonly float _tickInterval;
        private float _remaining;
        private float _tickTimer;
        private IEnemyTarget _target;

        public BurningEffect(float damagePerSecond, float duration, float tickInterval = 0.5f)
        {
            _damagePerSecond = damagePerSecond;
            _remaining = duration;
            _tickInterval = Mathf.Max(0.05f, tickInterval);
        }

        public void OnApply(IEnemyTarget target) => _target = target;

        public bool Tick(float dt)
        {
            if (_target == null || !_target.IsAlive) 
                return false;

            _remaining -= dt;
            _tickTimer += dt;

            if (_tickTimer >= _tickInterval)
            {
                float damage = _damagePerSecond * _tickTimer;
                _tickTimer = 0f;
                _target.TakeDamage(new DamageInfo(damage, _target.Position + Vector3.up, DamageKind.Burning));
            }

            return _remaining > 0f;
        }
    }
}

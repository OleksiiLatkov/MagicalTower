using UnityEngine;
using MagicalTower.Core;
using MagicalTower.Enemies;
using MagicalTower.Projectiles;
using MagicalTower.Towers;

namespace MagicalTower.Spells
{
    /// <summary>
    /// Picks a random enemy and lobs a single exploding fireball at it. On impact it deals area damage
    /// and applies a burning damage-over-time effect.
    /// </summary>
    public class FireballSpell : SpellBase
    {
        private readonly FireballSpellConfig _config;
        private readonly EnemyRegistry _registry;
        private readonly FireballProjectile.Pool _pool;
        private readonly Tower _tower;

        public override string Name => _config.DisplayName;

        public FireballSpell(FireballSpellConfig config, EnemyRegistry registry,
            FireballProjectile.Pool pool, Tower tower) : base(config.Cooldown)
        {
            _config = config;
            _registry = registry;
            _pool = pool;
            _tower = tower;
        }

        protected override bool TryCast()
        {
            IEnemyTarget target = _registry.GetRandom();
            if (target == null) return false;

            _pool.Spawn(new FireballLaunch
            {
                Start = _tower.Position + Vector3.up * 2.5f,
                Target = target.Position,
                Speed = _config.ProjectileSpeed,
                AreaDamage = _config.Damage,
                AreaRadius = _config.AreaRadius,
                BurnDamagePerSecond = _config.BurnDamagePerSecond,
                BurnDuration = _config.BurnDuration,
                ExplosionVfx = _config.ExplosionVfx
            });
            return true;
        }
    }
}

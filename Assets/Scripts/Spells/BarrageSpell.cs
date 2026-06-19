using System.Collections.Generic;
using UnityEngine;
using MagicalTower.Core;
using MagicalTower.Enemies;
using MagicalTower.Projectiles;
using MagicalTower.Towers;

namespace MagicalTower.Spells
{
    /// <summary>
    /// Fires one small parabolic bolt at every enemy currently visible on screen, each dealing a small
    /// amount of single-target damage.
    /// </summary>
    public class BarrageSpell : SpellBase
    {
        private readonly BarrageSpellConfig _config;
        private readonly EnemyRegistry _registry;
        private readonly BarrageProjectile.Pool _pool;
        private readonly Tower _tower;
        private readonly Camera _camera;
        private readonly List<IEnemyTarget> _visible = new();

        public override string Name => _config.DisplayName;

        public BarrageSpell(BarrageSpellConfig config, EnemyRegistry registry,
            BarrageProjectile.Pool pool, Tower tower, Camera camera) : base(config.Cooldown)
        {
            _config = config;
            _registry = registry;
            _pool = pool;
            _tower = tower;
            _camera = camera;
        }

        protected override bool OnCast()
        {
            _registry.GetVisible(_camera, _visible);
            if (_visible.Count == 0) 
                return false;

            Vector3 origin = _tower.Position + Vector3.up * 2.5f;
            foreach (IEnemyTarget target in _visible)
            {
                _pool.Spawn(new BarrageLaunch
                {
                    Start = origin,
                    Target = target,
                    Speed = _config.ProjectileSpeed,
                    Damage = _config.Damage,
                    ArcHeight = _config.ArcHeight
                });
            }
            return true;
        }
    }
}

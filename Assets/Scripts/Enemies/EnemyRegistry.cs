using System.Collections.Generic;
using UnityEngine;
using MagicalTower.Core;

namespace MagicalTower.Enemies
{
    /// <summary>
    /// Tracks every live enemy so spells can query for targets without scanning the scene.
    /// Bound <c>AsSingle</c>; enemies register on spawn and unregister on death.
    /// </summary>
    public class EnemyRegistry
    {
        private readonly List<IEnemyTarget> _active = new();

        public IReadOnlyList<IEnemyTarget> Active => _active;
        public int Count => _active.Count;

        public void Register(IEnemyTarget enemy)
        {
            if (!_active.Contains(enemy)) _active.Add(enemy);
        }

        public void Unregister(IEnemyTarget enemy) => _active.Remove(enemy);

        public IEnemyTarget GetRandom()
            => _active.Count == 0 ? null : _active[Random.Range(0, _active.Count)];

        /// <summary>Fills <paramref name="results"/> with enemies currently inside the camera frustum.</summary>
        public void GetVisible(Camera camera, List<IEnemyTarget> results)
        {
            results.Clear();
            if (camera == null) return;

            for (int i = 0; i < _active.Count; i++)
            {
                IEnemyTarget enemy = _active[i];
                if (enemy == null || !enemy.IsAlive) continue;

                Vector3 vp = camera.WorldToViewportPoint(enemy.Position);
                if (vp.z > 0f && vp.x >= 0f && vp.x <= 1f && vp.y >= 0f && vp.y <= 1f)
                    results.Add(enemy);
            }
        }
    }
}

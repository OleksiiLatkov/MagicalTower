using UnityEngine;

namespace MagicalTower.UI
{
    /// <summary>Default <see cref="IFloatingTextService"/> implementation backed by a pool of popups.</summary>
    public class FloatingTextService : IFloatingTextService
    {
        private readonly FloatingDamageText.Pool _pool;

        public FloatingTextService(FloatingDamageText.Pool pool) => _pool = pool;

        public void Show(Vector3 worldPosition, float amount, Color color)
            => _pool.Spawn(worldPosition, amount, color);
    }
}

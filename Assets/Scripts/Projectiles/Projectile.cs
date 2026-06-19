using UnityEngine;
using Zenject;

namespace MagicalTower.Projectiles
{
    /// <summary>Base for pooled projectiles. Holds its pool so it can return itself when finished.</summary>
    public abstract class Projectile : MonoBehaviour
    {
        private IMemoryPool _pool;

        public void SetPool(IMemoryPool pool) => _pool = pool;

        protected void Despawn()
        {
            if (_pool != null) 
                _pool.Despawn(this);
            else 
                Destroy(gameObject);
        }
    }
}

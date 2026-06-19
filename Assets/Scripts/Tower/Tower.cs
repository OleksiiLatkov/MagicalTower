using UnityEngine;
using Zenject;
using MagicalTower.Core;
using MagicalTower.UI;

namespace MagicalTower.Towers
{
    /// <summary>
    /// The central tower the player defends. Authored in the scene (not spawned at runtime). When its
    /// <see cref="Health"/> reaches zero it raises <see cref="Health.Died"/>, which the
    /// <c>GameController</c> turns into the game-over flow.
    /// </summary>
    public class Tower : MonoBehaviour, IDamageable
    {
        private IFloatingTextService _floatingText;

        public Health Health { get; private set; }
        public Vector3 Position => transform.position;

        [Inject]
        public void Construct(IFloatingTextService floatingText, TowerConfig config)
        {
            _floatingText = floatingText;
            Health = new Health(config.MaxHealth);
        }

        public void TakeDamage(in DamageInfo info)
        {
            if (Health == null || !Health.IsAlive) 
                return;

            Health.TakeDamage(info.Amount);
            _floatingText?.Show(transform.position + Vector3.up * 2f, info.Amount, new Color(1f, 0.3f, 0.3f));
        }
    }
}

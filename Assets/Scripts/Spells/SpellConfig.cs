using UnityEngine;
using Zenject;

namespace MagicalTower.Spells
{
    /// <summary>
    /// Base data for a spell (cooldown, damage, projectile speed, projectile prefab). A new spell type
    /// is a new subclass plus a matching <see cref="ISpell"/> implementation.
    /// </summary>
    public abstract class SpellConfig : ScriptableObject
    {
        [Header("Common")]
        public string DisplayName = "Spell";

        [Min(0.05f)] public float Cooldown = 2f;
        public float Damage = 10f;
        public float ProjectileSpeed = 12f;

        /// <summary>
        /// Builds the runtime spell, letting Zenject inject its dependencies (projectile pool, enemy
        /// registry, camera, tower) while passing this config as an explicit argument.
        /// </summary>
        public abstract ISpell CreateRuntime(DiContainer container);
    }
}

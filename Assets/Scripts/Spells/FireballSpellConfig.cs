using UnityEngine;
using Zenject;

namespace MagicalTower.Spells
{
    [CreateAssetMenu(menuName = "MagicalTower/Spells/Fireball", fileName = "FireballSpellConfig")]
    public class FireballSpellConfig : SpellConfig
    {
        [Header("Fireball")]
        [Tooltip("Radius of the area-of-effect explosion. Uses Damage as the area damage.")]
        public float AreaRadius = 3f;

        public float BurnDamagePerSecond = 4f;
        public float BurnDuration = 3f;
        public GameObject ExplosionVfx;

        public override ISpell CreateRuntime(DiContainer container)
            => container.Instantiate<FireballSpell>(new object[] { this });
    }
}

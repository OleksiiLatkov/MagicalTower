using UnityEngine;
using Zenject;

namespace MagicalTower.Spells
{
    [CreateAssetMenu(menuName = "MagicalTower/Spells/Barrage", fileName = "BarrageSpellConfig")]
    public class BarrageSpellConfig : SpellConfig
    {
        [Header("Barrage")]
        [Tooltip("Peak height of each bolt's parabolic arc.")]
        public float ArcHeight = 4f;

        public override ISpell CreateRuntime(DiContainer container)
            => container.Instantiate<BarrageSpell>(new object[] { this });
    }
}

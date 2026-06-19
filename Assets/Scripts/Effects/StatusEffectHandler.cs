using System.Collections.Generic;
using MagicalTower.Core;

namespace MagicalTower.Effects
{
    /// <summary>
    /// Owns the active status effects on a single enemy and ticks them each frame. Re-applying an
    /// effect of the same <see cref="IStatusEffect.Id"/> refreshes it rather than stacking endlessly.
    /// </summary>
    public class StatusEffectHandler
    {
        private readonly List<IStatusEffect> _effects = new();
        private readonly IEnemyTarget _owner;

        public StatusEffectHandler(IEnemyTarget owner) => _owner = owner;

        public void Apply(IStatusEffect effect)
        {
            _effects.RemoveAll(e => e.Id == effect.Id);
            effect.OnApply(_owner);
            _effects.Add(effect);
        }

        public void Tick(float dt)
        {
            for (int i = _effects.Count - 1; i >= 0; i--)
            {
                if (!_effects[i].Tick(dt))
                    _effects.RemoveAt(i);
            }
        }

        public void Clear() => _effects.Clear();
    }
}

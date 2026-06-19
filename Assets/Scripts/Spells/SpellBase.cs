using UnityEngine;

namespace MagicalTower.Spells
{
    /// <summary>
    /// Handles cooldown timing and the ready/cast gate, so concrete spells only implement their actual effect 
    /// </summary>
    public abstract class SpellBase : ISpell
    {
        private readonly float _cooldown;
        private float _timer;

        protected SpellBase(float cooldown)
        {
            _cooldown = cooldown;
            _timer = 0f;
        }

        public abstract string Name { get; }

        public bool IsReady => _timer <= 0f;

        public float CooldownNormalized => _cooldown <= 0f ? 0f : Mathf.Clamp01(_timer / _cooldown);

        public void Tick(float dt)
        {
            if (_timer > 0f) 
                _timer -= dt;
        }

        public bool TryCast()
        {
            if (_timer > 0f) 
                return false;   // still on cooldown
            
            if (!OnCast()) 
                return false;     // nothing happened (e.g. no valid target) — stay ready
            
            _timer = _cooldown;
            
            return true;
        }

        /// <summary>Performs the actual cast. Return true if it fired, false to leave the spell ready.</summary>
        protected abstract bool OnCast();
    }
}

namespace MagicalTower.Spells
{
    /// <summary>
    /// Handles cooldown timing so concrete spells only implement targeting + projectile spawning in
    /// <see cref="TryCast"/>.
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

        public void Tick(float dt)
        {
            if (_timer > 0f) _timer -= dt;
            if (_timer > 0f) return;

            // Only consume the cooldown if a cast actually happened (e.g. a valid target existed).
            if (TryCast()) _timer = _cooldown;
        }

        protected abstract bool TryCast();
    }
}

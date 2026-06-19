namespace MagicalTower.Spells
{
    /// <summary>
    /// A spell cast manually (from the UI). It owns its cooldown: <see cref="Tick"/> advances the
    /// timer and <see cref="TryCast"/> fires it when ready. Cooldown state is exposed for UI feedback.
    /// </summary>
    public interface ISpell
    {
        string Name { get; }

        /// <summary>True when off cooldown and able to cast.</summary>
        bool IsReady { get; }

        /// <summary>Remaining cooldown as 0..1 (1 right after a cast, 0 when ready) — for fill overlays.</summary>
        float CooldownNormalized { get; }

        /// <summary>Remaining cooldown in seconds.</summary>
        float CooldownRemaining { get; }

        /// <summary>Advances the cooldown timer. Does NOT cast.</summary>
        void Tick(float deltaTime);

        /// <summary>Attempts to cast now. Returns true if it actually fired (ready and had an effect).</summary>
        bool TryCast();
    }
}

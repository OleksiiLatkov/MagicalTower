namespace MagicalTower.Spells
{
    /// <summary>
    /// Spell owns its cooldown to fire when ready. Cooldown state is exposed for UI feedback.
    /// </summary>
    public interface ISpell
    {
        string Name { get; }

        /// <summary>True when off cooldown and able to cast.</summary>
        bool IsReady { get; }

        /// <summary>Remaining cooldown as 0..1 (1 right after a cast, 0 when ready) — for fill overlays.</summary>
        float CooldownNormalized { get; }

        /// <summary>Advances the cooldown timer. Does NOT cast.</summary>
        void Tick(float deltaTime);

        /// <summary>Attempts to cast now. Returns true if it actually fired (ready and had an effect).</summary>
        bool TryCast();
    }
}

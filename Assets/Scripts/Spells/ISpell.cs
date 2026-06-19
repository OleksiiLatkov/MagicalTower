namespace MagicalTower.Spells
{
    /// <summary>A spell ticked by the <see cref="SpellCaster"/>; it owns its cooldown and casting logic.</summary>
    public interface ISpell
    {
        string Name { get; }
        void Tick(float deltaTime);
    }
}

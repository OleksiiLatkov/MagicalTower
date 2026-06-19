namespace MagicalTower.Core
{
    /// <summary>Anything that can receive damage (the tower, enemies, future destructibles).</summary>
    public interface IDamageable
    {
        void TakeDamage(in DamageInfo info);
    }
}

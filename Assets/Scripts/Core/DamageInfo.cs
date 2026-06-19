using UnityEngine;

namespace MagicalTower.Core
{
    /// <summary>Categorises a hit so presentation (e.g. damage-number colour) can react to it</summary>
    public enum DamageKind
    {
        Normal,
        Explosion,
        Burning
    }

    /// <summary>Immutable description of a single instance of damage being dealt</summary>
    public readonly struct DamageInfo
    {
        public readonly float Amount;
        public readonly Vector3 Point;
        public readonly DamageKind Kind;

        public DamageInfo(float amount, Vector3 point, DamageKind kind = DamageKind.Normal)
        {
            Amount = amount;
            Point = point;
            Kind = kind;
        }
    }
}

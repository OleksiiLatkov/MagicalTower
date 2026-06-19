using System;
using UnityEngine;

namespace MagicalTower.Core
{
    /// <summary>
    /// health container shared by the tower and enemies. 
    /// </summary>
    public class Health
    {
        public float Max { get; private set; }
        public float Current { get; private set; }
        public bool IsAlive => Current > 0f;

        /// <summary>Current and max health after any change.</summary>
        public event Action<float, float> Changed;

        public event Action Died;

        public Health(float max) => Reset(max);

        public void Reset(float max)
        {
            Max = Mathf.Max(1f, max);
            Current = Max;
            Changed?.Invoke(Current, Max);
        }

        public void TakeDamage(float amount)
        {
            if (!IsAlive || amount <= 0f) 
                return;

            float applied = Mathf.Min(Current, amount);
            Current -= applied;
            Changed?.Invoke(Current, Max);

            if (Current <= 0f) 
                Died?.Invoke();
        }
    }
}

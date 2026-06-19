using UnityEngine;

namespace MagicalTower.UI
{
    /// <summary>
    /// Decoupled damage-number presentation. Damage-dealing code depends on this interface (injected)
    /// instead of knowing about the UI — keeps gameplay and presentation separated without signals.
    /// </summary>
    public interface IFloatingTextService
    {
        void Show(Vector3 worldPosition, float amount, Color color);
    }
}

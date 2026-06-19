using UnityEngine;
using UnityEngine.UI;
using MagicalTower.Spells;
using TMPro;

namespace MagicalTower.UI
{
    /// <summary>
    /// Drives a single spell button: casts the spell on click, disables itself while on cooldown, and
    /// shows the remaining cooldown as a radial overlay that wipes away as the spell reloads.
    /// </summary>
    public class SpellButtonView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private Button _button;
        [SerializeField] private Image _cooldownOverlay;

        
        private ISpell _spell;
        
        public void Init(ISpell spell)
        {
            _spell = spell;
            _label.text = spell.Name;

            _button.onClick.AddListener(() => _spell.TryCast());
            
            gameObject.name = "SpellButton" + spell.Name;
            
            Update();
        }

        private void Update() 
        {
            if (_spell == null) 
                return;
            
            _button.interactable = _spell.IsReady;
            _cooldownOverlay.fillAmount = _spell.CooldownNormalized;
        }
    }
}

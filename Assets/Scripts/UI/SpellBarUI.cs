using System.Collections.Generic;
using UnityEngine;
using Zenject;
using MagicalTower.Spells;

namespace MagicalTower.UI
{
    /// <summary>
    /// Builds one cast button per spell along the bottom of the screen. Buttons are generated from the
    /// injected spell list, so adding a spell config automatically adds its button
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class SpellBarUI : MonoBehaviour
    {
        [SerializeField] private SpellButtonView _spellButtonDummy;

        private List<ISpell> _spells;
        private Sprite _uiSprite;

        [Inject]
        public void Construct(List<ISpell> spells) => _spells = spells;

        private void Start()
        {
            if (_spells == null) 
                return;
            
            foreach (ISpell spell in _spells)
                CreateButton(spell);
        }

        private void CreateButton(ISpell spell)
        {
            var spellButton = Instantiate(_spellButtonDummy, _spellButtonDummy.transform.parent, false);
            spellButton.Init(spell);
            spellButton.gameObject.SetActive(true);
        }
    }
}

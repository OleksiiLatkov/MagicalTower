using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MagicalTower.Spells
{
    /// <summary>
    /// Lives on the tower and ticks every equipped spell so they auto-cast on their own cooldowns.
    /// The spell list is built from the configs in the installer and injected here.
    /// </summary>
    public class SpellCaster : MonoBehaviour
    {
        private List<ISpell> _spells;

        [Inject]
        public void Construct(List<ISpell> spells) => _spells = spells;

        private void Update()
        {
            if (_spells == null) 
                return;

            float dt = Time.deltaTime;
            for (int i = 0; i < _spells.Count; i++)
                _spells[i].Tick(dt);
        }
    }
}

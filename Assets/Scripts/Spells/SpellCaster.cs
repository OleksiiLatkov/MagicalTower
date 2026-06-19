using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MagicalTower.Spells
{
    /// <summary>
    /// Owns every spell's cooldown each frame
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

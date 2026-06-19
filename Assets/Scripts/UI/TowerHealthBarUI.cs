using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using MagicalTower.Towers;

namespace MagicalTower.UI
{
    /// <summary>Drives the tower's on-screen health bar (filled image + text) from its health events.</summary>
    public class TowerHealthBarUI : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_Text _label;

        private Tower _tower;

        [Inject]
        public void Construct(Tower tower) => _tower = tower;

        private void Start()
        {
            if (_tower?.Health == null) 
                return;
            
            _tower.Health.Changed += OnChanged;
            
            OnChanged(_tower.Health.Current, _tower.Health.Max);
        }

        private void OnDestroy()
        {
            if (_tower?.Health == null) 
                return;
            
            _tower.Health.Changed -= OnChanged;
        }

        private void OnChanged(float current, float max)
        {
            _slider.value = max > 0f ? current / max : 0f;
            _label.text = $"{Mathf.Ceil(current)} / {Mathf.Ceil(max)}";
        }
    }
}

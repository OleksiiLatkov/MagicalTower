using TMPro;
using UnityEngine;
using Zenject;

namespace MagicalTower.UI
{
    /// <summary>
    /// Pooled world-space damage number that floats up, billboards to the camera and fades out before
    /// returning itself to its pool.
    /// </summary>
    [RequireComponent(typeof(TextMeshPro))]
    public class FloatingDamageText : MonoBehaviour
    {
        [SerializeField] private float _lifetime = 0.8f;
        [SerializeField] private float _riseSpeed = 1.5f;
        [SerializeField] private TextMeshPro _text;
        
        private Camera _camera;
        private IMemoryPool _pool;
        private Color _baseColor;
        private float _age;

        [Inject]
        public void Construct(Camera camera) => _camera = camera;

        public void Init(Vector3 worldPos, float amount, Color color)
        {
            transform.position = worldPos;
            _baseColor = color;
            _text.text = Mathf.Max(1f, Mathf.Round(amount)).ToString("0");
            _text.color = color;
            _age = 0f;
        }

        private void Update()
        {
            _age += Time.deltaTime;
            transform.position += Vector3.up * (_riseSpeed * Time.deltaTime);

            if (_camera != null)
                transform.rotation = _camera.transform.rotation; // billboard

            float t = _age / _lifetime;
            Color c = _baseColor;
            c.a = Mathf.Clamp01(1f - t);
            _text.color = c;

            if (_age >= _lifetime)
                Despawn();
        }

        private void Despawn()
        {
            if (_pool != null) 
                _pool.Despawn(this);
            else 
                Destroy(gameObject);
        }

        public void SetPool(IMemoryPool pool) => _pool = pool;

        public class Pool : MonoMemoryPool<Vector3, float, Color, FloatingDamageText>
        {
            protected override void OnCreated(FloatingDamageText item)
            {
                base.OnCreated(item);
                item.SetPool(this);
            }

            protected override void Reinitialize(Vector3 pos, float amount, Color color, FloatingDamageText item)
                => item.Init(pos, amount, color);
        }
    }
}

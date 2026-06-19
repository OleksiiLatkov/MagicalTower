using UnityEngine;
using Zenject;
using MagicalTower.Core;
using MagicalTower.Effects;
using MagicalTower.Enemies.Movement;
using MagicalTower.Towers;
using MagicalTower.UI;

namespace MagicalTower.Enemies
{
    /// <summary>
    /// Generic, pooled enemy. Every difference between enemy types ("Default", "Fast", "Big & Slow")
    /// comes from the injected <see cref="EnemyConfig"/> — the prefab and this script never change.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class Enemy : MonoBehaviour, IEnemyTarget
    {
        [Tooltip("Renderer whose colour is tinted from the config at spawn.")]
        [SerializeField] private Renderer _renderer;

        private const float AttackRange = 1.4f;

        private Tower _tower;
        private EnemyRegistry _registry;
        private IFloatingTextService _floatingText;
        private IMemoryPool _pool;

        private EnemyConfig _config;
        private Health _health;
        private StatusEffectHandler _status;
        private IEnemyMovement _movement;
        private MaterialPropertyBlock _mpb;

        private float _attackTimer;
        private bool _active;

        public Vector3 Position => transform.position;
        public bool IsAlive => _active && _health != null && _health.IsAlive;

        [Inject]
        public void Construct(Tower tower, EnemyRegistry registry, IFloatingTextService floatingText)
        {
            _tower = tower;
            _registry = registry;
            _floatingText = floatingText;
        }

        /// <summary>Called by the pool every time the enemy is (re)spawned.</summary>
        public void Initialize(EnemyConfig config, Vector3 spawnPosition)
        {
            _config = config;
            transform.position = spawnPosition;
            transform.localScale = Vector3.one * config.Scale;

            _health = new Health(config.MaxHealth);
            _health.Died += OnDied;

            _status ??= new StatusEffectHandler(this);
            _status.Clear();

            _movement = config.Movement ?? new StraightMovement();
            _attackTimer = 0f;
            _active = true;

            ApplyColor(config.Color);
            _registry.Register(this);
        }

        private void Update()
        {
            if (!_active) 
                return;

            float dt = Time.deltaTime;
            _status.Tick(dt);
            
            if (!IsAlive) 
                return;

            Vector3 target = _tower.Position;
            float flatDistance = Vector3.Distance(
                new Vector3(transform.position.x, 0f, transform.position.z),
                new Vector3(target.x, 0f, target.z));

            if (flatDistance > AttackRange)
            {
                _movement.Tick(transform, target, _config.MoveSpeed, dt);
            }
            else
            {
                _attackTimer -= dt;
                if (_attackTimer <= 0f)
                {
                    _attackTimer = _config.AttackInterval;
                    _tower.TakeDamage(new DamageInfo(_config.AttackDamage, _tower.Position, DamageKind.Normal));
                }
            }
        }

        public void TakeDamage(in DamageInfo info)
        {
            if (!IsAlive) 
                return;

            _health.TakeDamage(info.Amount);
            Color color = info.Kind == DamageKind.Burning ? new Color(1f, 0.55f, 0.1f) : Color.white;
            _floatingText.Show(info.Point, info.Amount, color);
        }

        public void ApplyStatus(IStatusEffect effect)
        {
            if (IsAlive) 
                _status.Apply(effect);
        }

        private void ApplyColor(Color color)
        {
            if (_renderer == null) 
                return;
            
            _mpb ??= new MaterialPropertyBlock();
            _renderer.GetPropertyBlock(_mpb);
            _mpb.SetColor("_BaseColor", color);
            _mpb.SetColor("_Color", color);
            _renderer.SetPropertyBlock(_mpb);
        }

        private void OnDied()
        {
            if (!_active) return;
            _active = false;
            _registry.Unregister(this);
            _health.Died -= OnDied;
            Despawn();
        }

        private void Despawn()
        {
            if (_pool != null) _pool.Despawn(this);
            else Destroy(gameObject);
        }

        public void SetPool(IMemoryPool pool) => _pool = pool;

        /// <summary>One pool serves all enemy types; the config supplied at spawn drives the differences.</summary>
        public class Pool : MonoMemoryPool<EnemyConfig, Vector3, Enemy>
        {
            protected override void OnCreated(Enemy item)
            {
                base.OnCreated(item);
                item.SetPool(this);
            }

            protected override void Reinitialize(EnemyConfig config, Vector3 position, Enemy item)
                => item.Initialize(config, position);
        }
    }
}

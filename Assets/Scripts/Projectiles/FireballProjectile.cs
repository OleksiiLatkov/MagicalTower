using System.Collections.Generic;
using UnityEngine;
using Zenject;
using MagicalTower.Core;
using MagicalTower.Effects;

namespace MagicalTower.Projectiles
{
    /// <summary>All data a fireball needs for one flight, supplied by the spell at spawn time.</summary>
    public struct FireballLaunch
    {
        public Vector3 Start;
        public Vector3 Target;
        public float Speed;
        public float AreaDamage;
        public float AreaRadius;
        public float BurnDamagePerSecond;
        public float BurnDuration;
        public GameObject ExplosionVfx;
    }

    /// <summary>
    /// Flies a ballistic arc toward a target point and explodes on contact with an enemy or the
    /// ground, dealing area damage and applying a <see cref="BurningEffect"/> to everything caught.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class FireballProjectile : Projectile
    {
        [SerializeField] private float _gravity = 9.8f;
        [SerializeField] private float _groundY = 0.05f;

        private static readonly Collider[] OverlapBuffer = new Collider[64];
        private static readonly HashSet<IEnemyTarget> SeenBuffer = new();

        private Vector3 _velocity;
        private FireballLaunch _data;
        private bool _exploded;

        public void Launch(FireballLaunch data)
        {
            _data = data;
            _exploded = false;
            transform.position = data.Start;

            // Solve a ballistic velocity so the fireball arcs and lands near the target.
            Vector3 toTarget = data.Target - data.Start;
            float flatDistance = new Vector3(toTarget.x, 0f, toTarget.z).magnitude;
            float time = Mathf.Max(0.25f, flatDistance / Mathf.Max(0.1f, data.Speed));
            Vector3 v = toTarget / time;
            v.y += 0.5f * _gravity * time;
            _velocity = v;
        }

        private void Update()
        {
            if (_exploded) 
                return;

            float dt = Time.deltaTime;
            _velocity.y -= _gravity * dt;
            transform.position += _velocity * dt;

            if (transform.position.y <= _groundY)
                Explode(new Vector3(transform.position.x, _groundY, transform.position.z));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_exploded) 
                return;
            
            if (other.GetComponentInParent<IEnemyTarget>() != null)
                Explode(transform.position);
        }

        private void Explode(Vector3 point)
        {
            _exploded = true;
            transform.position = point;

            if (_data.ExplosionVfx != null)
            {
                GameObject vfx = Object.Instantiate(_data.ExplosionVfx, point, Quaternion.identity);
                Object.Destroy(vfx, 2f);
            }

            int count = Physics.OverlapSphereNonAlloc(point, _data.AreaRadius, OverlapBuffer, ~0, QueryTriggerInteraction.Collide);
            SeenBuffer.Clear();

            for (int i = 0; i < count; i++)
            {
                IEnemyTarget enemy = OverlapBuffer[i].GetComponentInParent<IEnemyTarget>();
                if (enemy == null || !enemy.IsAlive || !SeenBuffer.Add(enemy)) continue;

                enemy.TakeDamage(new DamageInfo(_data.AreaDamage, enemy.Position + Vector3.up, DamageKind.Explosion));
                if (_data.BurnDuration > 0f)
                    enemy.ApplyStatus(new BurningEffect(_data.BurnDamagePerSecond, _data.BurnDuration));
            }

            Despawn();
        }

        public class Pool : MonoMemoryPool<FireballLaunch, FireballProjectile>
        {
            protected override void OnCreated(FireballProjectile item)
            {
                base.OnCreated(item);
                item.SetPool(this);
            }

            protected override void Reinitialize(FireballLaunch data, FireballProjectile item)
                => item.Launch(data);
        }
    }
}

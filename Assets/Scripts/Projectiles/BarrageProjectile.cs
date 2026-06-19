using UnityEngine;
using Zenject;
using MagicalTower.Core;

namespace MagicalTower.Projectiles
{
    /// <summary>All data a barrage bolt needs for one flight, supplied by the spell at spawn time.</summary>
    public struct BarrageLaunch
    {
        public Vector3 Start;
        public IEnemyTarget Target;
        public float Speed;
        public float Damage;
        public float ArcHeight;
    }

    /// <summary>
    /// Small bolt that flies a parabolic arc to a single tracked target and deals damage on arrival.
    /// </summary>
    public class BarrageProjectile : Projectile
    {
        private BarrageLaunch _data;
        private Vector3 _start;
        private Vector3 _lastTargetPosition;
        private float _t;
        private float _flightTime;
        private bool _done;

        public void Launch(BarrageLaunch data)
        {
            _data = data;
            _start = data.Start;
            _t = 0f;
            _done = false;
            _lastTargetPosition = data.Target != null ? data.Target.Position : data.Start;

            float distance = Vector3.Distance(_start, _lastTargetPosition);
            _flightTime = Mathf.Max(0.15f, distance / Mathf.Max(0.1f, data.Speed));
            transform.position = _start;
        }

        private void Update()
        {
            if (_done) 
                return;

            _t += Time.deltaTime / _flightTime;

            if (_data.Target != null && _data.Target.IsAlive)
                _lastTargetPosition = _data.Target.Position;

            Vector3 pos = Vector3.Lerp(_start, _lastTargetPosition, _t);
            pos.y += _data.ArcHeight * Mathf.Sin(Mathf.PI * Mathf.Clamp01(_t));
            transform.position = pos;

            if (_t >= 1f)
                Arrive();
        }

        private void Arrive()
        {
            _done = true;
            if (_data.Target != null && _data.Target.IsAlive)
            {
                _data.Target.TakeDamage(
                    new DamageInfo(_data.Damage, _data.Target.Position + Vector3.up, DamageKind.Normal));
            }
            Despawn();
        }

        public class Pool : MonoMemoryPool<BarrageLaunch, BarrageProjectile>
        {
            protected override void OnCreated(BarrageProjectile item)
            {
                base.OnCreated(item);
                item.SetPool(this);
            }

            protected override void Reinitialize(BarrageLaunch data, BarrageProjectile item)
                => item.Launch(data);
        }
    }
}

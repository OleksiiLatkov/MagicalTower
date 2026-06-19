using UnityEngine;
using Zenject;
using MagicalTower.Enemies;
using MagicalTower.Towers;

namespace MagicalTower.Spawning
{
    /// <summary>
    /// Spawns enemies on a ring around the tower
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        private SpawnConfig _config;
        private Enemy.Pool _pool;
        private Tower _tower;

        private float _elapsed;
        private float _timer;

        [Inject]
        public void Construct(SpawnConfig config, Enemy.Pool pool, Tower tower)
        {
            _config = config;
            _pool = pool;
            _tower = tower;
        }

        private void Update()
        {
            if (_config == null) 
                return;

            float dt = Time.deltaTime;
            _elapsed += dt;
            _timer -= dt;

            SpawnPhase phase = _config.GetPhase(_elapsed);
            if (phase == null) 
                return;

            if (_timer <= 0f)
            {
                _timer = Mathf.Max(0.05f, phase.SpawnInterval);
                Spawn(phase);
            }
        }

        private void Spawn(SpawnPhase phase)
        {
            EnemyConfig enemyConfig = _config.PickEnemy(phase);
            
            if (enemyConfig == null) 
                return;

            float angle = Random.value * Mathf.PI * 2f;
            Vector3 origin = _tower != null ? _tower.Position : Vector3.zero;
            Vector3 pos = origin + new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * _config.SpawnRadius;
            pos.y = enemyConfig.Scale;

            _pool.Spawn(enemyConfig, pos);
        }
    }
}

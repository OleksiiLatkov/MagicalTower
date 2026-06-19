using System.Collections.Generic;
using UnityEngine;
using Zenject;
using MagicalTower.Core;
using MagicalTower.Enemies;
using MagicalTower.Projectiles;
using MagicalTower.Spawning;
using MagicalTower.Spells;
using MagicalTower.Towers;
using MagicalTower.UI;

namespace MagicalTower.Installers
{
    /// <summary>
    /// Single composition root for the game
    /// </summary>
    public class GameInstaller : MonoInstaller
    {
        [Header("Scene References")]
        [SerializeField] private Camera _gameCamera;
        [SerializeField] private Tower _tower;
        [SerializeField] private GameOverUI _gameOverUI;

        [Header("Prefabs")]
        [SerializeField] private Enemy _enemyPrefab;
        [SerializeField] private FireballProjectile _fireballPrefab;
        [SerializeField] private BarrageProjectile _barragePrefab;
        [SerializeField] private FloatingDamageText floatingDamageTextPrefab;

        [Header("Configs")]
        [SerializeField] private TowerConfig _towerConfig;
        [SerializeField] private SpawnConfig _spawnConfig;
        [SerializeField] private List<SpellConfig> _spells = new();

        [Header("Pool Sizes")]
        [SerializeField] private int _enemyPoolSize = 30;
        [SerializeField] private int _projectilePoolSize = 30;
        [SerializeField] private int _popupPoolSize = 30;

        public override void InstallBindings()
        {
            InstallCore();
            InstallFeedback();
            InstallEnemies();
            InstallProjectiles();
            InstallSpells();
        }

        private void InstallCore()
        {
            Container.Bind<Camera>().FromInstance(_gameCamera).AsSingle();
            Container.Bind<Tower>().FromInstance(_tower).AsSingle();
            Container.Bind<TowerConfig>().FromInstance(_towerConfig).AsSingle();
            Container.Bind<GameOverUI>().FromInstance(_gameOverUI).AsSingle();
            Container.Bind<EnemyRegistry>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameController>().AsSingle();
        }

        private void InstallFeedback()
        {
            Container.BindInterfacesTo<FloatingTextService>().AsSingle();
            Container.BindMemoryPool<FloatingDamageText, FloatingDamageText.Pool>()
                .WithInitialSize(_popupPoolSize)
                .FromComponentInNewPrefab(floatingDamageTextPrefab)
                .UnderTransformGroup("DamagePopups");
        }

        private void InstallEnemies()
        {
            Container.Bind<SpawnConfig>().FromInstance(_spawnConfig).AsSingle();
            Container.BindMemoryPool<Enemy, Enemy.Pool>()
                .WithInitialSize(_enemyPoolSize)
                .FromComponentInNewPrefab(_enemyPrefab)
                .UnderTransformGroup("Enemies");
        }

        private void InstallProjectiles()
        {
            Container.BindMemoryPool<FireballProjectile, FireballProjectile.Pool>()
                .WithInitialSize(_projectilePoolSize)
                .FromComponentInNewPrefab(_fireballPrefab)
                .UnderTransformGroup("Projectiles");

            Container.BindMemoryPool<BarrageProjectile, BarrageProjectile.Pool>()
                .WithInitialSize(_projectilePoolSize)
                .FromComponentInNewPrefab(_barragePrefab)
                .UnderTransformGroup("Projectiles");
        }

        private void InstallSpells()
        {
            // Each spell config builds its own runtime spell, with dependencies injected by Zenject.
            // Binding one ISpell per config means SpellCaster simply injects List<ISpell>.
            foreach (SpellConfig spellConfig in _spells)
            {
                SpellConfig captured = spellConfig;
                Container.Bind<ISpell>()
                    .FromMethod(ctx => captured.CreateRuntime(ctx.Container))
                    .AsCached();
            }
        }
    }
}

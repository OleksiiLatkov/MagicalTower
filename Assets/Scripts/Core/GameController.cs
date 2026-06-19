using System;
using UnityEngine;
using Zenject;
using MagicalTower.Towers;
using MagicalTower.UI;

namespace MagicalTower.Core
{
    /// <summary>
    /// Owns the lose flow
    /// </summary>
    public class GameController : IInitializable, IDisposable
    {
        private readonly Tower _tower;
        private readonly GameOverUI _gameOverUI;
        private bool _gameOver;

        public GameController(Tower tower, GameOverUI gameOverUI)
        {
            _tower = tower;
            _gameOverUI = gameOverUI;
        }

        public void Initialize()
        {
            Time.timeScale = 1f; // ensure a clean start after a restart
            if (_tower?.Health != null)
                _tower.Health.Died += OnTowerDestroyed;
        }

        public void Dispose()
        {
            if (_tower?.Health != null)
                _tower.Health.Died -= OnTowerDestroyed;
        }

        private void OnTowerDestroyed()
        {
            if (_gameOver) 
                return;
            
            _gameOver = true;
            
            _gameOverUI?.Show();
            Time.timeScale = 0f;
        }
    }
}

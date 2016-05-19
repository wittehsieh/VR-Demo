using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Cameo
{
    public class GameSystem : Singleton<GameSystem>
    {
        private class LevelInfo
        {
            public int EnemCount = 0;
            public LevelEventDelegate LevelStartEvent = delegate { };
            public LevelEventDelegate LevelFinishedEvent = delegate { };
        }
        public delegate void LevelEventDelegate();

        public bool GameMode = true;
        public bool UseVR = true;

        private Dictionary<int, LevelInfo> _levelInfo = new Dictionary<int, LevelInfo>();

        public void RegisterEnemy(int level, Enemy enemy)
        {
            if (!_levelInfo.ContainsKey(level))
                _levelInfo.Add(level, new LevelInfo());

            _levelInfo[level].LevelStartEvent += enemy.OnStart;
            _levelInfo[level].EnemCount++;
        }

        public void RegisterLevelFinished(int level, LevelEventDelegate callback)
        {
            if (!_levelInfo.ContainsKey(level))
                _levelInfo.Add(level, new LevelInfo());

            _levelInfo[level].LevelFinishedEvent += callback;
        }

        public void StartLevel(int level)
        {
            if(_levelInfo.ContainsKey(level))
            {
                _levelInfo[level].LevelStartEvent.Invoke();
            }
            else
            {
                Debug.Log("Level: " + level.ToString() + " is not exist");
            }
        }

        public void OnLevelEnemyHit(int level)
        {
            _levelInfo[level].EnemCount--;

            if (_levelInfo[level].EnemCount == 0)
            {
                _levelInfo[level].LevelFinishedEvent();
                ClearLevel(level);
            }
        }

        private void ClearLevel(int level)
        {
            _levelInfo[level].LevelFinishedEvent = delegate { };
            _levelInfo[level].LevelStartEvent = delegate { };
            _levelInfo.Remove(level);
        }
    }

}

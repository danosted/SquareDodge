namespace Assets.Code.MonoBehaviours.Obstacles
{
    using System.Collections.Generic;
    using UnityEngine;
    using IoC;
    using DataAccess;
    using DataAccess.DTOs;
    using GameLogic;
    using System.Linq;
    using Utilities;

    public class ObstacleField : MonoBehaviour
    {
        private IoC _container;
        private ICollection<ObstacleBase> _activeObstacles;
        private ICollection<ObstacleBase> _inactiveObstacles;

        private Wave _currentWave;
        private ScreenLogic _screen;
        private ObstacleLogic _obstacleLogic;

        public int FieldSize { get; set; }

        public void Activate(IoC ioc)
        {
            _container = ioc;
            _activeObstacles = new List<ObstacleBase>();
            _inactiveObstacles = new List<ObstacleBase>();
            gameObject.SetActive(true);

            // TODO 1 (DRO): Test
            _screen = _container.Resolve<ScreenLogic>();
            _obstacleLogic = _container.Resolve<ObstacleLogic>();

            var size = new Vector3(Mathf.Abs(_screen.ViewportToWorldBorderMin.x) + Mathf.Abs(_screen.ViewportToWorldBorderMax.x), Mathf.Abs(_screen.ViewportToWorldBorderMin.y) + Mathf.Abs(_screen.ViewportToWorldBorderMax.y), 0f);
            FieldSize = size.x > size.y ? (int)size.y : (int)size.x;
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public void StartWave(Wave wave)
        {
            if (_currentWave != null)
            {
                throw new UnityException(string.Format("Current wave is already set."));
            }
            _currentWave = wave;
        }

        private void CreateObstacles(int obstacleCount, int level)
        {
            var prefabManager = _container.Resolve<PrefabManager>();

            // TODO 1 (DRO): Include the size of the obstacles
            var possiblePositions = obstacleCount < FieldSize ? FieldSize : obstacleCount;
            Debug.LogFormat("possiblePositions. {0}.", possiblePositions);

            var positions = new int[possiblePositions];
            for(var i = 0; i < possiblePositions; i++)
            {
                positions[i] = i;
            }
            
            for (var i = 0; i < obstacleCount; i++)
            {
                var obst = _obstacleLogic.GetNormalObstacle();

                // Find what remains of the positions available
                var remainingPositions = positions.Where(x => x != -1).ToArray();

                // Pick a random
                var random = Random.Range(0, remainingPositions.Count());

                // Get the index value of the position
                var indexValue = remainingPositions[random];

                var obstacleWidth = 0.5f;
                var obstacleHeight = 0.5f;

                // Map from positions to world coordinates using screen borders
                var positionValue = MathUtil.MapValueFromRangeToRange(indexValue, 0, possiblePositions, _screen.ViewportToWorldBorderMin.x + obstacleWidth, _screen.ViewportToWorldBorderMax.x - obstacleWidth);
                Debug.LogFormat("positionValue. {0}.", positionValue);

                var randomPosition = new Vector3(positionValue, _screen.ViewportToWorldBorderMax.y - obstacleHeight, 0f);

                positions[indexValue] = -1;

                obst.Activate(_container, level, randomPosition);

                _activeObstacles.Add(obst);
            }
        }

        void Update()
        {
            if (_currentWave == null)
            {
                return;
            }
            if(_currentWave.IsStarted)
            {
                _currentWave.WaveActiveTime += Time.deltaTime;
                if(_currentWave.WaveActiveTime > _currentWave.WaveLengthSeconds)
                {
                    _currentWave = _currentWave.NextWave;
                }
                return;
            }
            _currentWave.IsStarted = true;
            CreateObstacles(_currentWave.ObstacleCount, _currentWave.ObstacleLevel);
        }
    }
}

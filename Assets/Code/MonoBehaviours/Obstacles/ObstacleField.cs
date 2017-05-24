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
    using Common;

    public class ObstacleField : PrefabBase
    {
        private ICollection<ObstacleBase> _activeObstacles;
        private ICollection<ObstacleBase> _inactiveObstacles;

        private Wave _currentWave;
        private ScreenUtil _screen;
        private ObstacleLogic _obstacleLogic;

        public int FieldSize { get; set; }

        public override void Activate(IoC container)
        {
            base.Activate(container);
            _activeObstacles = new List<ObstacleBase>();
            _inactiveObstacles = new List<ObstacleBase>();
            _screen = Container.Resolve<ScreenUtil>();
            _obstacleLogic = Container.Resolve<ObstacleLogic>();
            _currentWave = null;

            var size = _screen.GetScreenSizeInWorld();
            FieldSize = size.x > size.y ? (int)size.y : (int)size.x;

            gameObject.SetActive(true);
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

        ///// <summary>
        ///// Create obstacle distributed in equal possitions along the width of the playing field
        ///// </summary>
        //private void CreateObstacles(int obstacleCount, int level)
        //{
        //    var prefabManager = Container.Resolve<PrefabManager>();

        //    // TODO 1 (DRO): Include the size of the obstacles
        //    var possiblePositions = obstacleCount < FieldSize ? FieldSize : obstacleCount;

        //    // TODO 1 (DRO): Make it obstacle width independent
        //    // We distribute positions from the assumption, that each obstacle is 1 unit wide
        //    var positions = new int[possiblePositions];
        //    for(var i = 0; i < possiblePositions; i++)
        //    {
        //        positions[i] = i;
        //    }
            
        //    for (var i = 0; i < obstacleCount; i++)
        //    {
        //        var obst = _obstacleLogic.GetNormalObstacle();

        //        // Find what remains of the positions available
        //        var remainingPositions = positions.Where(x => x != -1).ToArray();

        //        // Pick a random
        //        var random = Random.Range(0, remainingPositions.Count());

        //        // Get the index value of the position
        //        var indexValue = remainingPositions[random];

        //        var obstacleWidth = 0.5f;
        //        var obstacleHeight = 0.5f;

        //        // Map from positions to world coordinates using screen borders
        //        var positionValue = MathUtil.MapInputFromInputRangeToOutputRange(indexValue, 0, possiblePositions, _screen.ViewportToWorldBorderMin.x + obstacleWidth, _screen.ViewportToWorldBorderMax.x - obstacleWidth);
                
        //        var randomPosition = new Vector3(positionValue, _screen.ViewportToWorldBorderMax.y - obstacleHeight, 0f);

        //        positions[indexValue] = -1;

        //        obst.Activate(Container, level, randomPosition);

        //        ScoreLogic.AddToScore(level);

        //        _activeObstacles.Add(obst);
        //    }
        //}

        void Update()
        {
            if (_currentWave == null)
            {
                if(!_activeObstacles.Any(ao => ao.isActiveAndEnabled))
                {
                    Container.Resolve<FlowLogic>().GameOver();
                }
                return;
            }
            if (_currentWave.IsStarted)
            {
                _currentWave.WaveActiveTime += Time.deltaTime;
                if(_currentWave.WaveActiveTime > _currentWave.WaveLengthSeconds)
                {
                    _currentWave = _currentWave.NextWave;
                }
                return;
            }
            _currentWave.IsStarted = true;
            var objs = Container.Resolve<ObstacleLogic>().CreateObstacles(_currentWave.ObstacleCount, _currentWave.ObstacleLevel, FieldSize);
            _activeObstacles.ToList().AddRange(objs);
        }
    }
}

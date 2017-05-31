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

        private Wave _currentWave;
        private ScreenUtil _screen;
        private ObstacleLogic _obstacleLogic;

        public int FieldSize { get; set; }

        public override void Activate(IoC container)
        {
            base.Activate(container);
            _activeObstacles = new List<ObstacleBase>();
            _screen = Container.Resolve<ScreenUtil>();
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

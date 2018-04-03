namespace Assets.Code.GameLogic
{
    using MonoBehaviours.Configuration;
    using Common;
    using DataAccess;
    using IoC;
    using MonoBehaviours.Obstacles;
    using UnityEngine;
    using System.Linq;
    using Assets.Code.Utilities;
    using System.Collections.Generic;

    public class ObstacleLogic : LogicBase
    {

        private ScreenUtil _screen { get; set; }
        private ScoreLogic ScoreLogic { get; set; }

        public ObstacleLogic(IoC container, PrefabManager prefabManager, GlobalConfiguration config) : base(container, prefabManager, config)
        {
            _screen = Container.Resolve<ScreenUtil>();
            ScoreLogic = Container.Resolve<ScoreLogic>();
        }

        internal NormalObstacle GetNormalObstacle()
        {
            return PrefabManager.GetPrefab(Configuration.prefab_normal_obstacle).GetComponent<NormalObstacle>();
        }

        internal void SpawnObstacleDebris(Vector3 position, int count)
        {
            for (var i = 0; i < count; i++)
            {
                var debris = PrefabManager.GetPrefab(Configuration.prefab_obstacle_debris).GetComponent<ObstacleDebris>();
                debris.Activate(Container, 6, position);
            }
        }

        /// <summary>
        /// Create obstacle distributed in equal possitions along the width of the playing field
        /// </summary>
        public ICollection<ObstacleBase> CreateObstacles(int obstacleCount, int level, int FieldSize)
        {
            var activeObjs = new List<ObstacleBase>();

            // TODO 1 (DRO): Include the size of the obstacles
            var possiblePositions = obstacleCount < FieldSize ? FieldSize : obstacleCount;

            // TODO 1 (DRO): Make it obstacle width independent
            // We distribute positions from the assumption, that each obstacle is 1 unit wide
            var positions = new int[possiblePositions];
            for (var i = 0; i < possiblePositions; i++)
            {
                positions[i] = i;
            }

            for (var i = 0; i < obstacleCount; i++)
            {
                var obst = GetNormalObstacle();

                // Find what remains of the positions available
                var remainingPositions = positions.Where(x => x != -1).ToArray();

                // Pick a random
                var random = Random.Range(0, remainingPositions.Count());

                // Get the index value of the position
                var indexValue = remainingPositions[random];

                var obstacleWidth = 0.5f;
                var obstacleHeight = 0.5f;

                // Map from positions to world coordinates using screen borders
                var positionValue = MathUtil.MapInputFromInputRangeToOutputRange(indexValue, 0, possiblePositions, _screen.ViewportToWorldBorderMin.x + obstacleWidth, _screen.ViewportToWorldBorderMax.x - obstacleWidth);

                var randomPosition = new Vector3(positionValue, _screen.ViewportToWorldBorderMax.y - obstacleHeight, 0f);

                positions[indexValue] = -1;

                obst.Activate(Container, level, randomPosition);

                //ScoreLogic.AddToScore(level);

                activeObjs.Add(obst);
            }
            return activeObjs;
        }
    }
}

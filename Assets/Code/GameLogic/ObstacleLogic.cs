namespace Assets.Code.GameLogic
{
    using MonoBehaviours.Configuration;
    using Common;
    using DataAccess;
    using IoC;
    using MonoBehaviours.Obstacles;
    using UnityEngine;

    public class ObstacleLogic : LogicBase
    {
        public ObstacleLogic(IoC container, PrefabManager prefabManager, GlobalConfiguration config) : base(container, prefabManager, config)
        {
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
    }
}

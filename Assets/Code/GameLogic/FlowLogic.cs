namespace Assets.Code.GameLogic
{
    using MonoBehaviours.Configuration;
    using Common;
    using DataAccess;
    using DataAccess.DTOs;
    using IoC;
    using UnityEngine;

    public class FlowLogic : LogicBase
    {
        public FlowLogic(IoC container, PrefabManager prefabManager, GlobalConfiguration config) : base(container, prefabManager, config)
        {
        }

        public void StartGameFlow()
        {
            var pman = Container.Resolve<PrefabManager>();
            var field = pman.GetPrefab(Configuration.prefab_obstacle_field);
            field.Activate(Container);
            field.StartWave(CreateWaves(null, 100));
        }

        private Wave CreateWaves(Wave lastWave, int i)
        {
            if (i == 0)
            {
                return lastWave;
            }
            if (lastWave == null)
            {
                lastWave = CreateWave();
            }

            var nextWave = CreateWave();
            nextWave.NextWave = lastWave;
            return CreateWaves(nextWave, i - 1);

        }

        private Wave CreateWave()
        {
            return new Wave
            {
                ObstacleCount = Random.Range(1, 3),
                ObstacleLevel = Random.Range(1, 5),
                WaveLengthSeconds = Random.Range(0f, 1f),
            };
        }

    }
}

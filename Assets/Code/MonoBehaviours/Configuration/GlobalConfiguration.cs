namespace Assets.Code.MonoBehaviours.Configuration
{
    using Obstacles;
    using System.Collections.Generic;
    using UnityEngine;

    public class GlobalConfiguration : MonoBehaviour
    {
        [Header("Obstacles")]
        public NormalObstacle prefab_normal_obstacle;
        public ObstacleField prefab_obstacle_field;
        public ObstacleDebris prefab_obstacle_debris;
    }
}

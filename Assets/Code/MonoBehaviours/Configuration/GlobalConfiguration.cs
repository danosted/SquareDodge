namespace Assets.Code.MonoBehaviours.Configuration
{
    using Drawing;
    using Obstacles;
    using System.Collections.Generic;
    using UnityEngine;
    using UserInterface;

    public class GlobalConfiguration : MonoBehaviour
    {
        [Header("Obstacles")]
        public NormalObstacle prefab_normal_obstacle;
        public ObstacleField prefab_obstacle_field;
        public ObstacleDebris prefab_obstacle_debris;
        [Header("UI")]
        public CanvasManager ui_game_canvas_manager;
        public CanvasManager ui_game_over_canvas_manager;
        [Header("Drawing Field")]
        public DrawingField prefab_drawing_field;
        public GameObject prefab_dummy;
        [Header("Game Params")]
        public bool param_game_over;
    }
}

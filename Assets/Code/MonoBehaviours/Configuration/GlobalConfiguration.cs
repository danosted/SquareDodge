namespace Assets.Code.MonoBehaviours.Configuration
{
    using Assets.Code.Common;
    using Drawing;
    using Obstacles;
    using System.Collections.Generic;
    using UnityEngine;
    using UserInterface;

    public class GlobalConfiguration : PrefabBase
    {
        [Header("Obstacles")]
        public NormalObstacle prefab_normal_obstacle;
        public ObstacleField prefab_obstacle_field;
        public ObstacleDebris prefab_obstacle_debris;
        [Header("UI")]
        public CanvasManager ui_game_canvas_manager;
        public CanvasManager ui_game_over_canvas_manager;
        public FloatingText ui_floating_text_canvas_element;
        [Header("Drawing Field")]
        public DrawingField prefab_drawing_field;
        public Vector2 param_drawing_field_figure_point_count;
        public Vector3 param_drawing_field_point_bonuses;
        [Header("Colors")]
        public Color col_txt_failure;
        public Color col_txt_success;
        [Header("Global Game Params")]
        public bool param_game_over;
        public bool param_debug_enabled;
    }
}

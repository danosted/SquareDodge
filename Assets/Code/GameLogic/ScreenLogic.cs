namespace Assets.Code.GameLogic
{
    using UnityEngine;
    using IoC;
    using Common;
    using DataAccess;
    using MonoBehaviours.Configuration;

    public class ScreenLogic : LogicBase
    {
        public ScreenLogic(IoC container, PrefabManager prefabManager, GlobalConfiguration config) : base(container, prefabManager, config)
        {
            ViewportToWorldBorderMin = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.farClipPlane));
            ViewportToWorldBorderMax = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.farClipPlane));
        }

        public Vector3 ViewportToWorldBorderMax { get; set; }
        public Vector3 ViewportToWorldBorderMin { get; set; }

        public bool IsOutOfViewportBounds(Vector3 position)
        {
            if (position.y > ViewportToWorldBorderMax.y)
            {
                return true;
            }

            if (position.y < ViewportToWorldBorderMin.y)
            {
                return true;
            }

            if (position.x > ViewportToWorldBorderMax.x)
            {
                return true;
            }

            if (position.x < ViewportToWorldBorderMin.x)
            {
                return true;
            }

            return false;
        }
    }
}
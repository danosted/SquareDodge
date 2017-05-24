namespace Assets.Code.Utilities
{
    using UnityEngine;

    public class ScreenUtil
    {

        public Vector3 ViewportToWorldBorderMax
        {
            get
            {
                return Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.farClipPlane));
            }
        }
        public Vector3 ViewportToWorldBorderMin
        {
            get
            {
                return Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.farClipPlane));
            }
        }

        public Vector3 ViewportToScreenBorderMax
        {
            get
            {
                return Camera.main.ViewportToScreenPoint(new Vector3(1, 1, Camera.main.farClipPlane));
            }
        }

        public Vector3 ViewportToScreenBorderMin
        {
            get
            {
                return Camera.main.ViewportToScreenPoint(new Vector3(0, 0, Camera.main.farClipPlane));
            }
        }

        public ScreenUtil()
        {
            //ViewportToWorldBorderMin = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.farClipPlane));
            //ViewportToWorldBorderMax = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.farClipPlane));

            //ViewportToScreenBorderMin = Camera.main.ViewportToScreenPoint(new Vector3(0, 0, Camera.main.farClipPlane));
            //ViewportToScreenBorderMax = Camera.main.ViewportToScreenPoint(new Vector3(1, 1, Camera.main.farClipPlane));
        }

        public Vector3 GetScreenPositionOnWorldPlane(Vector3 position)
        {
            return Camera.main.ScreenToWorldPoint(position);
        }


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

        public Vector2 GetScreenSizeInWorld()
        {
            return new Vector2(Mathf.Abs(ViewportToWorldBorderMin.x) + Mathf.Abs(ViewportToWorldBorderMax.x), Mathf.Abs(ViewportToWorldBorderMin.y) + Mathf.Abs(ViewportToWorldBorderMax.y));
        }

        public Vector2 GetScreenSize()
        {
            return new Vector2(Mathf.Abs(ViewportToScreenBorderMin.x) + Mathf.Abs(ViewportToScreenBorderMax.x), Mathf.Abs(ViewportToScreenBorderMin.y) + Mathf.Abs(ViewportToScreenBorderMax.y));
        }
    }

}

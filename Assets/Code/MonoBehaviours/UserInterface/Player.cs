namespace Assets.Code.MonoBehaviours.UserInterface
{
    using IoC;
    using Common;
    using UnityEngine;
    using Utilities;

    public class Player : PrefabBase
    {
        private RectTransform RectTransform { get; set; }
        private float _screenVerticalSize { get; set; }

        public override void Activate(IoC container)
        {
            base.Activate(container);
            RectTransform = GetComponent<RectTransform>();
            var screenUtil = Container.Resolve<ScreenUtil>();
            RectTransform = GetComponent<RectTransform>();
            _screenVerticalSize = screenUtil.GetScreenSize().y;
        }

        void Update()
        {
            var pointerPosition = Input.mousePosition;
            RectTransform.anchoredPosition3D = new Vector3(pointerPosition.x, pointerPosition.y - _screenVerticalSize, pointerPosition.z);
        }
    }
}

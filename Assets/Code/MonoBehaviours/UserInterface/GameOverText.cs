namespace Assets.Code.MonoBehaviours.UserInterface
{
    using IoC;
    using Common;
    using UnityEngine;
    using UnityEngine.UI;
    using GameLogic;
    using Configuration;
    using Utilities;

    [RequireComponent(typeof(Text))]
    public class GameOverText : PrefabBase
    {
        private Text Text { get; set; }
        private bool Activated { get; set; }
        private ScreenUtil ScreenUtil { get; set; }
        private float Size { get; set; }
        private RectTransform RectTransform { get; set; }

        public override void Activate(IoC container)
        {
            base.Activate(container);
            Activated = true;
            ScreenUtil = Container.Resolve<ScreenUtil>();
            RectTransform = GetComponent<RectTransform>();
            Size = (ScreenUtil.GetScreenSize().y - RectTransform.rect.height) * 0.5f;
        }

        // Shutdown game
        void Update()
        {
            if (!Activated) return;
            if (Mathf.Abs(RectTransform.anchoredPosition.y) >= Size)
            {
                Activated = false;
                Configuration.param_game_over = true;
                return;
            }
            RectTransform.Translate(Vector3.down * Time.deltaTime * 5f);
        }
    }
}

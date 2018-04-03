namespace Assets.Code.GameLogic
{
    using IoC;
    using Common;
    using DataAccess;
    using MonoBehaviours.Configuration;
    using MonoBehaviours.UserInterface;
    using UnityEngine;

    public class UserInterfaceLogic : LogicBase
    {
        public CanvasManager GameCanvas { get; private set; }
        public CanvasManager GameOverCanvas { get; private set; }

        public UserInterfaceLogic(IoC container, PrefabManager prefabManager, GlobalConfiguration config) : base(container, prefabManager, config)
        {
        }

        internal void InitializeGameCanvas()
        {
            if(GameCanvas != null)
            {
                return;
            }
            GameCanvas = PrefabManager.GetPrefab(Configuration.ui_game_canvas_manager);
        }

        internal void InitializeGameOverCanvas()
        {
            PrefabManager.ReturnPrefab(GameCanvas);

            GameOverCanvas = PrefabManager.GetPrefab(Configuration.ui_game_over_canvas_manager);
        }

        internal void ShowPlayerFloatingText(string textToShow, Color textColor)
        {
            var playerFloatingText = GameCanvas.GetComponentInChildren<FloatingText>();
            playerFloatingText.ShowTextForSeconds(textToShow, 1, textColor);
        }
    }
}
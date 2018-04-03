namespace Assets.Code.GameLogic
{
    using MonoBehaviours.Configuration;
    using Common;
    using DataAccess;
    using IoC;
    using System;

    public class ScoreLogic : LogicBase
    {
        public int CurrentScore { get; private set; }
        private UserInterfaceLogic UserInterfaceLogic { get; set; }

        public ScoreLogic(IoC container, PrefabManager prefabManager, GlobalConfiguration config) : base(container, prefabManager, config)
        {
            CurrentScore = 0;
            UserInterfaceLogic = Container.Resolve<UserInterfaceLogic>();
        }

        public void AddToScore(int toAdd)
        {
            var showColor = toAdd < 0 ? Configuration.col_txt_failure : Configuration.col_txt_success;
            UserInterfaceLogic.ShowPlayerFloatingText(toAdd.ToString(), showColor);
            CurrentScore += toAdd;
        }

        internal void AddDrawingFieldPoints(float activeSeconds)
        {
            var bonuses = Configuration.param_drawing_field_point_bonuses;
            if (activeSeconds >= 0f && activeSeconds <= 2f)
            {
                AddToScore((int)bonuses.x);
            }
            else if (activeSeconds > 2f && activeSeconds <= 3f)
            {
                AddToScore((int)bonuses.y);
            }
            else
            {
                AddToScore((int)bonuses.z);
            }
        }
    }
}

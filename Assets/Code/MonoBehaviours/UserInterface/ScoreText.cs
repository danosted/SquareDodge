namespace Assets.Code.MonoBehaviours.UserInterface
{
    using IoC;
    using Common;
    using UnityEngine;
    using UnityEngine.UI;
    using GameLogic;

    [RequireComponent(typeof(Text))]
    public class ScoreText : PrefabBase
    {
        private Text Text { get; set; }
        private ScoreLogic ScoreLogic { get; set; }

        public override void Activate(IoC container)
        {
            base.Activate(container);
            Text = GetComponent<Text>();
            ScoreLogic = ScoreLogic == null ? Container.Resolve<ScoreLogic>() : ScoreLogic;
        }

        void Update()
        {
            Text.text = ScoreLogic.CurrentScore.ToString();
        }
    }
}

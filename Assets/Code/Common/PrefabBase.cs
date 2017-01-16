namespace Assets.Code.Common
{
    using DataAccess;
    using GameLogic;
    using IoC;
    using MonoBehaviours.Configuration;
    using UnityEngine;

    public abstract class PrefabBase : MonoBehaviour
    {
        #region Properties
        protected IoC Container { get; set; }
        protected PrefabManager PrefabManager { get; private set; }
        protected ScoreLogic ScoreLogic { get; private set; }
        protected GlobalConfiguration Configuration { get; private set; }
        #endregion

        public virtual void Activate(IoC container)
        {
            Container = container;
            PrefabManager = PrefabManager == null ? Container.Resolve<PrefabManager>() : PrefabManager;
            ScoreLogic = ScoreLogic == null ? Container.Resolve<ScoreLogic>() : ScoreLogic;
            Configuration = Configuration == null ? PrefabManager.GetConfiguration() : Configuration;
        }
    }
}

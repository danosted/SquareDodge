namespace Assets.Code.Common
{
    using DataAccess;
    using IoC;
    using MonoBehaviours.Configuration;

    public class LogicBase
    {
        #region Properties
        protected IoC Container { get; set; }
        protected PrefabManager PrefabManager { get; private set; }
        protected GlobalConfiguration Configuration { get; private set; }
        #endregion
        
        #region Constructors
        public LogicBase(IoC container, PrefabManager prefabManager, GlobalConfiguration config)
        {
            Container = container;
            PrefabManager = prefabManager;
            Configuration = config;
        }
        #endregion
    }
}

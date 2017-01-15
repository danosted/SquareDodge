namespace Assets.Code.Common
{
    using DataAccess;
    using IoC;
    using MonoBehaviours.Configuration;
    using System.Collections.Generic;
    using UnityEngine;

    public class LogicBase
    {
        #region Properties
        protected IoC Container { get; set; }
        protected PrefabManager PrefabManager { get; set; }
        protected GlobalConfiguration Configuration { get; set; }
        protected ICollection<Object> ActivePrefabs { get; set; }
        protected ICollection<Object> InactivePrefabs { get; set; }
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

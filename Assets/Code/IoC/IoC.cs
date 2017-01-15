namespace Assets.Code.IoC
{
    using DataAccess;
    using MonoBehaviours.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class IoC
    {
        private ICollection<object> _container { get; set; }
        private PrefabManager PrefabManager { get; set; }
        private GlobalConfiguration Configuration { get; set; }

        public IoC(GlobalConfiguration config)
        {
            // Initialize Container
            _container = new HashSet<object>();

            // Initialize PrefabManager
            PrefabManager = Resolve<PrefabManager>();

            // Initialize GlobalConfiguration
            Configuration = PrefabManager.GetPrefab(config);
        }

        public T Resolve<T>()
        {
            var entity = (T)_container.FirstOrDefault(e => e.GetType() == typeof(T));

            if (entity != null)
            {
                return entity;
            }

            try
            {
                entity = (T)Activator.CreateInstance(typeof(T), new object[] { this, PrefabManager, Configuration });
            }
            catch (Exception e)
            {
                entity = (T)Activator.CreateInstance(typeof(T), new object[] { this });
            }
            _container.Add(entity);

            return entity;
        }

        private T CreateInstance<T>() where T : new()
        {
            return (T)Activator.CreateInstance(typeof(T), new object[] { });
        }
    }
}

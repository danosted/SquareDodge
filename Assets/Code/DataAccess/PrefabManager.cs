namespace Assets.Code.DataAccess
{
    using IoC;
    using MonoBehaviours.Configuration;
    using MonoBehaviours.UserInterface;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class PrefabManager
    {
        private object semaLock = new object();
        private IoC _container;
        private ICollection<Object> _activePrefabs;
        private ICollection<Object> _inactivePrefabs;

        public PrefabManager(IoC ioc)
        {
            _container = ioc;
            _activePrefabs = new List<Object>();
            _inactivePrefabs = new List<Object>();
        }

        public T GetPrefab<T>(T prefab) where T : Object
        {
            lock (semaLock)
            {
                var instance = (T)_inactivePrefabs.FirstOrDefault(p => p.GetType() == typeof(T) && p.name.Equals(prefab.name + "(Clone)"));
                //if (instance == null)
                //{
                //    instance = (T)_inactivePrefabs.FirstOrDefault(p => p.GetType() == typeof(T));
                //}
                if (instance != null)
                {
                    _inactivePrefabs.Remove(instance);
                    _activePrefabs.Add(instance);
                    return instance;
                }

                if (prefab == null)
                {
                    throw new UnityException(string.Format("Prefab was null. Type '{0}'.", typeof(T)));
                }
                instance = Object.Instantiate(prefab);
                _activePrefabs.Add(instance);
                return instance;
            }
        }

        public T GetActivePrefab<T>(T prefab) where T : Object
        {
            lock (semaLock)
            {
                var instance = (T)_activePrefabs.FirstOrDefault(p => p.GetType() == typeof(T));
                if (instance == null)
                {
                    throw new UnityException(string.Format("Prefab was not part of active prefabs. Type '{0}'.", typeof(T)));
                }
                return instance;
            }
        }

        public GlobalConfiguration GetConfiguration()
        {
            var config = _activePrefabs.FirstOrDefault(p => p.GetType() == typeof(GlobalConfiguration)) as GlobalConfiguration;
            if (config != null)
            {
                return config;
            }
            throw new UnityException("GlobalConfiguration not initialized.");
        }

        public void ReturnPrefab(Object prefab)
        {
            if (!_activePrefabs.Any(p => p.Equals(prefab)))
            {
                Object.Destroy(prefab);
                throw new UnityException(string.Format("Prefab was not part of active prefabs."));
            }
            var go = prefab as MonoBehaviour;
            if (go != null)
            {
                go.gameObject.SetActive(false);
            }
            _activePrefabs.Remove(prefab);
            _inactivePrefabs.Add(prefab);
        }

        internal void Shutdown()
        {
            lock (semaLock)
            {
                var except = new [] { typeof(GlobalConfiguration), typeof(CanvasManager) };
                var tempList = _activePrefabs.Where(ap => !except.Contains(ap.GetType())).ToList();
                foreach (var p in tempList)
                {
                    ((MonoBehaviour)p).gameObject.SetActive(false);
                    _activePrefabs.Remove(p);
                    _inactivePrefabs.Add(p);
                }
            }
        }
    }
}

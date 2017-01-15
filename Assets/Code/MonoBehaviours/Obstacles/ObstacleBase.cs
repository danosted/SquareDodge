namespace Assets.Code.MonoBehaviours.Obstacles
{
    using UnityEngine;
    using IoC;
    using DataAccess;
    using GameLogic;

    public class ObstacleBase : MonoBehaviour
    {
        protected IoC Container { get; set; }
        protected int Level { get; set; }
        protected float Speed { get; set; }
        protected PrefabManager PrefabManager { get; set; }
        protected ObstacleLogic ObstacleLogic { get; set; }
        
        public virtual void Activate(IoC ioc, int level, Vector3 intialPosition)
        {
            Container = ioc;
            Level = level;
            Speed = 1f;
            transform.position = intialPosition;
            gameObject.SetActive(true);
            PrefabManager = PrefabManager == null ? Container.Resolve<PrefabManager>() : PrefabManager;
            ObstacleLogic = ObstacleLogic == null ? Container.Resolve<ObstacleLogic>() : ObstacleLogic;
        }

        public void Deactivate()
        {
            PrefabManager.ReturnPrefab(this);
        }

        protected virtual void Update()
        {
            Move();
            CheckOutOfBounds();
        }

        protected virtual void Move()
        {
            transform.position = transform.position + (Vector3.down * Time.deltaTime * Speed * Level);
        }

        protected virtual void CheckOutOfBounds()
        {
            if(!Container.Resolve<ScreenLogic>().IsOutOfViewportBounds(transform.position))
            {
                return;
            }
            Deactivate();
        }

        protected virtual void OnMouseEnter()
        {
            Deactivate();
        }
    }
}

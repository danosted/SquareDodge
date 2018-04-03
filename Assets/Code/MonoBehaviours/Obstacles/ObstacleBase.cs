namespace Assets.Code.MonoBehaviours.Obstacles
{
    using UnityEngine;
    using IoC;
    using DataAccess;
    using GameLogic;
    using Common;
    using Utilities;
    using Assets.Code.MonoBehaviours.UserInterface;

    public class ObstacleBase : PrefabBase
    {
        protected int Level { get; set; }
        protected float Speed { get; set; }
        protected ObstacleLogic ObstacleLogic { get; private set; }
        protected UserInterfaceLogic UserInterfaceLogic { get; private set; }
        protected ScoreLogic ScoreLogic { get; private set; }
        protected Player _player { get; private set; }

        public virtual void Activate(IoC container, int level, Vector3 intialPosition)
        {
            base.Activate(container);
            ObstacleLogic = ObstacleLogic == null ? Container.Resolve<ObstacleLogic>() : ObstacleLogic;
            UserInterfaceLogic = UserInterfaceLogic == null ? Container.Resolve<UserInterfaceLogic>() : UserInterfaceLogic;
            ScoreLogic = ScoreLogic == null ? Container.Resolve<ScoreLogic>() : ScoreLogic;
            Level = level;
            Speed = 1f;
            transform.position = intialPosition;
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
            transform.Translate(Vector3.down * Time.deltaTime * Speed * Level);
        }

        protected virtual void CheckOutOfBounds()
        {
            if (!Container.Resolve<ScreenUtil>().IsOutOfViewportBounds(transform.position))
            {
                return;
            }
            Deactivate();
        }

        protected virtual void OnMouseEnter()
        {
            ScoreLogic.AddToScore(-Level);
            Deactivate();
        }
    }
}

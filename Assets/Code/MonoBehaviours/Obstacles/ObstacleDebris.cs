namespace Assets.Code.MonoBehaviours.Obstacles
{
    using UnityEngine;
    using IoC;

    public class ObstacleDebris : ObstacleBase
    {
        private Vector3 _rotation;
        private Vector3 _direction;

        public override void Activate(IoC ioc, int level, Vector3 intialPosition)
        {
            base.Activate(ioc, level, intialPosition);
            _rotation = new Vector3(0f, 0f, Random.Range(-1f, 1f));
            var randomHorizontalDirection = Random.Range(-1f, 1f);
            var randomVerticalDirection = Random.Range(-1f, 0.2f);
            _direction = new Vector3(randomHorizontalDirection, randomVerticalDirection, 0f).normalized;
        }

        protected override void Move()
        {
            
            transform.position = transform.position + (_direction.normalized * Time.deltaTime * Speed * Level);
            transform.Rotate(_rotation);

            // simulated gravity
            _direction.x = Mathf.MoveTowards(_direction.x, 0f, Time.deltaTime);
            _direction.y = Mathf.MoveTowards(_direction.y, -1f, Time.deltaTime);
            Speed += Time.deltaTime;
        }
    }
}

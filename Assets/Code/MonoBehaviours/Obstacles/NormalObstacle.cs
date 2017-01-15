namespace Assets.Code.MonoBehaviours.Obstacles
{
    using UnityEngine;
    using GameLogic;

    public class NormalObstacle : ObstacleBase
    {
        void OnTriggerEnter2D(Collider2D other)
        {
            // TODO 1 (DRO): destroy obstacle and spawn debris if collided with other obstacle
            var otherObst = other.GetComponent<NormalObstacle>();
            if (otherObst == null)
            {
                return;
            }
            ObstacleLogic.SpawnObstacleDebris(transform.position, 10);
            Deactivate();
        }
    }
}

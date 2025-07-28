using Player;
using UnityEngine;

namespace Obstacles.Obstacles {
    public class StaticObstacle : ObstacleBase {
        [SerializeField] private float _obstacleHeight;
        public override float Height => _obstacleHeight;

        public override ObstacleBase InstantiateNew(Vector3 pos, Transform parent) {
            var newObstacle = Instantiate(gameObject, pos, Quaternion.identity);
            newObstacle.transform.SetParent(parent, false);
            return newObstacle.GetComponent<ObstacleBase>();
        }

        protected override void OnTriggerEnter(Collider other) {
            if (other.TryGetComponent<PlayerController>(out var player)) {
                player.TakeDamage();
                Destroy(gameObject);
            }
        }
    }
}
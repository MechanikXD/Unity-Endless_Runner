using Player;
using UnityEngine;

namespace Obstacles.Obstacles {
    public class StaticObstacle : ObstacleBase {
        public override ObstacleBase InstantiateNew(Vector3 pos, Transform parent) {
            var newObstacle = Instantiate(this, pos, Quaternion.identity);
            newObstacle.transform.SetParent(parent, false);
            return newObstacle;
        }

        protected override void OnTriggerEnter(Collider other) {
            if (other.TryGetComponent<PlayerController>(out var player)) {
                player.TakeDamage();
                Destroy(gameObject);
            }
        }
    }
}
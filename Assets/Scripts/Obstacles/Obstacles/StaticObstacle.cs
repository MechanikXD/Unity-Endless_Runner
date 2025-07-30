using Obstacles.Collectible;
using Player;
using UnityEngine;

namespace Obstacles.Obstacles {
    public class StaticObstacle : ObstacleBase {
        [SerializeField] private bool _canHaveCollectible;
        [SerializeField] private Transform _collectibleSpawnPoint;
        [SerializeField] private CollectibleBase[] _collectiblePool;

        public override bool CanSpawnCollectible => _canHaveCollectible;

        public override ObstacleBase InstantiateNew(Vector3 pos, Transform parent) {
            var newObstacle = Instantiate(this, pos, Quaternion.identity);
            newObstacle.transform.SetParent(parent, false);
            return newObstacle;
        }

        public override CollectibleBase SpawnCollectible(Transform parent) {
            if (_canHaveCollectible) {
                return _collectiblePool[Random.Range(0, _collectiblePool.Length)]
                    .InstantiateNew(_collectibleSpawnPoint.localPosition, parent);
            }
            else return null;
        }

        protected override void OnTriggerEnter(Collider other) {
            if (other.TryGetComponent<PlayerController>(out var player)) {
                player.TakeDamage();
                Destroy(gameObject);
            }
        }
    }
}
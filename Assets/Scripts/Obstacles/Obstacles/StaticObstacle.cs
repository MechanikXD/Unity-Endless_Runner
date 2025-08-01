using Obstacles.Collectible;
using Player;
using UnityEngine;

namespace Obstacles.Obstacles {
    public class StaticObstacle : ObstacleBase {
        [SerializeField] private bool _canHaveCollectible;
        [SerializeField] private Transform _collectibleSpawnPoint;
        [SerializeField] private CollectibleBase[] _collectiblePool;

        public override bool CanSpawnCollectible => _canHaveCollectible;

        public override CollectibleBase SpawnCollectible(Transform parent) {
            if (_canHaveCollectible) {
                return _collectiblePool[Random.Range(0, _collectiblePool.Length)]
                    .InstantiateNew(_collectibleSpawnPoint.localPosition, parent);
            }
            else return null;
        }

        protected override void OnPlayerCollision(PlayerController player) {
            player.TakeDamage();
            Destroy(gameObject);
        }
    }
}
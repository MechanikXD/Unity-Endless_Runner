using Obstacles.Collectible;
using Player;
using UnityEngine;

namespace Obstacles {
    public abstract class ObstacleBase : MonoBehaviour {
        public abstract bool CanSpawnCollectible { get; }

        public virtual CollectibleBase SpawnCollectible(Transform parent) => null;

        private void OnTriggerEnter(Collider other) {
            if (other.TryGetComponent<PlayerController>(out var player)) OnPlayerCollision(player);
        }

        protected abstract void OnPlayerCollision(PlayerController player);
    }
}

using Obstacles.Collectible;
using UnityEngine;

namespace Obstacles {
    public abstract class ObstacleBase : MonoBehaviour {
        public abstract bool CanSpawnCollectible { get; }

        public virtual CollectibleBase SpawnCollectible(Transform parent) => null;
        protected abstract void OnTriggerEnter(Collider other);
    }
}

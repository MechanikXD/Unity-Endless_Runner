using Obstacles.Collectible;
using UnityEngine;

namespace Obstacles {
    public abstract class ObstacleBase : MonoBehaviour {
        public abstract bool CanSpawnCollectible { get; }
        
        public abstract ObstacleBase InstantiateNew(Vector3 pos, Transform parent);

        public virtual CollectibleBase SpawnCollectible(Transform parent) => null;
        protected abstract void OnTriggerEnter(Collider other);
    }
}

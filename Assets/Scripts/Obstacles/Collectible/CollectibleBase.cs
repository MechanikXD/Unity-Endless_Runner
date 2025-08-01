using Player;
using UnityEngine;

namespace Obstacles.Collectible {
    public abstract class CollectibleBase : MonoBehaviour {
        [SerializeField] protected int _scoreGiven;

        public abstract CollectibleBase InstantiateNew(Vector3 position, Transform parent);

        protected abstract void OnPlayerCollision(PlayerController player);

        protected void OnTriggerEnter(Collider other) {
            if (other.TryGetComponent<PlayerController>(out var player)) OnPlayerCollision(player);
        }
    }
}
using Core.Score;
using Player;
using UnityEngine;

namespace Obstacles.Collectible {
    public abstract class CollectibleBase : MonoBehaviour {
        [SerializeField] protected int _scoreGiven;

        public abstract CollectibleBase InstantiateNew(Vector3 position, Transform parent);

        protected void OnTriggerEnter(Collider other) {
            if (other.TryGetComponent<PlayerController>(out _)) {
                ScoreManager.AddScore(_scoreGiven);
                Destroy(gameObject);
            }
        }
    }
}
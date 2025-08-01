using Core.Score;
using Player;
using UnityEngine;

namespace Obstacles.Collectible.Collectibles {
    public class SimpleCollectible : CollectibleBase {
        public override CollectibleBase InstantiateNew(Vector3 position, Transform parent) {
            var collectible = Instantiate(this, position, Quaternion.identity);
            collectible.transform.SetParent(parent, false);
            return collectible;
        }

        protected override void OnPlayerCollision(PlayerController player) {
            ScoreManager.AddScore(_scoreGiven);
            Destroy(gameObject);
        }
    }
}
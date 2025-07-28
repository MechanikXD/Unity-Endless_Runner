using UnityEngine;

namespace Core.LevelGeneration {
    public class Platform : MonoBehaviour {
        [SerializeField] private float _platformLength;
        // [SerializeField] private Transform[] _obstacleSpawnPoints;
        // [SerializeField] private Obstacle[] _obstaclePool;
        public float Length => _platformLength;
        public Vector3 Position => transform.position;

        public Platform InstantiateNew(Vector3 pos, Transform parent) {
            return Instantiate(gameObject, pos, Quaternion.identity, parent).GetComponent<Platform>();
        }
        public void ClearObject() {}
        public void PlaceRandomObject() {}

        public void MoveTo(Vector3 position) {
            transform.localPosition = position;
        }
    }
}
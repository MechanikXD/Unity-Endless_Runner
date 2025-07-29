using UnityEngine;

namespace Obstacles {
    [RequireComponent(typeof(Rigidbody))]
    public abstract class ObstacleBase : MonoBehaviour {
        public abstract ObstacleBase InstantiateNew(Vector3 pos, Transform parent);
        protected abstract void OnTriggerEnter(Collider other);
    }
}

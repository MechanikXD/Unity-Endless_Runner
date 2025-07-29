using System.Collections.Generic;
using Obstacles;
using UnityEngine;

namespace Core.LevelGeneration {
    public class Platform : MonoBehaviour {
        [SerializeField] private float _platformLength;
        [SerializeField] private Transform[] _obstacleSpawnPoints;
        [SerializeField] private ObstacleBase[] _obstaclePool;
        private List<ObstacleBase> _createdObstacles = new List<ObstacleBase>();

        [SerializeField] private float _obstacleSpawnChance;
        [SerializeField] private float _secondObstacleChance;
        public float Length => _platformLength;
        public Vector3 Position => transform.position;

        public Platform InstantiateNew(Vector3 pos, Transform parent) {
            return Instantiate(gameObject, pos, Quaternion.identity, parent).GetComponent<Platform>();
        }

        public void ClearObject() {
            foreach (var obstacle in _createdObstacles) {
                if (obstacle != null) Destroy(obstacle.gameObject);
            }
            _createdObstacles.Clear();
        }

        public void PlaceRandomObject() {
            if (Random.value < _obstacleSpawnChance) {
                var firstIndex = Random.Range(0, _obstacleSpawnPoints.Length);
                var randomPoint = _obstacleSpawnPoints[firstIndex];
                var randomObstacle = _obstaclePool[Random.Range(0, _obstaclePool.Length)];

                _createdObstacles.Add(randomObstacle.InstantiateNew(Vector3.zero, randomPoint));

                if (Random.value < _secondObstacleChance) {
                    var secondIndex = Random.Range(0, _obstacleSpawnPoints.Length);
                    if (firstIndex == secondIndex) {
                        secondIndex = secondIndex + 1 == _obstacleSpawnPoints.Length ? 0 : secondIndex + 1;
                    }
                    
                    var otherPoint = _obstacleSpawnPoints[secondIndex];
                    var otherObstacle = _obstaclePool[Random.Range(0, _obstaclePool.Length)];

                    _createdObstacles.Add(otherObstacle.InstantiateNew(Vector3.zero, otherPoint));
                }
            }
        }

        public void MoveTo(Vector3 position) {
            transform.localPosition = position;
        }
    }
}
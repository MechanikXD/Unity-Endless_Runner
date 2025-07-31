using System.Collections.Generic;
using Obstacles;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.LevelGeneration {
    public class Platform : MonoBehaviour {
        [SerializeField] private float _platformLength;
        [SerializeField] private Transform[] _obstacleSpawnPoints;
        [SerializeField] private ObstacleBase[] _obstaclePool;
        private List<GameObject> _createdObstacles = new List<GameObject>();

        [SerializeField] private float _obstacleSpawnChance;
        [SerializeField] private float _secondObstacleChance;
        [SerializeField] private float _collectibleSpawnChance;
        public float Length => _platformLength;
        public Vector3 Position => transform.position;

        public Platform InstantiateNew(Vector3 pos, Transform parent) {
            return Instantiate(gameObject, pos, Quaternion.identity, parent).GetComponent<Platform>();
        }

        public void ClearObject() {
            foreach (var obstacle in _createdObstacles) {
                if (obstacle != null) Destroy(obstacle);
            }
            _createdObstacles.Clear();
        }

        private static int[] ShuffleIndexes(int arraySize) {
            var array = new int[arraySize];
            for (var i = 0; i < arraySize; i++) {
                array[i] = i;
            }
            
            while (arraySize > 1) {
                arraySize--;
                var otherIndex = Random.Range(0, arraySize + 1);
                (array[otherIndex], array[arraySize]) = (array[arraySize], array[otherIndex]);
            }

            return array;
        }

        public void PlaceRandomObject() {
            // Shuffle indexes instead of picking random, array must have more than 2 point.
            var indexes = ShuffleIndexes(_obstacleSpawnPoints.Length);

            if (Random.value < _obstacleSpawnChance) {
                // First Obstacle
                var randomPoint = _obstacleSpawnPoints[indexes[0]];
                var randomObstacle = _obstaclePool[Random.Range(0, _obstaclePool.Length)];

                if (Random.value < _collectibleSpawnChance && randomObstacle.CanSpawnCollectible) {
                    var randomCollectible = randomObstacle.SpawnCollectible(randomPoint);
                    _createdObstacles.Add(randomCollectible.gameObject);
                }

                _createdObstacles.Add(randomObstacle.InstantiateNew(Vector3.zero, randomPoint).gameObject);

                if (Random.value < _secondObstacleChance) {
                    // Second Obstacle
                    var otherPoint = _obstacleSpawnPoints[indexes[1]];
                    var otherObstacle = _obstaclePool[Random.Range(0, _obstaclePool.Length)];

                    if (Random.value < _collectibleSpawnChance &&
                        otherObstacle.CanSpawnCollectible) {
                        var otherCollectible = otherObstacle.SpawnCollectible(otherPoint);
                        _createdObstacles.Add(otherCollectible.gameObject);
                    }
                    
                    _createdObstacles.Add(otherObstacle.InstantiateNew(Vector3.zero, otherPoint).gameObject);
                }
            }
        }

        public void MoveTo(Vector3 position) {
            transform.localPosition = position;
        }
    }
}
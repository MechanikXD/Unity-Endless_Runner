using System.Collections.Generic;
using Obstacles;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Core.LevelGeneration {
    public class Platform : MonoBehaviour {
        [SerializeField] private float _platformLength;
        [SerializeField] private Transform[] _obstacleSpawnPoints;
        [FormerlySerializedAs("_obstaclePool")]
        [SerializeField] private ObstacleBase[] _possibleObstacles;  // Obstacles to generate from
        private List<GameObject> _createdObstacles = new List<GameObject>();  // To store existing obstacles

        [SerializeField] private float _obstacleSpawnChance;
        [SerializeField] private float _secondObstacleChance;
        [SerializeField] private float _collectibleSpawnChance;
        public float Length => _platformLength;
        public Vector3 Position => transform.position;

        /// <summary>
        /// Clears attached obstacles to this platform
        /// </summary>
        public void ClearObject() {
            foreach (var obstacle in _createdObstacles) {
                if (obstacle != null) Destroy(obstacle);
            }
            _createdObstacles.Clear();
        }
        
        /// <summary>
        /// Creates array of shuffled indexes starting from 0.
        /// </summary>
        /// <param name="arraySize"> Amount of indexes or objects in a array </param>
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

        /// <summary>
        /// Randomly places obstacles/collectibles on the platform 
        /// </summary>
        public void PlaceRandomObject() {
            // Shuffle indexes instead of picking random, array must have more than 2 point.
            var indexes = ShuffleIndexes(_obstacleSpawnPoints.Length);

            if (Random.value < _obstacleSpawnChance) {
                // First Obstacle
                var randomPoint = _obstacleSpawnPoints[indexes[0]];
                var randomObstacle = _possibleObstacles[Random.Range(0, _possibleObstacles.Length)];

                if (Random.value < _collectibleSpawnChance && randomObstacle.CanSpawnCollectible) {
                    var randomCollectible = randomObstacle.SpawnCollectible(randomPoint);
                    _createdObstacles.Add(randomCollectible.gameObject);
                }

                _createdObstacles.Add(Instantiate(randomObstacle, Vector3.zero, Quaternion.identity,
                    randomPoint).gameObject);

                if (Random.value < _secondObstacleChance) {
                    // Second Obstacle
                    var otherPoint = _obstacleSpawnPoints[indexes[1]];
                    var otherObstacle = _possibleObstacles[Random.Range(0, _possibleObstacles.Length)];

                    if (Random.value < _collectibleSpawnChance &&
                        otherObstacle.CanSpawnCollectible) {
                        var otherCollectible = otherObstacle.SpawnCollectible(otherPoint);
                        _createdObstacles.Add(otherCollectible.gameObject);
                    }

                    _createdObstacles.Add(Instantiate(otherObstacle, Vector3.zero,
                        Quaternion.identity, otherPoint).gameObject);
                }
            }
        }

        public void MoveTo(Vector3 position) => transform.localPosition = position;
    }
}
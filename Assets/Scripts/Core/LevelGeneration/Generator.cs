using System.Collections.Generic;
using UnityEngine;

namespace Core.LevelGeneration {
    public class Generator : MonoBehaviour {
        [SerializeField] private Transform _levelOrigin;
        [SerializeField] private Platform _platformPrefab;
        [SerializeField] private int _platformCount;
        private Queue<Platform> _platformPool;
        private Vector3 _lastPlatformPosition;
        
        private Transform _platformParent;
        private Vector3 _platformParentPosition;
        [SerializeField] private Vector3 _platformNotVisiblePosition;
        [SerializeField] private float _moveSpeed;
        
        private void Awake() {
            _platformParent = new GameObject("Level").transform;
            _lastPlatformPosition = _levelOrigin.position;
            _platformParentPosition = _platformParent.position;
            
            _platformPool = new Queue<Platform>(_platformCount);
            for (var _ = 0; _ < _platformCount; _++) {
                var platformPosition = NextPlatformPosition();
                var newPlatform = Instantiate(_platformPrefab, platformPosition,
                    Quaternion.identity, _platformParent);
                
                _platformPool.Enqueue(newPlatform);
                _lastPlatformPosition = platformPosition;
            }
        }

        private void Update() {
            _platformParentPosition.x += _moveSpeed * Time.deltaTime;
            _platformParent.position = _platformParentPosition;

            if (OutsidePlayerVisibility(_platformPool.Peek())) RepositionLastPlatform();
        }

        private Vector3 NextPlatformPosition() {
            var nextPos = _lastPlatformPosition;
            nextPos.x -= _platformPrefab.Length;
            return nextPos;
        }

        private bool OutsidePlayerVisibility(Platform platform) {
            return platform.Position.x - _platformNotVisiblePosition.x >= 0;
        }

        private void RepositionLastPlatform() {
            var lastPlatform = _platformPool.Dequeue();
            
            lastPlatform.ClearObject();
            lastPlatform.PlaceRandomObject();
            
            var newPosition = NextPlatformPosition();
            _lastPlatformPosition = newPosition;
            lastPlatform.MoveTo(newPosition);
            
            _platformPool.Enqueue(lastPlatform);
        }
    }
}
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour {
        [SerializeField] private Transform _leftPosition;
        [SerializeField] private Transform _middlePosition;
        [SerializeField] private Transform _rightPosition;
        private Tween _currentMovement;
        private int _lastPlatformPositions;

        [SerializeField] private float _repositionTime;
        [SerializeField] private float _jumpDuration;
        [SerializeField] private Vector3 _jumpElevation;
        [SerializeField] private float _crouchTime;
        [SerializeField] private Vector3 _crouchScale;

        private void Awake() {
            // Start at middle platform.
            MoveToMiddlePlatform(0f);
        }

        private void MoveToLeftPlatform(float duration) {
            // TODO: Buffer or take over last movement
            if (_currentMovement.IsActive()) return;
            
            _currentMovement = transform.DOMove(_leftPosition.localPosition, duration);
            _lastPlatformPositions = -1;
        }

        private void MoveToMiddlePlatform(float duration) {
            if (_currentMovement.IsActive()) return;
            
            _currentMovement = transform.DOMove(_middlePosition.localPosition, duration);
            _lastPlatformPositions = 0;
        }

        private void MoveToRightPlatform(float duration) {
            if (_currentMovement.IsActive()) return;
            
            _currentMovement = transform.DOMove(_rightPosition.localPosition, duration);
            _lastPlatformPositions = 1;
        }

        private void TryJump() {
            if (_currentMovement.IsActive()) return;

            var originalPosition = transform.position;
            var newPosition = originalPosition + _jumpElevation;
            
            var animationSequence = DOTween.Sequence();
            animationSequence.Append(transform.DOMove(newPosition, _repositionTime));
            animationSequence.Append(transform.DOMove(originalPosition, _repositionTime).SetDelay(_jumpDuration));

            _currentMovement = animationSequence.Play();
        }
        
        private void TryCrouch() {
            if (_currentMovement.IsActive()) return;

            var playerTransform = transform;
            var originalScale = playerTransform.localScale;
            var originalPosition = playerTransform.position;
            var adjustedPosition = originalPosition;
            adjustedPosition.y /= 2f;
            
            var animationSequence = DOTween.Sequence();
            animationSequence.Append(transform.DOScale(_crouchScale, _repositionTime)
                .OnComplete(() => transform.position = adjustedPosition));
            animationSequence.Append(transform.DOScale(originalScale, _repositionTime)
                .SetDelay(_crouchTime).OnComplete(() => transform.position = originalPosition));

            _currentMovement = animationSequence.Play();
        }

        #region Input System Events

        public void OnLeft() {
            switch (_lastPlatformPositions) {
                case 1:
                    MoveToMiddlePlatform(_repositionTime);
                    break;
                case 0:
                    MoveToLeftPlatform(_repositionTime);
                    break;
                case -1:
                    break;
            }
        }

        public void OnRight() {
            switch (_lastPlatformPositions) {
                case 1:
                    break;
                case 0:
                    MoveToRightPlatform(_repositionTime);
                    break;
                case -1:
                    MoveToMiddlePlatform(_repositionTime);
                    break;
            }
        }

        public void OnUp() => TryJump();

        public void OnDown() => TryCrouch();

        #endregion
    }
}
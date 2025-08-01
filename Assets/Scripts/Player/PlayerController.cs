using System;
using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour {
        [SerializeField] private Animator _animator;
        [SerializeField] private float _crossFade;
        [SerializeField] private int _maxHealth;
        [SerializeField] private float _iFramesDuration;
        private bool _inIFrames;
        private int _currentHealth;
        [SerializeField] private Transform _leftPosition;
        [SerializeField] private Transform _middlePosition;
        [SerializeField] private Transform _rightPosition;
        private int _lastPlatformPositions;
        // Target values
        private Vector3 _targetPosition;
        private Vector3 _targetScale = Vector3.one;
        // Movement state
        private bool _isMoving;
        private bool _isScaling;
        private bool _isJumpingOrCrouching;
        // Original value to revert to after jump or crouch
        private float _originalHeight;
        // Float error
        private const float CalcError = 0.05f;
        [Header("Movement and Jump")]
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _jumpElevation;
        [SerializeField] private float _crouchOrJumpDuration = 0.75f;
        private bool _canJump = true;
        [Header("Crouch")]
        [SerializeField] private float _scaleSpeed;
        [SerializeField] private Vector3 _crouchScale;
        private bool _canCrouch = true;

        private Coroutine _setOriginalHeightLaterCoroutine;
        private Coroutine _removeIFramesLaterCoroutine;

        // Some event to throw
        public static event Action Defeated;

        private void OnEnable() {
            if (_setOriginalHeightLaterCoroutine != null) StartCoroutine(SetOriginalHeightLater());
            if (_removeIFramesLaterCoroutine != null) StartCoroutine(RemoveIFramesLater());
        }

        private void OnDisable() {
            if (_setOriginalHeightLaterCoroutine != null) StopCoroutine(SetOriginalHeightLater());
            if (_removeIFramesLaterCoroutine != null) StartCoroutine(RemoveIFramesLater());
        }

        #region Movement
        
        private void Awake() {
            // Start at middle platform.
            var middle = _middlePosition.localPosition;
            SetDesiredHeight(middle.y, true);
            SetDesiredPosition(middle, true);
            _originalHeight = transform.position.y;
            _currentHealth = _maxHealth;
        }

        private void Update() {
            // Movement
            var distanceToTarget = Vector3.Distance(transform.position, _targetPosition);

            if (distanceToTarget > CalcError) {
                _isMoving = true;
                transform.position = Vector3.Lerp(transform.position, _targetPosition,
                    _moveSpeed * Time.deltaTime);
            }
            else if (_isMoving) {
                // Snap when close enough
                _isMoving = false;
                transform.position = _targetPosition;
                _animator.CrossFade("Normal", _crossFade);
            }

            // Scale
            var scaleDifference = Vector3.Distance(transform.position, _targetPosition);

            if (scaleDifference > CalcError) {
                _isScaling = true;
                transform.localScale =
                    Vector3.Lerp(transform.localScale, _targetScale, _scaleSpeed * Time.deltaTime);
            }
            else if (_isScaling) {
                // Snap when close enough
                _isScaling = false;
                transform.localScale = _targetScale;
            }
        }

        private void SetDesiredPosition(Vector3 position, bool isInstant = false) {
            position = new Vector3(position.x, _targetPosition.y, position.z);

            if (isInstant) {
                transform.position = position;
                _targetPosition = position;
            }
            else _targetPosition = position;
        }

        private void SetDesiredHeight(float y, bool isInstant = false) {
            if (isInstant) {
                var currentTransform = transform;
                var position = currentTransform.position;
                position.y = y;
                currentTransform.position = position;
                _targetPosition.y = y;
            }
            else {
                _targetPosition.y = y;
            }
        }

        private void SetDesiredScale(Vector3 scale, bool isInstant = false) {
            if (isInstant) {
                transform.localScale = scale;
                _targetScale = scale;
            }
            else _targetScale = scale;
        }

        private void Jump() {
            if (_isJumpingOrCrouching || !_canJump) return;
            
            _animator.CrossFade("Tilt Up", _crossFade);
            SetDesiredHeight(_originalHeight + _jumpElevation);
            _isJumpingOrCrouching = true;
            _canJump = false;

            _setOriginalHeightLaterCoroutine = StartCoroutine(SetOriginalHeightLater());
        }

        private void Crouch() {
            if (_isJumpingOrCrouching || !_canCrouch) return;
            
            _animator.CrossFade("Tilt Down", _crossFade);
            SetDesiredHeight(_crouchScale.y / 2);
            SetDesiredScale(_crouchScale);
            _isJumpingOrCrouching = true;
            _canCrouch = false;

            _setOriginalHeightLaterCoroutine = StartCoroutine(SetOriginalHeightLater());
        }
        
        private IEnumerator SetOriginalHeightLater() {
            yield return new WaitForSeconds(_crouchOrJumpDuration);
            SetDesiredHeight(_originalHeight);
            SetDesiredScale(Vector3.one);   // Note: scale must be vector.one by default
            _isJumpingOrCrouching = false;
            yield return new WaitUntil(AtOriginalHeight);
            _canCrouch = true;
            _setOriginalHeightLaterCoroutine = null;
        }

        private bool AtOriginalHeight() => Math.Abs(transform.position.y - _originalHeight) < 0.01f;
        
        #endregion

        #region Input System Events

        public void OnLeft() {
            switch (_lastPlatformPositions) {
                case 1:
                    SetDesiredPosition(_middlePosition.localPosition);
                    _lastPlatformPositions = 0;
                    break;
                case 0:
                    SetDesiredPosition(_leftPosition.localPosition);
                    _lastPlatformPositions = -1;
                    break;
                case -1:
                    // Already there
                    return;
            }
            _animator.CrossFade("Tilt Left", _crossFade);
        }

        public void OnRight() {
            switch (_lastPlatformPositions) {
                case 1:
                    // Already there
                    return;
                case 0:
                    SetDesiredPosition(_rightPosition.localPosition);
                    _lastPlatformPositions = 1;
                    break;
                case -1:
                    SetDesiredPosition(_middlePosition.localPosition);
                    _lastPlatformPositions = 0;
                    break;
            }
            _animator.CrossFade("Tilt Right", _crossFade);
        }

        public void OnUp() => Jump();

        public void OnDown() => Crouch();

        public void OnPause() => UIManager.ShowPauseMenu();

        #endregion

        public void TakeDamage() {
            if (_inIFrames) return;
            
            _currentHealth -= 1;
            if (_currentHealth <= 0) {
                enabled = false;    // disable this script/controller
                Defeated?.Invoke();
            }
            else {
                _inIFrames = true;

                _removeIFramesLaterCoroutine = StartCoroutine(RemoveIFramesLater());
            }
        }
        
        private IEnumerator RemoveIFramesLater() {
            yield return new WaitForSeconds(_iFramesDuration);
            _inIFrames = false;
            _removeIFramesLaterCoroutine = null;
        }
    }
}
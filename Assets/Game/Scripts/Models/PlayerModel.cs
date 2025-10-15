using UnityEngine;

namespace Models
{
    public class PlayerModel : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        
        [Header("Move Settings")] [SerializeField]
        private float forwardSpeed;

        [SerializeField] private float laneDistance;
        [SerializeField] private float laneChangeSpeed;

        [Header("Jump / Gravity")] 
        [SerializeField] private float jumpForce;
        [SerializeField] private float gravity;

        private CharacterController _controller;
        
        private Vector3 _moveDirection;
        private Vector2 _touchStart;
        
        private int _currentLane = 1;
        
        private float _verticalVelocity;
      
        private bool _isSwiping;
        private bool _isDead = false;
        
        private const float SWIPE_THRESHOLD = 50f;

        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            
            if (animator == null)
                Debug.LogWarning("Animator is not assigned on PlayerController.");

            if (animator != null)
            {
                animator.SetBool("Running", true);
                animator.SetBool("Death", false);
            }
        }

        private void Update()
        {
            if (_isDead) return;

            HandleInput();
            MovePlayer();
            UpdateAnimator();
        }

        private void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                ChangeLane(-1);
            if (Input.GetKeyDown(KeyCode.RightArrow))
                ChangeLane(1);
            if (Input.GetKeyDown(KeyCode.UpArrow))
                TryJump();
            
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                _touchStart = Input.mousePosition;
                _isSwiping = true;
            }
            else if (Input.GetMouseButtonUp(0) && _isSwiping)
            {
                Vector2 delta = (Vector2)Input.mousePosition - _touchStart;
                _isSwiping = false;
                ProcessSwipe(delta);
            }
#endif

            if (Input.touchCount == 1)
            {
                Touch t = Input.GetTouch(0);
                if (t.phase == TouchPhase.Began)
                {
                    _touchStart = t.position;
                    _isSwiping = true;
                }
                else if (t.phase == TouchPhase.Ended && _isSwiping)
                {
                    Vector2 delta = t.position - _touchStart;
                    _isSwiping = false;
                    ProcessSwipe(delta);
                }
            }
        }

        private void ProcessSwipe(Vector2 delta)
        {
            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y) && Mathf.Abs(delta.x) > SWIPE_THRESHOLD)
            {
                if (delta.x > 0) 
                    ChangeLane(1);
                else 
                    ChangeLane(-1);
            }
            else if (Mathf.Abs(delta.y) > SWIPE_THRESHOLD)
            {
                if (delta.y > 0) 
                    TryJump();
            }
        }

        private void MovePlayer()
        {
            if (_controller.isGrounded)
            {
                if (_verticalVelocity < 0) _verticalVelocity = -1f; 
            }
            else
            {
                _verticalVelocity -= gravity * Time.deltaTime;
            }

            float targetX = (_currentLane - 1) * laneDistance;
            float newX = Mathf.Lerp(transform.position.x, targetX, Time.deltaTime * laneChangeSpeed);

            _moveDirection.x = (newX - transform.position.x) / Time.deltaTime;
            _moveDirection.y = _verticalVelocity;
            _moveDirection.z = forwardSpeed;

            _controller.Move(_moveDirection * Time.deltaTime);
        }

        private void ChangeLane(int dir)
        {
            _currentLane = Mathf.Clamp(_currentLane + dir, 0, 2);
        }

        private void TryJump()
        {
            if (_controller.isGrounded)
            {
                _verticalVelocity = jumpForce;
                
                if (animator != null)
                    animator.SetTrigger("Jump");
            }
        }

        private void UpdateAnimator()
        {
            if (animator == null)
                return;

            bool running = forwardSpeed > 0f && !_isDead;
            animator.SetBool("Running", running);
        }
        
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (_isDead) 
                return;

            if (hit.gameObject.CompareTag("Obstacle"))
            {
                Die();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(_isDead) 
                return;

            if (other.CompareTag("Coin") && other.TryGetComponent(out CoinModel coin))
            {
                coin.CollectedAnimation();
                coin.AddCoin(); 
            }
        }

        private void Die()
        {
            _isDead = true;

            forwardSpeed = 0f;

            _controller.enabled = false;
            
            if (animator != null)
            {
                animator.SetBool("Running", false);
                animator.SetBool("Death", true);
            }
        }
    }
}

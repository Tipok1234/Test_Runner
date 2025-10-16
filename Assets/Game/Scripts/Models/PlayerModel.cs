using System.Threading.Tasks;
using Managers;
using Screens;
using UnityEngine;

namespace Models
{
    public class PlayerModel : MonoBehaviour
    {
        [SerializeField] private CharacterController controller;
        [SerializeField] private Animator animator;
        
        [SerializeField] private Vector3 startPosition;
        
        [Header("Move Settings")] [SerializeField]
        private float forwardSpeed;

        [SerializeField] private float laneDistance;
        [SerializeField] private float laneChangeSpeed;

        [Header("Jump / Gravity")] 
        [SerializeField] private float jumpForce;
        [SerializeField] private float gravity;
        
        private Vector3 _moveDirection;
        private Vector2 _touchStart;

        public int CollectedCoin { get; private set; }
        
        private int _currentLane = 1;
        
        private float _verticalVelocity;
      
        private bool isSwiping;
        private bool isWin = false;
        private bool isDead = false;
        
        private const float SWIPE_THRESHOLD = 50f;

        private void Start()
        {
            if (animator == null)
                Debug.LogWarning("Animator is not assigned on PlayerController.");

            if (animator != null)
            {
                animator.SetBool("Running", true);
                animator.SetBool("Death", false);
            }
        }

        public void ResetState()
        {
            isWin = false;
            CollectedCoin = 0;
            controller.enabled = false;

            _currentLane = 1;
            transform.rotation = Quaternion.identity;
            transform.position = startPosition;

            animator.SetBool("Running", false);
            animator.SetBool("Death", false);

            if (isDead)
            {
                animator.ResetTrigger("Idle");
                animator.SetTrigger("Idle");
            }

            isDead = false;

            controller.enabled = true;
            animator.SetBool("Running", true);
        }

        public void WinState()
        {
            isWin = true;
            animator.SetTrigger("Idle");
            animator.SetBool("Running", false);
            animator.ResetTrigger("Idle");
            transform.rotation = Quaternion.identity;
        }

        private void Update()
        {
            if (isDead || isWin)
                return;
            
            HandleInput();
            MovePlayer();
            UpdateAnimator();
        }
        
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (isDead) 
                return;

            if (hit.gameObject.CompareTag("Obstacle"))
            {
                Die();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(isDead) 
                return;

            if (other.CompareTag("Coin") && other.TryGetComponent(out CoinModel coin))
            {
                CollectedCoin++;
                coin.CollectedAnimation();
                coin.AddCoin(); 
            }
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
                isSwiping = true;
            }
            else if (Input.GetMouseButtonUp(0) && isSwiping)
            {
                Vector2 delta = (Vector2)Input.mousePosition - _touchStart;
                isSwiping = false;
                ProcessSwipe(delta);
            }
#endif

            if (Input.touchCount == 1)
            {
                Touch t = Input.GetTouch(0);
                if (t.phase == TouchPhase.Began)
                {
                    _touchStart = t.position;
                    isSwiping = true;
                }
                else if (t.phase == TouchPhase.Ended && isSwiping)
                {
                    Vector2 delta = t.position - _touchStart;
                    isSwiping = false;
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
            if (controller.isGrounded)
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

            controller.Move(_moveDirection * Time.deltaTime);
        }

        private void ChangeLane(int dir)
        {
            AudioManager.Instance.SwipeSound();
            _currentLane = Mathf.Clamp(_currentLane + dir, 0, 2);
        }

        private void TryJump()
        {
            if (controller.isGrounded)
            {
                _verticalVelocity = jumpForce;
            }
        }

        private void UpdateAnimator()
        {
            if (animator == null)
                return;

            bool running = forwardSpeed > 0f && !isDead;
            animator.SetBool("Running", running);
        }

        private void Die()
        {
            isDead = true;

            controller.enabled = false;
            
            if (animator != null)
            {
                //animator.SetBool("Running", false);
                animator.SetBool("Death", true);
                OpenLoseScreen();
            }
        }

        private async void OpenLoseScreen()
        {
            await Task.Delay(1000);
            
            UIManager.Instance.GetScreen<GameScreen>().Timer.StopTimer();
            UIManager.Instance.GetScreen<LoseScreen>().Setup(CollectedCoin);
        }
    }
}

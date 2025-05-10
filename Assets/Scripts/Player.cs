using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace Assets.Scripts 
{
    public class Player : Character
    {

        public InputActionAsset inputActions;

        public InputAction moveAction;
        public InputAction rollAction;
        public InputAction atackAction;
        public InputAction spawnAction;

        public float speed = 5;
        public float rollSpeed = 10;
        public float rotationSpeed = 700;
        public float rollCooldown = 1;

        public Transform cameraTransform;

        private CharacterController _controller;
        private Vector3 _rollDirection;
        private bool _isRolling = false;
        private float _lastRollTime;
        private Animator _animator;

        private Vector2 _moveInput;
        private Vector3 _damageMoveVelocity;
        private float _damageMoveVelocityDeceleration = 5f;

        private static readonly int SpeedAnimationsHash = Animator.StringToHash("Speed");
        private static readonly int MoveAnimationsHash = Animator.StringToHash("IsMoving");

        [SerializeField] SkeletonSpawner spawner;


        private void Awake()
        {
            moveAction = inputActions.FindAction("Move");
            rollAction = inputActions.FindAction("Roll");
            atackAction = inputActions.FindAction("Attack");
            spawnAction = inputActions.FindAction("Spawn");
        }

        void Start()
        {
            _controller = GetComponent<CharacterController>();
            Health = 100;
            Speed = speed;
            AttackDamage = 10f; // Attack Damage& Range will be in weapon
            AttackRange = 1.5f;
            AttackCooldown = 1f;
        }

        private void OnEnable()
        {
            inputActions.FindActionMap("Player").Enable();
        }

        private void OnDisable()
        {
            inputActions.FindActionMap("Player").Disable();
        }

        void Update()
        {

            if (Time.time - _lastRollTime > rollCooldown && !_isRolling && rollAction.triggered)
            {
                StartRoll();
            }

            if (atackAction.triggered)
            {
                Attack();
            }

            if (spawnAction.triggered)
            {
                spawner.SpawnSkeleton();
            }

            HandleMovement();
        }

        void HandleMovement()
        {
            _moveInput = moveAction.ReadValue<Vector2>();
            var inputMagnitude = _moveInput.magnitude;

            var moveForce = transform.forward * (inputMagnitude * speed * Time.deltaTime);
            var nextPosition = transform.position + moveForce;

            nextPosition += _damageMoveVelocity * Time.deltaTime;
            _damageMoveVelocity = Vector3.Lerp(_damageMoveVelocity, Vector3.zero, _damageMoveVelocityDeceleration * Time.deltaTime);

            if (NavMesh.SamplePosition(nextPosition, out var navHit, 2f, NavMesh.AllAreas))
            {
                nextPosition = navHit.position;
            }

            transform.position = nextPosition;

            if (inputMagnitude > 0.1f)
            {
                var forwardDirection = transform.position - cameraTransform.position;
                forwardDirection.y = 0;

                var rightDirection = Vector3.Cross(forwardDirection, Vector3.down);
                var targetDirection = forwardDirection * _moveInput.y + rightDirection * _moveInput.x;
                targetDirection.Normalize();

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection),
                    rotationSpeed * Time.deltaTime);
            }

            /*_animator.SetFloat(SpeedAnimationsHash, Mathf.Clamp(inputMagnitude, 0.1f, 1));
            _animator.SetBool(MoveAnimationsHash, inputMagnitude > 0.1f);*/
        }

        void StartRoll()
        {
            _isRolling = true;
            _lastRollTime = Time.time;
            _rollDirection = transform.forward;
            Invoke("EndRoll", 0.5f);
        }

        void EndRoll()
        {
            _isRolling = false;
        }

        public override void Attack()
        {
            if (canAttack)
            {
                StartCoroutine(AttackCoroutine());
            }
        }

        private IEnumerator AttackCoroutine()
        {
            canAttack = false;
            Debug.Log("Player attacks");
            yield return new WaitForSeconds(AttackCooldown);
            canAttack = true;
        }
        protected override void Die()
        {
            Debug.Log("Player has died");
        }
    }
}

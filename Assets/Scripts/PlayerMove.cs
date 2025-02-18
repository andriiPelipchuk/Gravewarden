using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5.0f;
    public float rollSpeed = 10.0f;
    public float rotationSpeed = 700f;
    public float rollCooldown = 1f;

    private CharacterController controller;
    private Vector3 rollDirection;
    private bool isRolling = false;
    private float lastRollTime;
    public Transform cameraTransform;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Time.time - lastRollTime > rollCooldown && !isRolling && Input.GetButtonDown("Jump"))
        {
            StartRoll();
        }

        HandleMovement();
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            horizontal = touch.position.x > Screen.width / 2 ? 1 : -1;
            vertical = touch.position.y > Screen.height / 2 ? 1 : -1;
        }

        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        if (direction.magnitude >= 0.1f && !isRolling)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, 0.1f);

            transform.rotation = Quaternion.Euler(0, angle, 0);

            Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        if (isRolling)
        {
            controller.Move(rollDirection * rollSpeed * Time.deltaTime);
        }
    }

    void StartRoll()
    {
        isRolling = true;
        lastRollTime = Time.time;
        rollDirection = transform.forward;
        Invoke("EndRoll", 0.5f);
    }

    void EndRoll()
    {
        isRolling = false;
    }
    
}
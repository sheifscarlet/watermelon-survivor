using System.Collections;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }

    [SerializeField] private float maxSpeed = 10f; // Maximum speed to limit movement
    public float MaxSpeed
    {
        get { return maxSpeed; }
        set { maxSpeed = value; }
    }

    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float drag = 0.5f;    //drag to simulate friction

    private Rigidbody rb;
    public bool isGrounded;

    [Header("Components")] 
    [SerializeField] private Camera playerCamera;

    // Debug
    [SerializeField] private bool showSpeed;

    // Boost variables
    private Coroutine boostCoroutine;
    private float boostDurationRemaining = 0f;
    private bool isBoostActive = false;
    private float originalMoveSpeed;
    private float originalMaxSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = drag;

        
        originalMoveSpeed = moveSpeed;
        originalMaxSpeed = maxSpeed;
    }

    void Update()
    {
        Move();

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        LimitSpeed();
        if (showSpeed)
        {
            Debug.Log(rb.velocity.magnitude);
        }
    }

    void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal"); 
        float moveVertical = Input.GetAxis("Vertical"); 

        // Get camera direction
        Vector3 forward = playerCamera.transform.forward; 
        Vector3 right = playerCamera.transform.right; 

        
        forward.y = 0; 
        right.y = 0;
        
        forward.Normalize();
        right.Normalize();
        
        Vector3 movement = (right * moveHorizontal + forward * moveVertical).normalized;
        rb.AddForce(movement * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);
    }

    public void Jump()
    {
        AudioController.instance.PlaySound("Jump");
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    void LimitSpeed()
    {
        // If the velocity magnitude exceeds the max speed, scale it back
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    
    public void ActivateBoost(float duration, float newMoveSpeed, float newMaxSpeed)
    {
        boostDurationRemaining += duration;
        
        moveSpeed = newMoveSpeed;
        maxSpeed = newMaxSpeed;

        if (boostCoroutine == null)
        {
            boostCoroutine = StartCoroutine(BoostCoroutine());
        }
    }

    private IEnumerator BoostCoroutine()
    {
        if (!isBoostActive)
        {
            isBoostActive = true;
        }

        while (boostDurationRemaining > 0)
        {
            boostDurationRemaining -= Time.deltaTime;
            yield return null;
        }

        // Reset speeds to original values
        moveSpeed = originalMoveSpeed;
        maxSpeed = originalMaxSpeed;
        isBoostActive = false;
        NotificationsController.instance.DeactivateBoostNotification();
        

        boostCoroutine = null;
    }
}

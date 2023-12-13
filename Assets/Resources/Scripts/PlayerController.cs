using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    
    [Header("References")] [Space]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform orientation;
    public GameObject weaponCamera;
    private Rigidbody rb;

    [Header("Player Variables")] [Space]
    public int _playerHealth;
    public int playerHealth
    {
        get
        {
            return _playerHealth;
        }
        set
        {
            _playerHealth = value;
            _playerHealth = Mathf.Clamp(_playerHealth, 0, 100);
        }
    }
    
    [SerializeField] private float walkSpeed;
    private float defaultWalkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpPower;
    public bool isRunning;

    private float horizontalInput;
    private float verticalInput;
    private Vector3 direction;
    private float characterHeight;
    private float characterWidth;
    
    [Header("Interaction Variables")] [Space]
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask interactableLayer;
    
    private void Awake()
    {
        instance = this;

        rb = GetComponent<Rigidbody>();
        characterHeight = GetComponent<BoxCollider>().size.y;
        characterWidth = GetComponent<BoxCollider>().size.x;
    }

    void Start()
    {
        defaultWalkSpeed = walkSpeed;
        UIManager.instance.UpdateHealth();
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");   
        verticalInput = Input.GetAxisRaw("Vertical");

        direction = (orientation.forward * verticalInput) + (orientation.right * horizontalInput);
        
        UIManager.instance.UpdateInteractMessage(string.Empty);
        
        if (isGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpPower, rb.velocity.z);

            rb.drag = 0f;
        }

        if (isGrounded())
            rb.drag = 10f;
        else
            rb.drag = 0f;

        LimitSpeed();
        
        CheckInteractables();
    }
    
    private void FixedUpdate()
    {
        if (isGrounded())
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                isRunning = true;
                walkSpeed = runSpeed;
            }
            else
            {
                isRunning = false;
                walkSpeed = defaultWalkSpeed;
            }
            
            rb.AddForce(direction.normalized * walkSpeed, ForceMode.Force);
        }
        else
        {
            rb.AddForce(direction.normalized * (walkSpeed * 0.1f), ForceMode.Force);
            isRunning = false;
        }
    }

    private void LimitSpeed()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (flatVel.magnitude > walkSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * walkSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void CheckInteractables()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * rayDistance);
        
        if (Physics.Raycast(ray, out RaycastHit hitInfo, rayDistance, interactableLayer))
        {
            if (hitInfo.collider.GetComponent<Interactable>() != null)
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                
                UIManager.instance.UpdateInteractMessage(interactable.promptMessage);
                
                if (Input.GetKeyDown(KeyCode.E))
                    interactable.BaseInteract();
            }
        }
    }
    
    public bool isGrounded()
    {
        return Physics.SphereCast(transform.position, characterWidth / 4, Vector3.down, out RaycastHit rayHit,
            characterHeight / 2);
    }
}
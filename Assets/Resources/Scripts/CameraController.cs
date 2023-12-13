using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance { get; private set; }
    
    [SerializeField] private float sensivityX;
    [SerializeField] private float sensivityY;
    
    [SerializeField] private Transform orientation;

    private float xRotation;
    private float yRotation;

    private void Awake() => instance = this;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        yRotation = 68.32f;
    }

    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * sensivityX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.fixedDeltaTime * sensivityY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    }
    
    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        orientation.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}

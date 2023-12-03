using UnityEngine;

public class SwayController : MonoBehaviour
{
    [Header("Sway")] [Space]
    [SerializeField] private float smoothSway;
    [SerializeField] private float swayMultiplier;

    private void Update()
    {
        float multiplier = swayMultiplier * ((1 / Time.deltaTime) / 60); 
        
        float mouseX = Input.GetAxisRaw("Mouse X") * multiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * multiplier;

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;
        
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smoothSway * Time.deltaTime);
    }
}
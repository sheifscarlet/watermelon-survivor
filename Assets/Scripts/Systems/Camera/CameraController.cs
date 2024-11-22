using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform player; 
    [SerializeField] float mouseSensitivity = 100f;
    [SerializeField] float maxVerticalAngle = 80f; 
    [SerializeField] float minVerticalAngle = -30f; 
    
    [SerializeField] bool invertHorizontal = false; 
    [SerializeField] bool invertVertical = false; 

    private float rotationX = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; 
    }

    private void Update()
    {
        //cam pos
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        
        if (invertHorizontal)
        {
            mouseX = -mouseX;
        }
        if (invertVertical)
        {
            mouseY = -mouseY;
        }
        
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle); 

        
        transform.localRotation = Quaternion.Euler(rotationX, transform.localEulerAngles.y + mouseX, 0);
        
        transform.position = player.position; 
    }
}

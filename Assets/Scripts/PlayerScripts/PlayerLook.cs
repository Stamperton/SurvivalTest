using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Transform playerBody;

    //Controller Options
    public bool invertMouse = false;
    public float mouseSensitivity = 100;

    //Private Variables
    float xRotation = 0f;


    // Start is called before the first frame update
    void Start()
    {
        MouseHandling.MouseToFPSMode();
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //Y Rotation & Mouse Invert
        if (invertMouse)
        {
            xRotation += mouseY;
        }
        else
            xRotation -= mouseY;


        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}

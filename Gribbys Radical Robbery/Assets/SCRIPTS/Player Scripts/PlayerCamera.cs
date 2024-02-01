using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float sensX = 2;
    public float sensY = 2;

    public Transform orientation;

    float xRotation;
    float yRotation;

    private void Start()
    {
        // make cursor invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX * 1000;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY * 1000;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // rotation cam and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}

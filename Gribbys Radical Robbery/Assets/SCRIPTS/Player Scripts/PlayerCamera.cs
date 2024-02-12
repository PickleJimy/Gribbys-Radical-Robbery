using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Range(1,60)]
    public int sens = 30;

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
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sens * 100;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sens * 100;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // rotation cam and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}

using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float sens = 15;

    public Transform orientation;

    float xRotation;
    float yRotation;

    float time;

    private void Start()
    {
        // make cursor invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (PlayerPrefs.GetFloat("camSensitivity") == 0)
        {
            PlayerPrefs.SetFloat("camSensitivity", 15);
        }

        sens = PlayerPrefs.GetFloat("camSensitivity");
    }

    private void FixedUpdate()
    {
        // get mouse input
        time = Time.deltaTime;
    }

    private void Update()
    {
        // get mouse input
        float mouseX = Input.GetAxis("Mouse X") * time * sens * 10;
        float mouseY = Input.GetAxis("Mouse Y") * time * sens * 10;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // rotation cam and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}

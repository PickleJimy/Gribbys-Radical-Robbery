using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LorePage : MonoBehaviour
{

    public GameObject player;
    public GameObject playerCam;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerCam = GameObject.Find("PlayerCam");

        PauseGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PauseGame()
    {
        if (!PauseController.gameIsPaused)
        {
            PauseController.gameIsPaused = true;
            player.GetComponent<PlayerMovement>().enabled = false;
            player.GetComponent<GrabAndStab>().enabled = false;
            player.GetComponent<PlayerStats>().enabled = false;
            playerCam.GetComponent<PlayerCamera>().enabled = false;
            PauseController.stealability = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
        }
        else
        {
            UnpauseGame();
        }
    }

    public void UnpauseGame()
    {
        Destroy(gameObject);
        PauseController.gameIsPaused = false;
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<GrabAndStab>().enabled = true;
        player.GetComponent<PlayerStats>().enabled = true;
        playerCam.GetComponent<PlayerCamera>().enabled = true;
        PauseController.stealability = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
    }
}

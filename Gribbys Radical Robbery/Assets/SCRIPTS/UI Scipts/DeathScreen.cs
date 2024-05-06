using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    private GameObject stealablesParent;
    private GameObject stealablesChild;

    public GameObject playerCamera;
    public GameObject deathMenu;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayerDead()
    {
        PauseGame();
    }

    void PauseGame()
    {
        if (!PauseController.gameIsPaused)
        {
            Instantiate(deathMenu, new Vector3(0, 0, 0), Quaternion.identity);
            PauseController.gameIsPaused = true;
            gameObject.GetComponent<PlayerMovement>().enabled = false;
            gameObject.GetComponent<GrabAndStab>().enabled = false;
            gameObject.GetComponent<PlayerStats>().enabled = false;
            playerCamera.GetComponent<PlayerCamera>().enabled = false;
            PauseController.stealability = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            ChangeStealables();
            Time.timeScale = 0;
        }
        else
        {
            UnpauseGame();
        }
    }

    public void UnpauseGame()
    {
        Destroy(deathMenu);
        PauseController.gameIsPaused = false;
        gameObject.GetComponent<PlayerMovement>().enabled = true;
        gameObject.GetComponent<GrabAndStab>().enabled = true;
        gameObject.GetComponent<PlayerStats>().enabled = true;
        playerCamera.GetComponent<PlayerCamera>().enabled = true;
        PauseController.stealability = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ChangeStealables();
        Time.timeScale = 1;
    }

    public void ChangeStealables()
    {
        if (stealablesParent != null)
        {
            int i = 0;

            GameObject[] allStealables = new GameObject[stealablesParent.transform.childCount];

            foreach (Transform child in stealablesParent.transform)
            {
                allStealables[i] = child.gameObject;
                i += 1;
            }

            foreach (GameObject child in allStealables)
            {
                child.gameObject.GetComponent<Collider>().enabled = PauseController.stealability;
            }
        }
    }
}
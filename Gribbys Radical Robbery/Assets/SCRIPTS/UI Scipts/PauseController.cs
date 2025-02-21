using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    public GameObject pauseMenu;
    private GameObject stealablesParent;
    private GameObject stealablesChild;
    public GameObject playerCamera;

    public static bool gameIsPaused = false;
    public static bool stealability = false;

    public GameObject puaseMain;
    public GameObject options;

    [Header("Input")]
    public KeyCode pauseKey = KeyCode.P;

    void Start()
    {
        stealablesParent = GameObject.Find("Stealables");
        UnpauseGame();
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            PauseGame();
        }
    }

    public void Click(Button button)
    {
        if (button.name == "Resume Button")
        {
            UnpauseGame();
        }

        if (button.name == "Restart Button")
        {
            RestartScene();
            UnpauseGame();
        }

        if (button.name == "Options Button")
        {
            puaseMain.SetActive(false);
            options.SetActive(true);
        }

        if (button.name == "Main Menu Button")
        {
            PlayerPrefs.SetString("currentLevel", SceneManager.GetActiveScene().name);
            UnpauseGame();
            PlayerStats.stolenGoods = GameManager.stolenGoods;
            PlayerStats.health = 100;
            SceneManager.LoadScene("Main Menu");
        }

        if (button.name == "Quit Button")
        {
            Application.Quit();
        }
    }

    void PauseGame()
    {
        if (!gameIsPaused)
        {
            pauseMenu.SetActive(true);
            gameIsPaused = true;
            gameObject.GetComponent<PlayerMovement>().enabled = false;
            gameObject.GetComponent<GrabAndStab>().enabled = false;
            playerCamera.GetComponent<PlayerCamera>().enabled = false;
            stealability = false;
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

    void UnpauseGame()
    {
        pauseMenu.SetActive(false);
        gameIsPaused = false;
        gameObject.GetComponent<PlayerMovement>().enabled = true;
        gameObject.GetComponent<GrabAndStab>().enabled = true;
        playerCamera.GetComponent<PlayerCamera>().enabled = true;
        stealability = true;
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
                child.gameObject.GetComponent<Collider>().enabled = stealability;
            }
        }
    }

    void RestartScene()
    {
        PlayerStats.stolenGoods = GameManager.stolenGoods;
        PlayerStats.health = 100;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}

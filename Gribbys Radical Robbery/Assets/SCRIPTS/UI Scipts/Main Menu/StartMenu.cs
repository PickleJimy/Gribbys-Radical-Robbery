using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject levelSelect;
    public Animator startgame;
    public Animator startgamelight;
    public GameObject fireOut;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Click(Button button)
    {
        if (button.name == "Continue Button")
        {
            startgame.SetTrigger("Game Started");
            startgamelight.SetTrigger("Game Started");
            Destroy(fireOut, 1);
            if (PlayerPrefs.GetString("currentLevel") != null)
            {
                StartCoroutine(LoadLevel(2.3f, PlayerPrefs.GetString("currentLevel")));
            }
        }

        if (button.name == "New Game Button")
        {
            startgame.SetTrigger("Game Started");
            startgamelight.SetTrigger("Game Started");
            Destroy(fireOut, 1);
            GameManager.stolenGoods = 0;
            PlayerStats.stolenGoods = 0;
            StartCoroutine(LoadLevel(2.3f, "Parking Lot"));
        }

        if (button.name == "Levels Button")
        {
            gameObject.SetActive(false);
            levelSelect.SetActive(true);
        }

        if (button.name == "Back Button")
        {
            gameObject.SetActive(false);
            mainMenu.SetActive(true);
        }
    }


    public IEnumerator LoadLevel(float time, string level)
    {
        yield return new WaitForSeconds(time);

        SceneManager.LoadScene(level);

    }
}

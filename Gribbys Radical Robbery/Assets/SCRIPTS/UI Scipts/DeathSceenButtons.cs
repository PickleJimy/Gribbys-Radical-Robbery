using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathSceenButtons : MonoBehaviour
{
    public void Click(Button button)
    {
        Debug.Log("GOG");

        if (button.name == "Retry Button")
        {
            RestartScene();
        }

        if (button.name == "Main Menu Button")
        {
            PlayerPrefs.SetString("currentLevel", SceneManager.GetActiveScene().name);
            SceneManager.LoadScene("Main Menu");
        }
    }

    void RestartScene()
    {
        PlayerStats.stolenGoods = GameManager.stolenGoods;
        PlayerStats.health = 100;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public GameObject startMenu;

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
        if (button.name == "Back Button")
        {
            gameObject.SetActive(false);
            startMenu.SetActive(true);
        }

        if (button.name == "Parking Lot")
        {
            SceneManager.LoadScene("Parking Lot");
        }
        
        if (button.name == "Player Test Scene")
        {
            SceneManager.LoadScene("PlayerControlCodin");
        }

        if (button.name == "Enemy Test Scene")
        {
            SceneManager.LoadScene("Enemy Test");
        }
    }
}

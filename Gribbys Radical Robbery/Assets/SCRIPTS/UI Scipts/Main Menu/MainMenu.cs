using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject startMenu;
    public GameObject optionsMenu;


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
        if (button.name == "Start Button")
        {
            gameObject.SetActive(false);
            startMenu.SetActive(true);
        }

        if (button.name == "Options Button")
        {
            gameObject.SetActive(false);
            optionsMenu.SetActive(true);
        }

        if (button.name == "Quit Button")
        {
            Application.Quit();
        }
    }
}

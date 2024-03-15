using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject levelSelect;

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
}

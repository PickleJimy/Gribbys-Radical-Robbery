using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [Header("Sensitivity")]
    public Slider sensSlider;
    public TextMeshProUGUI sensNum;

    public GameObject menuMain;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetFloat("camSensitivity") == 0)
        {
            PlayerPrefs.SetFloat("camSensitivity", 15);
        }

        sensSlider.value = PlayerPrefs.GetFloat("camSensitivity");
    }

    // Update is called once per frame
    void Update()
    {
        ChangeSens();
    }

    void ChangeSens()
    {
        sensNum.text = sensSlider.value.ToString();
        PlayerPrefs.SetFloat("camSensitivity", sensSlider.value);
    }

    public void Click(Button button)
    {
        if (button.name == "Back Button")
        {
            menuMain.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}

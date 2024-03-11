using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [Header("Sensitivity")]
    public Slider sensSlider;
    public TextMeshProUGUI sensNum;

    public GameObject playerCam;
    public GameObject pauseMain;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ChangeSens();
    }

    void ChangeSens()
    {
        sensNum.text = sensSlider.value.ToString();
        playerCam.GetComponent<PlayerCamera>().sens = sensSlider.value;
    }

    public void Click(Button button)
    {
        if (button.name == "Back Button")
        {
            pauseMain.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}

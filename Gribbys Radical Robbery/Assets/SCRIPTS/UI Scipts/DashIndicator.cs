using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DashIndicator : MonoBehaviour
{
    public GameObject gribbyFace;

    public Image gribbyImage;

    private bool canDash;

    public float maximum;

    private float current;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        canDash = PlayerMovement.readyToDash;

        if (!canDash)
        {
            gribbyFace.SetActive(true);

            current -= 0.1f;

            SetFill();    
        }


        if (canDash)
        {
            gribbyFace.SetActive(false);

            current = maximum;

            SetFill();
        }

    }

    void SetFill()
    {
        float fillAmount = (float)current / (float)maximum;
        gribbyImage.fillAmount = fillAmount;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DashIndicator : MonoBehaviour
{
    public GameObject gribbyFace;

    private bool canDash;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        canDash = PlayerMovement.readyToDash;

        if (!canDash)
            gribbyFace.SetActive(true);

        if (canDash)
            gribbyFace.SetActive(false);

    }
}

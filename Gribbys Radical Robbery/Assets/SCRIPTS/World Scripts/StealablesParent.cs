using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealablesParent : MonoBehaviour
{
    public GameObject grabText;

    private void Start()
    {
        grabText = GameObject.Find("Grab Text");

        ChangeStealables();
    }

    public void ChangeStealables()
    {
        if (gameObject != null)
        {
            int i = 0;

            GameObject[] allStealables = new GameObject[gameObject.transform.childCount];

            foreach (Transform child in gameObject.transform)
            {
                allStealables[i] = child.gameObject;
                i += 1;
            }

            foreach (GameObject child in allStealables)
            {
                if (child.gameObject.GetComponent<StealableItem>() != null)
                {
                    child.gameObject.GetComponent<StealableItem>().grabText = grabText;
                }
                if (child.gameObject.GetComponent<StealableHealthItem>() != null)
                {
                    child.gameObject.GetComponent<StealableHealthItem>().grabText = grabText;
                }
                if (child.gameObject.GetComponent<StealablePage>() != null)
                {
                    child.gameObject.GetComponent<StealablePage>().grabText = grabText;
                }
            }
        }
    }

}

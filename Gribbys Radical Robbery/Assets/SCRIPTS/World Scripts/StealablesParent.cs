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
                child.gameObject.GetComponent<StealableItem>().grabText = grabText;
            }
        }
    }

}

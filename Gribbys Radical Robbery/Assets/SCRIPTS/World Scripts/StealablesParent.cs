using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealablesParent : MonoBehaviour
{
    public GameObject grabTextRef;
    public static GameObject grabText;

    private void Start()
    {
        grabText = grabTextRef;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyItem : MonoBehaviour
{
    private bool mouseOver;

    // Start is called before the first frame update
    void Start()
    {
        mouseOver = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerWeapon") && mouseOver && other.GetComponent<PlayerDealDamage>().dealDamage)
        {
            Destroy(gameObject);
        }
    }

    private void OnMouseOver()
    {
        mouseOver = true;
    }

    private void OnMouseExit()
    {
        mouseOver = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealableItem : MonoBehaviour
{
    private GameObject player;
    private bool inStealingRange;
    public KeyCode grabKey;

    private void Start()
    {
        player = GameObject.Find("Player");
        grabKey = player.gameObject.GetComponent<GrabAndStab>().grabKey;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            inStealingRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            inStealingRange = false;
    }

    private void OnMouseOver()
    {
        if (inStealingRange && Input.GetKey(grabKey))
        {
            player.gameObject.GetComponent<PlayerStats>().stolenGoods ++;
            Destroy(gameObject);
        }
    }
}

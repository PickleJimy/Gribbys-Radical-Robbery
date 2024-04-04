using System.Collections;
using System.IO;
using UnityEngine;

public class StealableItem : MonoBehaviour
{
    public GameObject grabText;
    private bool inStealingRange;
    public KeyCode grabKey;
    private bool canGrab;

    private void Start()
    {
        grabKey = GrabAndStab.grabKey;
        canGrab = true;
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
        if (inStealingRange)
        {
            grabText.SetActive(true);
            PlayerStats.stealableNameText = gameObject.name;
        }

        if (!inStealingRange)
        {
            grabText.SetActive(false);
        }

        if (inStealingRange && Input.GetKey(grabKey) && canGrab)
        {
            StartCoroutine(DelaySteal(0.4f));
        }
    }

    public IEnumerator DelaySteal(float time)
    {
        canGrab = false;

        yield return new WaitForSeconds(time);

        PlayerStats.IncreaseStolenGoods();
        grabText.SetActive(false);
        Destroy(gameObject);
    }

    private void OnMouseExit()
    {
        grabText.SetActive(false);
    }
}

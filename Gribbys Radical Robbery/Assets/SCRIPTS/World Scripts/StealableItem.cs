using System.Collections;
using System.IO;
using UnityEngine;

public class StealableItem : MonoBehaviour
{
    public GameObject grabText;
    private bool inStealingRange;
    public KeyCode grabKey;

    private void Start()
    {
        grabText = StealablesParent.grabText;
        grabKey = GrabAndStab.grabKey;
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

        if (inStealingRange && Input.GetKey(grabKey))
        {
            StartCoroutine(DelaySteal(0.4f));
        }
    }

    public IEnumerator DelaySteal(float time)
    {
        yield return new WaitForSeconds(time);

        grabText.SetActive(false);
        Destroy(gameObject);
        PlayerStats.stolenGoods++;
    }

    private void OnMouseExit()
    {
        grabText.SetActive(false);
    }
}

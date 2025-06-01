using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class StealablePage : MonoBehaviour
{
    public GameObject grabText;
    private bool inStealingRange;
    public KeyCode grabKey;
    private bool canGrab;

    public GameObject pageMenu;

    [TextArea(5,30)]
    public string wordsOnPage;

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

        // PlayerStats.IncreaseStolenGoods();
        grabText.SetActive(false);
        Destroy(gameObject);

        Instantiate(pageMenu, new Vector3(0, 0, 0), Quaternion.identity);

    }

    private void OnMouseExit()
    {
        grabText.SetActive(false);
    }
}

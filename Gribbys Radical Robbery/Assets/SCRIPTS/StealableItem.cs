using UnityEngine;

public class StealableItem : MonoBehaviour
{
    private GameObject player;
    private GameObject playerUI;
    private bool inStealingRange;
    public KeyCode grabKey;

    private void Start()
    {
        player = GameObject.Find("Player");
        grabKey = player.gameObject.GetComponent<GrabAndStab>().grabKey;
        playerUI = GameObject.Find("Grab Text");
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
            playerUI.SetActive(true);
        }

        if (!inStealingRange)
        {
            playerUI.SetActive(false);
        }

        if (inStealingRange && Input.GetKey(grabKey))
        {
            playerUI.SetActive(false);
            Destroy(gameObject);
            player.gameObject.GetComponent<PlayerStats>().stolenGoods++;
        }
    }

    private void OnMouseExit()
    {
        playerUI.SetActive(false);
    }
}

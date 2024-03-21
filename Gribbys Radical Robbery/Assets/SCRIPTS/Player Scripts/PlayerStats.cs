using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerStats : MonoBehaviour
{
    public static int health = Mathf.Clamp(100, 0, 100);

    public int damageDealt;

    public static int stolenGoods;

    public TextMeshProUGUI stolenGoodsCounter;
    private GameObject playerUI;


    void Start()
    {
        Invoke("HideGrab", 0.1f);
    }

    void HideGrab()
    {
        playerUI = GameObject.Find("Grab Text");
        playerUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSGC();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyDamager") && other.GetComponent<EnemyDealDamage>().dealDamage)
        {
            Debug.Log("YEEOWCH");
            damageDealt = other.GetComponent<EnemyDealDamage>().damage;
            health -= damageDealt;
        }
    }

    public void UpdateSGC()
    {
        stolenGoodsCounter.text = "Goods Stolen: " + stolenGoods;
    }
}

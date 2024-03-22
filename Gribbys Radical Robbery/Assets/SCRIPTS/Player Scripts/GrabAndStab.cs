using System.Collections;
using UnityEngine;

public class GrabAndStab : MonoBehaviour
{
    [Header("Keybinds")]
    public static KeyCode grabKey = KeyCode.Mouse1;
    public static KeyCode stabKey = KeyCode.Mouse0;

    [Header("Arms Reference ")]
    public GameObject arms;

    public Animator GribbyAnim;

    [Header("Damage")]
    [Range(0, 100)]
    public int damage;
    public bool dealDamage;
    public float stabCooldown;
    bool readyToStab;

    void Start()
    {
        DelayDealDamage(0f, false, true);
    }

    // Update is called once per frame
    void Update()
    {
        Grab();
        Stab();
    }

    public void Grab()
    {
        if (Input.GetKey(grabKey))
        {
            Debug.Log("GRAB");
            GribbyAnim.SetTrigger("isGrabbing");
        }
    }

    public void Stab()
    {
        if (Input.GetKey(stabKey) && readyToStab)
        {
            Debug.Log("STAB");
            GribbyAnim.SetTrigger("isStabbing");
            DelayDealDamage(0.08f, true, false);
            DelayDealDamage(stabCooldown, false, true);
        }
    }

    public IEnumerator DealDamage(float time, bool dmg, bool stab)
    {
        yield return new WaitForSeconds(time);
        dealDamage = dmg;
        readyToStab = stab;
        
    }

    public void DelayDealDamage(float time, bool dmg, bool stab)
    {    
        StartCoroutine(DealDamage(time, dmg, stab));
    }
}

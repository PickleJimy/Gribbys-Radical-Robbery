using System.Collections;
using UnityEngine;

public class GrabAndStab : MonoBehaviour
{
    [Header("Keybinds")]
    public KeyCode grabKey = KeyCode.Mouse1;
    public KeyCode stabKey = KeyCode.Mouse0;

    [Header("Arms Reference ")]
    public GameObject arms;

    public Animator GribbyAnim;

    [Header("Damage")]
    [Range(0, 100)]
    public int damage;
    public bool dealDamage;
    private float stabCooldown = 0.5f;
    bool readyToStab;

    void Start()
    {
        DelayDealDamage(0f, true);
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
            DelayDealDamage(0.02f, false);
            DelayDealDamage(stabCooldown, true);
        }
    }

    public IEnumerator DealDamage(float f, bool g)
    {
        yield return new WaitForSeconds(f);
        dealDamage = !dealDamage;
        readyToStab = g;
    }

    public void DelayDealDamage(float n, bool m)
    {    
        StartCoroutine(DealDamage(n, m));
    }
}

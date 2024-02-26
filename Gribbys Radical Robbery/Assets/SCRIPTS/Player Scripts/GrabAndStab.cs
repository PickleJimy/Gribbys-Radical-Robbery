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

    void Start()
    {
        DelayDealDamge(0f);
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
        if (Input.GetKey(stabKey))
        {
            Debug.Log("STAB");
            GribbyAnim.SetTrigger("isStabbing");
            DelayDealDamge(0.1f);
            DelayDealDamge(0.5f);
        }
    }

    public IEnumerator DealDamage(float f)
    {
        yield return new WaitForSeconds(f);
        dealDamage = !dealDamage;
    }

    public void DelayDealDamge(float n)
    {    
        StartCoroutine(DealDamage(n));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyItem : MonoBehaviour
{
    private bool mouseOver;
    public bool grounded;
    public bool groundDestructable;
    public float itemHeight;
    public LayerMask whatIsGround;

    // Start is called before the first frame update
    void Start()
    {
        mouseOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, itemHeight, whatIsGround);
        if (grounded && groundDestructable)
        {
            Destroy(gameObject);
        }
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

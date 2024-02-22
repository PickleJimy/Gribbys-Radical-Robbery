using UnityEngine;

public class GrabAndStab : MonoBehaviour
{
    [Header("Keybinds")]
    public KeyCode grabKey = KeyCode.Mouse1;
    public KeyCode stabKey = KeyCode.Mouse0;

    [Header("Arms Reference ")]
    public GameObject arms;

    public Animator GribbyAnim;

    void Start()
    {

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
        }
    }
}

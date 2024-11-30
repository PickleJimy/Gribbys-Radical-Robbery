using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedObject : MonoBehaviour
{
    public GameObject key;
    public AudioSource unlockSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (key == null)
        {
            if (!unlockSound.isPlaying)
            {
                unlockSound.Play();
            }
            StartCoroutine(DelayUnlock(0.8f));
        }
    }

    public IEnumerator DelayUnlock(float time)
    {
        yield return new WaitForSeconds(time);

        Destroy(gameObject);
    }
}

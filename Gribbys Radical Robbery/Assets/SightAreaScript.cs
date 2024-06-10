using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightAreaScript : MonoBehaviour
{
    public MeshCollider areaOfSight;

    // Start is called before the first frame update
    void Start()
    {
        areaOfSight = GetComponent<MeshCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

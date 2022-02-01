using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationSpawn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        BiomeDirector director = GetComponentInParent<BiomeDirector>();
        GameObject templ = director.FindBigDecoToSpawn();
        if (templ)
        {
            Instantiate(templ, transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

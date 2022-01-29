using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class NPCController : MonoBehaviour
{
    Rigidbody m_rigidbody;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_rigidbody.velocity = new Vector3(-GameplayConstants.humanSpeed / 2.0f, 0.0f, 0.0f);
    }
}

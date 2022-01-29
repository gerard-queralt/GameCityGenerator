using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class HumanoidVisual : MonoBehaviour
{
    Animator m_animator;
    Rigidbody m_rigidbody;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_rigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float speedMagnitude = m_rigidbody.velocity.magnitude;
        m_animator.SetFloat("Speed", speedMagnitude / GameplayConstants.humanSpeed);
    }
}

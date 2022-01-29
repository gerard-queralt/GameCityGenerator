using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class MCController : MonoBehaviour
{
    Rigidbody m_rigidbody;
    Vector2 m_rawAxis;
    Vector3 m_velocity;
    Vector3 m_lookAt;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_velocity = Vector3.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_velocity = Vector3.zero;
        m_lookAt = Vector3.forward;
    }

    // Update is called once per frame
    void Update()
    {
        m_rigidbody.velocity = m_velocity;
        m_rigidbody.rotation = Quaternion.LookRotation(m_lookAt);
    }

    void OnMove(InputValue i_input)
    {
        m_rawAxis = i_input.Get<Vector2>();
        m_velocity = new Vector3(m_rawAxis.x * GameplayConstants.humanSpeed, 0.0f, m_rawAxis.y * GameplayConstants.humanSpeed);
        if (m_velocity.magnitude > 0.0f)
        {
            m_lookAt = m_velocity.normalized;
        }
    }
}

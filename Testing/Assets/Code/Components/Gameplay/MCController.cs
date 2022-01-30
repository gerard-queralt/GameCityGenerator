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
    bool m_actionRequested = false;

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

        if (m_actionRequested)
        {
            m_actionRequested = false;
            //Execute action
            Vector3 start = transform.position + new Vector3(0.0f, 0.5f, 0.0f);
            Vector3 direction = m_lookAt.normalized;
            float maxDistance = 1.5f;
            LayerMask mask = LayerMask.GetMask(GameplayConstants.PhysicLayer.Interactible);
            QueryTriggerInteraction queryTrigger = QueryTriggerInteraction.Ignore;
            RaycastHit hit;
            if (Physics.Raycast(start, direction, out hit, maxDistance, mask, queryTrigger))
            {
                Interactible interactible = hit.collider.gameObject.GetComponent<Interactible>();
                interactible.OnInteracted();
            }
        }
    }

    void OnMove(InputValue i_value)
    {
        m_rawAxis = i_value.Get<Vector2>();
        m_velocity = new Vector3(m_rawAxis.x * GameplayConstants.humanSpeed, 0.0f, m_rawAxis.y * GameplayConstants.humanSpeed);
        if (m_velocity.magnitude > 0.0f)
        {
            m_lookAt = m_velocity.normalized;
        }
    }

    void OnAction(InputValue i_value)
    {
        if (i_value.isPressed)
        {
            m_actionRequested = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField] Transform m_target;

    Vector3 m_delta;

    private void Awake()
    {
        Debug.Assert(m_target != null, "Target not set in Follow component of GameObject[" + gameObject.name + "]");
    }

    // Start is called before the first frame update
    void Start()
    {
        m_delta = transform.position - m_target.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = m_target.position + m_delta;
    }
}

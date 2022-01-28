using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularMovement : MonoBehaviour
{
    [SerializeField] float m_radiusMeters = 2;
    [SerializeField] float m_circleSeconds = 10;

    Vector3 m_initialPosition;
    float m_elapsedSeconds = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_initialPosition = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        m_elapsedSeconds += Time.deltaTime;
        gameObject.transform.position = m_initialPosition
                                            + gameObject.transform.up * Mathf.Sin(2 * Mathf.PI * m_elapsedSeconds / m_circleSeconds) * m_radiusMeters
                                            + gameObject.transform.right * Mathf.Cos(2 * Mathf.PI * m_elapsedSeconds / m_circleSeconds) * m_radiusMeters;
    }
}

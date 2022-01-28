using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using UnityEngine.Events;

public class CircularMovement : MonoBehaviour
{
    [SerializeField] float m_radiusMeters = 2;
    [SerializeField] float m_circleSeconds = 10;
    //[SerializeField] Text m_circleCounter;

    public struct CircleState
    {
        public int numCircles;
    }
    [System.Serializable] class NumCirclesChangedEvent: UnityEvent<CircleState> { }
    [SerializeField] NumCirclesChangedEvent sig_circlesChanged;

    World m_world;

    private int m_numCircles = 0;

    Vector3 m_initialPosition;
    float m_elapsedSeconds = 0;

    private void Awake()
    {
        //Debug.Assert(m_circleCounter != null, "CircleCounter not assigned in GameObject[" + gameObject.name + "]");
        m_world = GetComponentInParent<World>();
        Debug.Assert(m_world != null, "World not found in parent");
    }

    // Start is called before the first frame update
    void Start()
    {
        m_numCircles = 0;
        m_initialPosition = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        m_elapsedSeconds += Time.deltaTime;
        gameObject.transform.position = m_initialPosition
                                            + gameObject.transform.up * Mathf.Sin(2 * Mathf.PI * m_elapsedSeconds / m_circleSeconds) * m_radiusMeters
                                            + gameObject.transform.right * Mathf.Cos(2 * Mathf.PI * m_elapsedSeconds / m_circleSeconds) * m_radiusMeters;

        int oldNumCircles = m_numCircles;
        m_numCircles = Mathf.FloorToInt(m_elapsedSeconds / m_circleSeconds);
        //m_circleCounter.text = string.Format("Vueltas: {0}", m_numCircles);
        if (oldNumCircles != m_numCircles)
        {
            CircleState state = new CircleState();
            state.numCircles = m_numCircles;
            sig_circlesChanged.Invoke(state);
            m_world.OnNumCirclesChanged(this, m_numCircles);
        }
    }

    public int GetNumCircles()
    {
        return m_numCircles;
    }
}

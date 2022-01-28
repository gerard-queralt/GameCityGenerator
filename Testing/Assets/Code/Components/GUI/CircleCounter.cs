using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class CircleCounter : MonoBehaviour
{
    Text m_text;
    //[SerializeField] CircularMovement m_circularMovement;
    UIDirector m_director;

    private void Awake()
    {
        //Debug.Assert(m_circularMovement != null, "CircularMovement not set in GameObject[" + gameObject.name + "]");
        m_text = GetComponent<Text>();
        m_director = GetComponentInParent<UIDirector>();
        Debug.Assert(m_director != null, "UIDirector not found in parent");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //m_text.text = string.Format("Vueltas: {0}", m_circularMovement.GetNumCircles());
        m_text.text = string.Format("Vueltas: {0}", m_director.GetNumCircles());
    }

    //public void OnNumCirclesChanged(CircularMovement.CircleState i_circleState)
    //{
    //    m_text.text = string.Format("Vueltas: {0}", i_circleState.numCircles);
    //}
}

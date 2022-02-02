using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddXP : MonoBehaviour
{
    [SerializeField] int m_xpAmount;
    private World m_director;
    private bool m_xpGiven = false;

    private void Awake()
    {
        m_director = GetComponentInParent<World>();
        Debug.Assert(m_director != null, "World not found in parent of GameObject[" + gameObject.name + "]");
    }

    public void OnAction()
    {
        if (!m_xpGiven)
        {
            m_director.RequestAddXP(m_xpAmount);
            m_xpGiven = true;
        }
        else
        {
            Debug.LogError("Trying to add xp while game is paused!");
        }
    }
}

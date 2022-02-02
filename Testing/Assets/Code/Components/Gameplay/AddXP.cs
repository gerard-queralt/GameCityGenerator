using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddXP : MonoBehaviour
{
    private World m_gameWorld;
    [SerializeField] int m_xpAmount;
    private bool m_xpGiven = false;

    private void Awake()
    {
        m_gameWorld = GetComponentInParent<World>();
        Debug.Assert(m_gameWorld != null, "World not found in parent of GameObject[" + gameObject.name + "]");
    }

    public void OnAction()
    {
        if (!m_xpGiven)
        {
            m_gameWorld.RequestAddXP(m_xpAmount);
            m_xpGiven = true;
        }
        else
        {
            Debug.LogError("Trying to add xp while game is paused!");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward : MonoBehaviour
{
    private bool m_rewardGiven = false;

    public void OnAction()
    {
        if (!m_rewardGiven)
        {
            m_rewardGiven = true;
            Debug.Log("Object given to player");
        }
    }
}

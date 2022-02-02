using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward : MonoBehaviour
{
    private World m_gameWorld;
    private bool m_rewardGiven = false;
    [SerializeField] ItemDef m_itemToReward;
    [SerializeField] uint m_itemAmount = 1;

    private void Awake()
    {
        Debug.Assert(m_itemToReward != null, "Item reward not set in GameObject[" + gameObject.name + "]");
        Debug.Assert(m_itemAmount > 0, "Item amount must be greater than zero in GameObject[" + gameObject.name + "]");
        m_gameWorld = GetComponentInParent<World>();
    }

    public void OnAction()
    {
        if (!m_rewardGiven)
        {
            m_gameWorld.RequestAddItem(m_itemToReward, m_itemAmount);
            m_rewardGiven = true;
        }
    }
}

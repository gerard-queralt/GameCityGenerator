using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class World : MonoBehaviour
{
    [System.Serializable] class NumCirclesChangedEvent : UnityEvent<int> { }
    [SerializeField] NumCirclesChangedEvent sig_circlesChanged;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnNumCirclesChanged(CircularMovement i_circularMovement, int i_numCircles)
    {
        sig_circlesChanged.Invoke(i_numCircles);
    }

    public void RequestAddXP(int i_xp_value)
    {
        Game.instance.AddXP(i_xp_value);
    }

    public void RequestAddItem(ItemDef i_item, uint i_itemAmount)
    {
        Debug.Assert(i_item != null, "Trying to add null item");
        Debug.Assert(i_itemAmount > 0, "Trying to add zero or less items");
        Game.instance.AddItem(i_item, i_itemAmount);
    }
}

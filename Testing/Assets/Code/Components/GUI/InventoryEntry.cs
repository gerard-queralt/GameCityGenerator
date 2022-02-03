using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryEntry : MonoBehaviour
{
    [SerializeField] Image m_icon;
    [SerializeField] Text m_amount;

    private ItemDef m_item;

    private void Awake()
    {
        Debug.Assert(m_icon != null, "Icon not set in InventoryEntry");
        Debug.Assert(m_amount != null, "Amount not set in InventoryEntry");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //TMP
        if (m_item)
        {
            m_amount.text = Game.instance.inventory.GetItemAmount(m_item).ToString();
        }
    }

    public void DisplayItem(ItemDef i_item)
    {
        m_item = i_item;
        m_icon.sprite = i_item.icon;
        m_amount.text = Game.instance.inventory.GetItemAmount(i_item).ToString();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventory
{
    public uint GetItemAmount(ItemDef i_item);
}

public class Inventory : IInventory
{
    public struct SaveData
    {
        public Dictionary<ItemDef, uint> stock;
    }

    public SaveData m_data;

    public Inventory()
    {
        m_data = new SaveData
        {
            stock = new Dictionary<ItemDef, uint>()
        };
    }

    public void AddItem(ItemDef i_item, uint i_itemAmount)
    {
        if (m_data.stock.ContainsKey(i_item))
        {
            m_data.stock[i_item] += i_itemAmount;
        }
        else
        {
            m_data.stock.Add(i_item, i_itemAmount);
        }
    }

    public uint GetItemAmount(ItemDef i_item)
    {
        uint amount;
        //Redundant but more readable
        if (m_data.stock.TryGetValue(i_item, out amount))
        {
            return amount;
        }
        return 0;
    }
}

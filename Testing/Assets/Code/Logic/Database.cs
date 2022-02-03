using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public interface IDatabase
{
    public ItemDef FindItemByName(string i_name);
    public List<ItemDef> ListAllManaItems();
}

public class Database : IDatabase
{
    private Dictionary<string, ItemDef> m_itemsByName;

    public Database()
    {
        m_itemsByName = new Dictionary<string, ItemDef>();
        ItemDef[] allItems = Resources.LoadAll<ItemDef>("Database" + Path.DirectorySeparatorChar + "Items" + Path.DirectorySeparatorChar);
        foreach (ItemDef item in allItems)
        {
            m_itemsByName.Add(item.name, item);
        }
    }

    public ItemDef FindItemByName(string i_name)
    {
        ItemDef item;
        //Redundant, since TryGetValue assigns "null" to the item if it can't be found, but I think this is more clear
        if (m_itemsByName.TryGetValue(i_name, out item))
        {
            return item;
        }
        return null;
    }

    public List<ItemDef> ListAllManaItems()
    {
        List<ItemDef> allManaItems = new List<ItemDef>();
        foreach (ItemDef item in m_itemsByName.Values)
        {
            allManaItems.Add(item);
        }
        return allManaItems;
    }
}

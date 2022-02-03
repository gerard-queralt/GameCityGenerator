using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
    public struct SaveData
    {
        
    }

    private static Game s_instance = null;
    private SaveData m_data;
    private Database m_database;
    private Character m_character;
    private Inventory m_inventory;

    private Game()
    {
        m_database = new Database();
        m_character = new Character();
        m_inventory = new Inventory();

        SaveDelegate saveDelegate = new SaveDelegate();
        SaveDelegate.LoadResult result = saveDelegate.Load(m_database, out m_data, out m_character.m_data, out m_inventory.m_data);
        if (result == SaveDelegate.LoadResult.Failed)
        {
            m_character = new Character();
            m_data = new SaveData();
        }
    }

    public static Game instance
    {
        get {
            if (s_instance == null)
            {
                s_instance = new Game();
            }
            return s_instance;
        }
    }

    public IDatabase database
    {
        get
        {
            return m_database;
        }
    }
    public ICharacter character
    {
        get
        {
            return m_character;
        }
    }

    public IInventory inventory
    {
        get
        {
            return m_inventory;
        }
    }

    public void AddXP(int i_xp_value)
    {
        m_character.AddXP(i_xp_value);
        Save();
    }

    public void AddItem(ItemDef i_item, uint i_itemAmount)
    {
        bool accepted = true;
        if (accepted)
        {
            m_inventory.AddItem(i_item, i_itemAmount);
            Save();
        }
    }

    private void Save()
    {
        SaveDelegate saveDelegate = new SaveDelegate();
        saveDelegate.Save(m_data, m_character.m_data, m_inventory.m_data);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveDelegate
{
    private static int k_version = 2;

    public void Save(Game.SaveData i_gameSaveData, Character.SaveData i_characterSaveData, Inventory.SaveData i_inventorySaveData)
    {
        FileStream file = new FileStream(Application.persistentDataPath + Path.DirectorySeparatorChar + "data.bin", FileMode.OpenOrCreate);
        BinaryWriter writer = new BinaryWriter(file);
        writer.Write(k_version);
        writer.Write(i_characterSaveData.level);
        writer.Write(i_characterSaveData.xp);
        writer.Write(i_inventorySaveData.stock.Count);
        foreach(KeyValuePair<ItemDef, uint> pair in i_inventorySaveData.stock)
        {
            writer.Write(pair.Key.name);
            writer.Write(pair.Value);
        }
        writer.Close();
        file.Close();
    }

    public enum LoadResult
    {
        Ok,
        Failed
    }
    public LoadResult Load(IDatabase i_database, out Game.SaveData o_gameSaveData, out Character.SaveData o_characterSaveData, out Inventory.SaveData o_inventorySaveData)
    {
        o_gameSaveData = new Game.SaveData();
        o_characterSaveData = new Character.SaveData();
        o_inventorySaveData = new Inventory.SaveData();
        o_inventorySaveData.stock = new Dictionary<ItemDef, uint>();
        try
        {
            FileStream file = new FileStream(Application.persistentDataPath + Path.DirectorySeparatorChar + "data.bin", FileMode.Open);
            BinaryReader reader = new BinaryReader(file);
            int version = reader.ReadInt32();
            if (version == 1)
            {
                reader.ReadInt32();
                o_characterSaveData.level = reader.ReadInt32();
                o_characterSaveData.xp = reader.ReadInt32();
            }
            else if(version == 2)
            {
                o_characterSaveData.level = reader.ReadInt32();
                o_characterSaveData.xp = reader.ReadInt32();
                int numItemsInInventory = reader.ReadInt32();
                for (int i = 0; i < numItemsInInventory; i++)
                {
                    string itemName = reader.ReadString();
                    uint amount = reader.ReadUInt32();
                    ItemDef item = i_database.FindItemByName(itemName);
                    if (item)
                    {
                        o_inventorySaveData.stock.Add(item, amount);
                    }
                    else
                    {
                        Debug.LogError("Item " + itemName + " is not in the database");
                    }
                }
            }
            reader.Close();
            file.Close();
            return LoadResult.Ok;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            return LoadResult.Failed;
        }
    }
}

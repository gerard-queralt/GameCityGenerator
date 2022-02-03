using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPanel : MonoBehaviour
{
    public InventoryEntry m_entryTemplate;

    private void Awake()
    {
        Debug.Assert(m_entryTemplate != null, "InventoryEntry not set in InventoryPanel");
        m_entryTemplate.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        List<ItemDef> allManaItems = Game.instance.database.ListAllManaItems();
        foreach (ItemDef manaItem in allManaItems)
        {
            GameObject newEntry = Instantiate(m_entryTemplate.gameObject, transform);
            newEntry.SetActive(true);
            InventoryEntry entry = newEntry.GetComponent<InventoryEntry>();
            entry.DisplayItem(manaItem);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

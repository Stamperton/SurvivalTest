using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizedLoot : MonoBehaviour
{
    StorageBase thisStorage;

    public SO_LootTable lootTable;
    SO_LootTable _table;

    public List<InventoryEntry> lootList = new List<InventoryEntry>();

    private void Start()
    {
        _table = Instantiate(lootTable);

        thisStorage = GetComponent<StorageBase>();

        PopulateList();

        PopulateStorage();
    }

    void PopulateList()
    {
        foreach (InventoryEntry _entry in _table.possibleResources)
        {
            lootList.Add(_entry);
        }
    }

    void PopulateStorage()
    {
        int _cycleCount = Random.Range(_table.resourcesInLoot.x, _table.resourcesInLoot.y + 1);

        for (int i = 0; i < _cycleCount; i++)
        {
            int _entry = Random.Range(0, lootList.Count - 1);

            Resource _resource = lootList[_entry].resource;

            UtilityInventory.TransferWholeStackBetweenInventorySlots(thisStorage.inventoryEntries, lootList[_entry], lootList[_entry].quantityHeld);

            lootList.Remove(lootList[_entry]);
        }
    }
}

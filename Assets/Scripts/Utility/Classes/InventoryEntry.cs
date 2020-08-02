using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryEntry
{
    public Resource resource;
    public E_ResourceType resourceType;

    public int quantityHeld;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLootTable", menuName = "Loot Table")]
public class SO_LootTable : ScriptableObject
{
    public InventoryEntry[] possibleResources;
    public Vector2Int resourcesInLoot;
}

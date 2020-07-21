using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilityInventory
{
    //INVENTORY SEARCH METHODS


    //Check passed InventoryEntry list for a resource of the same type that isn't at full stack. Returns True if it does, outputs the InventoryEntry
    public static bool CheckForSameUseInventorySpace(List<InventoryEntry> inventoryList, Resource resourceToCollect, out InventoryEntry usableEntry)
    {
        for (int i = 0; i < inventoryList.Count; i++)
        {
            if (resourceToCollect.resourceType == inventoryList[i].resourceType)
            {
                if (inventoryList[i].quantityHeld >= resourceToCollect.maxStackSize)
                {
                    continue;
                }
                if (inventoryList[i].quantityHeld <= resourceToCollect.maxStackSize)
                {
                    usableEntry = inventoryList[i];
                    return true;
                }
            }
        }

        Debug.Log("Null SameUse");
        usableEntry = null;
        return false;
    }


    //Check passed InventoryEntry list for an Empty InventoryEntry slot. Returns True if it does, outputs the InventoryEntry
    public static bool CheckForEmptyInventorySpace(List<InventoryEntry> inventoryList, Resource resourceToCollect, out InventoryEntry usableEntry)
    {
        for (int i = 0; i < inventoryList.Count; i++)
        {

            if (inventoryList[i].resourceType == BuildingMaterials.Null)
            {
                //AddToInventorySlot(i, resourceToCollect);
                usableEntry = inventoryList[i];
                return true;
            }
        }

        usableEntry = null;
        return false;
    }

    //Checks for Same Type Inventory Entry, then Empty Inventory. Returns True and relevant Entry if found, False if neither.
    public static bool CheckSameUseThenEmpty(List<InventoryEntry> inventoryList, Resource resourceToCollect, out InventoryEntry usableEntry)
    {
        if (CheckForSameUseInventorySpace(inventoryList, resourceToCollect, out usableEntry))
        {
            return true;
        }

        if (CheckForEmptyInventorySpace(inventoryList, resourceToCollect, out usableEntry))
        {
            return true;
        }

        return false;
    }

    //Find a usable resource within the selected list.
    public static bool FindResource(List<InventoryEntry> inventoryList, Resource resourceToCollect, out InventoryEntry usableEntry)
    {
        for (int i = 0; i < inventoryList.Count; i++)
        {
            if (resourceToCollect.resourceType == inventoryList[i].resourceType)
            {
                usableEntry = inventoryList[i];
                return true;
            }
        }
        usableEntry = null;
        return false;
    }

    //Moves Resources between two InventoryEntry slots. Returns True if successful.
    public static void TransferBetweenInventorySlots(InventoryEntry slotToMoveFrom, InventoryEntry slotToMoveTo)
    {
        //Increment New Resource Slot
        slotToMoveTo.resource = slotToMoveFrom.resource;
        slotToMoveTo.resourceType = slotToMoveFrom.resourceType;
        slotToMoveTo.quantityHeld++;

        //Decrement Old Resource Slot
        slotToMoveFrom.quantityHeld--;
        if (slotToMoveFrom.quantityHeld <= 0)
        {
            ResetInventorySlot(slotToMoveFrom);
            //FIND A WAY TO DO THE UI GODDAMN IT
        }

    }

    public static bool TransferWholeStackBetweenInventorySlots(List<InventoryEntry> inventoryList, InventoryEntry slotToMoveFrom, int amountToMove)
    {
        for (int i = 0; i < amountToMove; i++)
        {
            if (CheckSameUseThenEmpty(inventoryList, slotToMoveFrom.resource, out InventoryEntry _entry))
            {
                TransferBetweenInventorySlots(slotToMoveFrom, _entry);
            }
            else
                return false;
        }
        return true;
    }

    public static void AddToPlayerHotbar(InventoryEntry hotbarSlotToAddTo, Resource resourceToAdd)
    {
        //Implement HOtbar
    }

    //Generates an SO_ColletableResource at targeted inventory slot.
    public static void CreateInInventorySlot(InventoryEntry slotToAddTo, Resource resourceToAdd)
    {
        slotToAddTo.resource = resourceToAdd;
        slotToAddTo.resourceType = resourceToAdd.resourceType;
        slotToAddTo.quantityHeld += resourceToAdd.quantityProduced;

    }

    public static void IncrementInventorySlot(InventoryEntry slotToIncrement, Resource resourceToAdd)
    {
        slotToIncrement.resource = resourceToAdd;
        slotToIncrement.resourceType = resourceToAdd.resourceType;
        slotToIncrement.quantityHeld++;
    }

    //Removes a single instance of a resource from a slot. Call multiple times to handle item purchasing.
    public static void DecrementInventorySlot(InventoryEntry slotToDecrement)
    {
        slotToDecrement.quantityHeld--;
        if (slotToDecrement.quantityHeld <= 0)
        {
            ResetInventorySlot(slotToDecrement);
        }
    }

    //Reset InventoryEntry to empty values.
    public static void ResetInventorySlot(InventoryEntry slotToReset)
    {
        slotToReset.resource = null;
        slotToReset.resourceType = BuildingMaterials.Null;
        slotToReset.quantityHeld = 0;

        //Find a way to update UI. Really. 
    }

    public static int TotalOfTypeInInventory(List<InventoryEntry> inventoryEntries, BuildingMaterials resourceType)
    {
        int amountHeld = 0;

        foreach (InventoryEntry entry in inventoryEntries)
        {
            if (entry.resourceType == resourceType)
                amountHeld += entry.quantityHeld;
        }

        return amountHeld;
    }
}

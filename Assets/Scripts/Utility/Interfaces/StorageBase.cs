using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageBase : MonoBehaviour
{
    public List<InventoryEntry> inventoryEntries = new List<InventoryEntry>(12);
    public GUIInventorySlot[] inventoryEntriesGUI;
    public Canvas canvasGUI;

    public bool canUseAsStorage;

    private void Start()
    {
        UpdateUI();
    }

    public virtual void OnStorageButtonPress(int inventoryPosition)
    {
        if (inventoryEntries[inventoryPosition].resource == null)
            return;

        //Checks for Same Use then Empty Slot in Player Inventory.

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (UtilityInventory.TransferWholeStackBetweenInventorySlots(PlayerInventory.instance.inventoryEntries, inventoryEntries[inventoryPosition], inventoryEntries[inventoryPosition].quantityHeld))
            {
                //TODO : Update Player UI
                UpdateUI();
                return;
            }
        }

        else if (UtilityInventory.CheckSameUseThenEmpty(PlayerInventory.instance.inventoryEntries, inventoryEntries[inventoryPosition].resource, out InventoryEntry _entry))
        {
            UtilityInventory.TransferBetweenInventorySlots(inventoryEntries[inventoryPosition], _entry);
            UpdateUI();
        }

        else
            return;
    }

    public virtual void UpdateUI()
    {
        for (int i = 0; i < inventoryEntries.Count; i++)
        {
            if (inventoryEntries[i].resource != null)
            {
                inventoryEntriesGUI[i].image.sprite = inventoryEntries[i].resource.icon;
                if (inventoryEntries[i].quantityHeld > 1)
                    inventoryEntriesGUI[i].quantity.text = inventoryEntries[i].quantityHeld.ToString();
                else
                    inventoryEntriesGUI[i].quantity.text = null;
            }
            else
            {
                inventoryEntriesGUI[i].image.sprite = GameManager.instance.blankIcon;
                inventoryEntriesGUI[i].quantity.text = null;
            }

        }
    }

    public virtual void Interact()
    {
        canvasGUI.gameObject.SetActive(true);
        PlayerInventory.instance.ToggleInventoryAndEquipment(false);
        PlayerInventory.instance.ToggleInventory(true);
        PlayerInventory.instance.currentStorage = this;

        MouseHandling.MouseToCanvasMode();
    }

    public virtual void CloseStorage()
    {
        canvasGUI.gameObject.SetActive(false);
        PlayerInventory.instance.ToggleInventory(false);
        PlayerInventory.instance.currentStorage = null;

        MouseHandling.MouseToFPSMode();
    }
}


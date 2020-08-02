using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingStation : StorageBase, IInteractable
{
    //UI Elements
    public Text recipeCostText;
    public Button craftButton;

    SO_CraftingRecipe currentRecipe = null;

    private void Start()
    {
        canUseAsStorage = false;
        canvasGUI.gameObject.SetActive(false);
    }

    public override void Interact()
    {
        base.Interact();
        currentRecipe = null;
        recipeCostText.text = "";
        UpdateUI();
    }

    public void PrepareRecipe(SO_CraftingRecipe recipe)
    {
        if (recipe == null) 
            return;

        currentRecipe = recipe;

        recipeCostText.text = String.Format("{0}\n", currentRecipe.recipeName);

        foreach (ResourceCost cost in currentRecipe.costOfRecipe)
        {
            recipeCostText.text += String.Format("{0} x {1}\n", cost.materialType.resourceType, cost.materialCost);
        }
    }


    //RECIPE BUTTON
    public void ConvertResource()
    {
        if (currentRecipe == null)
            return;

        // Check for free Output space
        if (!UtilityInventory.CheckSameUseThenEmpty(PlayerInventory.instance.inventoryEntries, currentRecipe.resourceToCraft, out InventoryEntry _output))
            return;
        //Check if Recipe is Affordable
        foreach (ResourceCost _resourceCost in currentRecipe.costOfRecipe)
        {
            int _cost = _resourceCost.materialCost;
            int _held = UtilityInventory.TotalOfTypeInInventory(PlayerInventory.instance.inventoryEntries, _resourceCost.materialType.resourceType) + UtilityInventory.TotalOfTypeInInventory(inventoryEntries, _resourceCost.materialType.resourceType);
            if (_held >= _cost)
                continue;
            else
            {
                Debug.Log("Recipe Too Expensive");
                return;
            }
        }
        //Pay for Recipe
        foreach (ResourceCost _resourceCost in currentRecipe.costOfRecipe)
        {
            int _cost = _resourceCost.materialCost;
            for (int i = 0; i < _cost; i++)
            {
                if (UtilityInventory.FindResource(PlayerInventory.instance.inventoryEntries, _resourceCost.materialType, out InventoryEntry _entry))
                {
                    UtilityInventory.DecrementInventorySlot(_entry);
                }
            }
        }
        //Generate Resource
        UtilityInventory.CreateInInventorySlot(_output, currentRecipe.resourceToCraft);
        UpdateUI();
    }

    public override void UpdateUI()
    {
        base.UpdateUI();
    }

    public override void OnStorageButtonPress(int inventoryPosition)
    {
        base.OnStorageButtonPress(inventoryPosition);

        //if (inventoryEntries[inventoryPosition].resource == null)
        //    return;

        ////Checks for Same Use then Empty Slot in Player Inventory.
        //if (UtilityInventory.CheckSameUseThenEmpty(PlayerInventory.instance.inventoryEntries, inventoryEntries[inventoryPosition].resource, out InventoryEntry _entry))
        //{
        //    UtilityInventory.TransferBetweenInventorySlots(inventoryEntries[inventoryPosition], _entry);
        //    UpdateUI();
        //}
        //else
        //    return;
    }
}

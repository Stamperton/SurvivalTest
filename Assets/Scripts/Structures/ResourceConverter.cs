using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceConverter : StorageBase, IInteractable
{
    public InventoryEntry outputInventoryEntry;

    public GUIInventorySlot outputInventoryEntryGUI;

    public SO_CraftingRecipe[] conversionRecipes;
    SO_CraftingRecipe currentRecipe;

    public Slider progressBar;

    bool isConverting = false;


    //Timer Test Variables
    float timer;


    private void Start()
    {
        canvasGUI.gameObject.SetActive(false);
    }   

    public void CheckRecipe()
    {
        if (isConverting)
            return;

        currentRecipe = null;

        for (int i = 0; i < conversionRecipes.Length; i++)
        {
            if (inventoryEntries[0].resourceType == conversionRecipes[i].costOfRecipe[0].materialType.resourceType)
            {
                currentRecipe = conversionRecipes[i];

                if (conversionRecipes[i].craftingTime == 0)
                {
                    if (PayForConversion())
                        GenerateResource();
                }
                else
                {
                    if (PayForConversion())
                    {
                        timer = 0;
                        isConverting = true;
                    }
                }
                return;
            }
        }
    }

    void ConvertResource(SO_CraftingRecipe recipe)
    {
        UtilityInventory.CreateInInventorySlot(outputInventoryEntry, recipe.resourceToCraft);
    }

    private void Update()
    {
        //REMOVE, PURELY FOR RESTING
        UpdateUI();

        if (inventoryEntries[0].resource != null)
        {
            CheckRecipe();
            Debug.Log("Checking Furnace Recipe");
        }

        if (isConverting)
        {
            Debug.Log(timer);
            progressBar.maxValue = currentRecipe.craftingTime;
            timer += Time.fixedUnscaledDeltaTime;
            progressBar.value = timer;

            if (timer >= currentRecipe.craftingTime)
            {
                isConverting = false;
                timer = 0;
                GenerateResource();
            }
        }
    }

    public bool PayForConversion()
    {
        if (currentRecipe == null)
            return false;

        // Check for free Output space
        if (outputInventoryEntry.resource != currentRecipe.resourceToCraft || outputInventoryEntry.quantityHeld >= outputInventoryEntry.resource.maxStackSize)
        {
            if (outputInventoryEntry.resource != null)
            {
                return false;
            }
        }

        //Check if Recipe is Affordable
        foreach (ResourceCost _resourceCost in currentRecipe.costOfRecipe)
        {
            int _cost = _resourceCost.materialCost;
            int _held = UtilityInventory.TotalOfTypeInInventory(inventoryEntries, _resourceCost.materialType.resourceType);
            if (_held >= _cost)
                continue;
            else
            {
                Debug.Log("Recipe Too Expensive");
                return false;
            }
        }
        //Pay for Recipe
        foreach (ResourceCost _resourceCost in currentRecipe.costOfRecipe)
        {
            int _cost = _resourceCost.materialCost;
            for (int i = 0; i < _cost; i++)
            {
                if (UtilityInventory.FindResource(inventoryEntries, _resourceCost.materialType, out InventoryEntry _entry))
                {
                    UtilityInventory.DecrementInventorySlot(_entry);
                }
            }
        }
        //Generate Resource
        return true;
    }

    void GenerateResource()
    {
        UtilityInventory.CreateInInventorySlot(outputInventoryEntry, currentRecipe.resourceToCraft);
    }


    public virtual void OnOutputButtonPress()
    {
        if (outputInventoryEntry.resource == null)
            return;

        //Checks for Same Use then Empty Slot in Player Inventory.

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (UtilityInventory.TransferWholeStackBetweenInventorySlots(PlayerInventory.instance.inventoryEntries, outputInventoryEntry, outputInventoryEntry.quantityHeld))
            {
                //TODO : Update Player UI
                return;
            }
        }

        else if (UtilityInventory.CheckForSameUseInventorySpace(PlayerInventory.instance.inventoryEntries, outputInventoryEntry.resource, out InventoryEntry _entry))
        {
            UtilityInventory.TransferBetweenInventorySlots(outputInventoryEntry, _entry);
        }

        else if (UtilityInventory.CheckForEmptyInventorySpace(PlayerInventory.instance.inventoryEntries, outputInventoryEntry.resource, out InventoryEntry _emptyentry))
        {
            UtilityInventory.TransferBetweenInventorySlots(outputInventoryEntry, _emptyentry);
        }


        else
            return;
    }

    public override void UpdateUI()
    {
        base.UpdateUI();

        if (outputInventoryEntry.resource != null)
        {
            outputInventoryEntryGUI.image.sprite = outputInventoryEntry.resource.icon;
            if (outputInventoryEntry.quantityHeld > 1)
                outputInventoryEntryGUI.quantity.text = outputInventoryEntry.quantityHeld.ToString();
        }
        else
        {
            outputInventoryEntryGUI.image.sprite = GameManager.instance.blankIcon;
            outputInventoryEntryGUI.quantity.text = null;
        }
    }
}

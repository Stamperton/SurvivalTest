using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingPlot : StorageBase, IInteractable
{
    public GameObject plantPlot;
    public SO_PlantingRecipe[] growingRecipes;

    GameObject growingPlant;

    SO_PlantingRecipe currentRecipe;

    public InventoryEntry seedInventoryEntry;

    float timer = 0f;
    int currentGrowState = 0;

    bool isPlanted;

    private void Start()
    {
        UpdateUI();

        canvasGUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isPlanted)
            return;

        if (inventoryEntries[0].resource != null)
        {
            CheckRecipe();
        }
    }

    public override void Interact()
    {
        if (isPlanted)
            return;

        base.Interact();
    }

    public void CheckRecipe()
    {
        if (isPlanted)
            return;

        currentRecipe = null;

        for (int i = 0; i < growingRecipes.Length; i++)
        {
            if (inventoryEntries[0].resource.resourceType == growingRecipes[i].requiredSeed)
            {
                UtilityInventory.DecrementInventorySlot(inventoryEntries[0]);
                currentRecipe = growingRecipes[i];

                currentGrowState = 0;

                UpdateUI();

                CloseStorage();

                isPlanted = true;

                StartCoroutine(IE_GrowPlant());

                return;
            }
        }
    }

    public void OnHarvestPlant()
    {
        //Populate SeedEntry with Random number of Seeds
        UtilityInventory.ResetInventorySlot(seedInventoryEntry);
        int seedAmount = Mathf.CeilToInt(Random.Range(currentRecipe.seedAmountRange.x, currentRecipe.seedAmountRange.y));
        UtilityInventory.CreateInInventorySlot(seedInventoryEntry, currentRecipe.seed);
        if (seedAmount > 1)
        {
            for (int i = 0; i < seedAmount - 1; i++)
            {
                UtilityInventory.IncrementInventorySlot(seedInventoryEntry, currentRecipe.seed);
            }
        }

        if (UtilityInventory.CheckSameUseThenEmpty(PlayerInventory.instance.inventoryEntries, currentRecipe.seed, out InventoryEntry _seedEntry))
        {
            UtilityInventory.TransferWholeStackBetweenInventorySlots(PlayerInventory.instance.inventoryEntries, seedInventoryEntry, seedInventoryEntry.quantityHeld);
        }
        else
            PlayerInventory.instance.DropItem(seedInventoryEntry);

        isPlanted = false;
    }


    IEnumerator IE_GrowPlant()
    {
        growingPlant = Instantiate(currentRecipe.plantGrowStates[currentGrowState], plantPlot.transform.position, plantPlot.transform.rotation);
        growingPlant.transform.SetParent(plantPlot.transform);

        for (int i = 0; i < currentRecipe.plantGrowStates.Length -1; i++)
        {
            yield return new WaitForSeconds(currentRecipe.growingTime / currentRecipe.plantGrowStates.Length);
            currentGrowState++;
            Destroy(growingPlant);

            growingPlant = Instantiate(currentRecipe.plantGrowStates[currentGrowState], plantPlot.transform.position, plantPlot.transform.rotation);
            growingPlant.transform.SetParent(plantPlot.transform);
            Debug.Log(currentGrowState);
                     
        }

        StopCoroutine(IE_GrowPlant());
    }
}


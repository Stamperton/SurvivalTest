using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingPlot : StorageBase, IInteractable
{
    public GameObject plantPlot;
    public SO_PlantingRecipe[] growingRecipes;

    public BoxCollider harvestCollider;
    CrosshairTooltip plotTooltip;

    SO_PlantingRecipe currentRecipe;
    GameObject growingPlant;
    public InventoryEntry seedInventoryEntry;

    float timer = 0f;

    public Resource startingSeed;
    public bool blockPlanting = false;

    bool isPlanted;
    bool canHarvest;

    private void Start()
    {
        harvestCollider.enabled = false;
        plotTooltip = GetComponent<CrosshairTooltip>();
        canvasGUI.gameObject.SetActive(false);

        if (startingSeed != null)
        {
            UtilityInventory.CreateInInventorySlot(inventoryEntries[0], startingSeed);
        }
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
        if (isPlanted && !canHarvest)
            return;

        else if (canHarvest)
        {
            HarvestPlant();
            return;
        }

        else if (!blockPlanting)
            base.Interact();
    }

    public void CheckRecipe()
    {
        if (isPlanted)
            return;

        currentRecipe = null;

        for (int i = 0; i < growingRecipes.Length; i++)
        {
            if (inventoryEntries[0].resourceType == growingRecipes[i].requiredSeed)
            {
                currentRecipe = growingRecipes[i];
                growingPlant = Instantiate(currentRecipe.plantToGrow, plantPlot.transform.position, plantPlot.transform.rotation);
                growingPlant.transform.localScale = Vector3.zero;
                growingPlant.transform.SetParent(plantPlot.transform);
                UtilityInventory.DecrementInventorySlot(inventoryEntries[0]);

                UpdateUI();
                CloseStorage();
                isPlanted = true;
                StartCoroutine(GrowPlant());

                //canHarvest = true;
            }
        }
    }

    void HarvestPlant()
    {
        if (UtilityInventory.CheckSameUseThenEmpty(PlayerInventory.instance.inventoryEntries, currentRecipe.plantResource, out InventoryEntry _entry))
        {
            UtilityInventory.CreateInInventorySlot(_entry, currentRecipe.plantResource);
        }
        else
            return;

        //Populate SeedEntry with Random number of Seeds
        UtilityInventory.ResetInventorySlot(seedInventoryEntry);
        int seedAmount = Mathf.CeilToInt(Random.Range(currentRecipe.seedAmountRange.x, currentRecipe.seedAmountRange.y));
        UtilityInventory.CreateInInventorySlot(seedInventoryEntry, currentRecipe.plantSeed);
        if (seedAmount > 1)
        {
            for (int i = 0; i < seedAmount - 1; i++)
            {
                UtilityInventory.IncrementInventorySlot(seedInventoryEntry, currentRecipe.plantSeed);
            }
        }

        if (UtilityInventory.CheckSameUseThenEmpty(PlayerInventory.instance.inventoryEntries, currentRecipe.plantSeed, out InventoryEntry _seedEntry))
        {
            UtilityInventory.TransferWholeStackBetweenInventorySlots(PlayerInventory.instance.inventoryEntries, seedInventoryEntry, seedInventoryEntry.quantityHeld);
        }
        else
            PlayerInventory.instance.DropItem(seedInventoryEntry);

        if (blockPlanting)
            Destroy(gameObject);
        else
            Destroy(growingPlant);


        harvestCollider.enabled = false;
        canHarvest = false;
        isPlanted = false;

    }

    void MakeHarvestable()
    {
        PlotTooltip(currentRecipe.recipeName);
        canHarvest = true;
    }

    IEnumerator GrowPlant()
    {
        harvestCollider.enabled = true;
        timer = 0;

        PlotTooltip(currentRecipe.recipeName + " (Growing)");

        Vector3 startSize = new Vector3(0, 0, 0);
        Vector3 maxSize = new Vector3(1, 1, 1);

        do
        {
            growingPlant.transform.localScale = Vector3.Lerp(startSize, maxSize, timer / currentRecipe.growingTime);
            timer += Time.deltaTime;
            yield return null;
        } while (timer < currentRecipe.growingTime);

        MakeHarvestable();

    }

    void PlotTooltip(string _text)
    {
        if (!blockPlanting)
        {
            plotTooltip.toolTipText = _text + "\nGrowing Plot";
        }
        else
            plotTooltip.toolTipText = _text;

    }
}


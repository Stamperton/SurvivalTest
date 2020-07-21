using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BuildableObject : MonoBehaviour, IInteractable
{
    [Header("UI Variables")]
    public string buildingName;
    public Text buildingCostText;
    public Text playerCostText;
    public Text buildingNameText;

    public GameObject BuildingOutline;
    public GameObject ActualBuilding;

    Collider thisCollider;

    Canvas BuildCanvas;

    public ResourceCost[] totalCost;


    private void Start()
    {
        BuildCanvas = GetComponentInChildren<Canvas>();
        BuildCanvas.enabled = false;

        thisCollider = GetComponent<Collider>();

        GameManager.instance.onEnterBuildMode += MakeBuildingOutlineVisible;
        GameManager.instance.onExitBuildMode += MakeBuildingOutlineInvisible;

        MakeBuildingOutlineInvisible();
        ActualBuilding.transform.position = new Vector3(0, -100, 0);
    }

    void PopulateCanvas()
    {
        buildingNameText.text = buildingName;
        //buildingCostText.text = "This Building Costs\n";
        buildingCostText.text = null;
        //playerCostText.text = "You Currently Have\n";
        playerCostText.text = null;
        foreach (ResourceCost cost in totalCost)
        {
            buildingCostText.text += String.Format("{0} x {1}\n", cost.materialType.resourceType, cost.materialCost);
        }

        foreach (ResourceCost cost in totalCost)
        {
            int materialCount = UtilityInventory.TotalOfTypeInInventory(PlayerInventory.instance.inventoryEntries, cost.materialType.resourceType);
            playerCostText.text += String.Format("{0} x {1}\n", cost.materialType.resourceType, materialCount);
        }
    }

    void MakeBuildingOutlineVisible()
    {
        BuildingOutline.SetActive(true);
        thisCollider.enabled = true;
    }

    void MakeBuildingOutlineInvisible()
    {
        BuildingOutline.SetActive(false);
        thisCollider.enabled = false;
    }

    public void BuildBuilding()
    {
        GameManager.instance.onEnterBuildMode -= MakeBuildingOutlineVisible;
        GameManager.instance.onExitBuildMode -= MakeBuildingOutlineInvisible;

        MakeBuildingOutlineInvisible();
        ActualBuilding.transform.position = this.transform.position;

        BuildCanvas.enabled = false;
        MouseHandling.MouseToFPSMode();
    }

    public void Interact()
    {
        PopulateCanvas();
        BuildCanvas.enabled = true;
        MouseHandling.MouseToCanvasMode();
    }

    public void CancelCanvas()
    {
        BuildCanvas.enabled = false;
        MouseHandling.MouseToFPSMode();
    }

    public void CheckToBuild()
    {
        foreach (ResourceCost cost in totalCost)
        {
            int materialHeld = 0;

            materialHeld = PlayerInventory.instance.TotalOfTypeInInventory(cost.materialType.resourceType);
            if (materialHeld >= cost.materialCost)
            {
                continue;
            }
            else
            {
                //TODO : "Can't Build" Error
                Debug.Log("Can't Afford Building");
                return;
            }
        }

        PayForBuilding();

        BuildBuilding();
    }

    public void PayForBuilding()
    {
        foreach (ResourceCost cost in totalCost)
        {
            for (int i = 0; i < cost.materialCost; i++)
            {
                UtilityInventory.FindResource(PlayerInventory.instance.inventoryEntries, cost.materialType, out InventoryEntry _entry);
                UtilityInventory.DecrementInventorySlot(_entry);

                //PlayerInventory.instance.RemoveFromInventory(cost.materialType);
            }
        }
        
    }
}

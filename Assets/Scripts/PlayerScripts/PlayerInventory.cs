using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    #region Singleton
    public static PlayerInventory instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Multiple PlayerInventory in Scene");
            Destroy(this.gameObject);
        }
    }
    #endregion

    [Header("Lists & Setup")]
    public List<InventoryEntry> inventoryEntries = new List<InventoryEntry>(8);
    public GUIInventorySlot[] GUIInventorySlots = new GUIInventorySlot[8];

    [Header("Dropped GO Setup")]
    public GameObject droppedItemsPrefab;
    public Transform droppedItemSpawnPoint;

    //Storage Variables
    [HideInInspector] public StorageBase currentStorage;
    [HideInInspector] public bool canToggleInventory = true;

    ResourceTool tool; //Equipped Tool

    //UI Variables
    [Header("UI Variables")]
    public GameObject inventoryPanel;
    public GameObject equipmentPanel;

    public GameObject useButton;
    public GameObject equipButton;
    public GameObject discardPanel;
    public GameObject itemInteractionPanel;
    //Currently Initialised Button
    int currentButton;


    private void Start()
    {
        for (int i = 0; i < GUIInventorySlots.Length; i++)
        {
            EmptySlot(i);
        }

        ToggleInventoryAndEquipment(false);

        SetItemInteractionPanel(false);
    }


    //TESTING PURPOSES ONLY DO NOT KEEP
    private void Update()
    {
        UpdateUI();
    }


    public bool CollectSomething(Resource resourceToCollect)
    {
        InventoryEntry _entry;

        if (UtilityInventory.CheckSameUseThenEmpty(inventoryEntries, resourceToCollect, out _entry))
        {
            UtilityInventory.CreateInInventorySlot(_entry, resourceToCollect);
            UpdateUI();
            return true;
        }


        return false;
    }

    //Called by button on GUI. Moves things to Storage or Hotbar.
    public void OnInventoryButtonPress(int inventoryPosition)
    {
        //Empty Slot? Do Nothing.
        if (inventoryEntries[inventoryPosition].resource == null)
        {
            SetItemInteractionPanel(false);
            return;
        }

        //First, Check for Currently Accessed Storage
        if (currentStorage != null && currentStorage.canUseAsStorage)
        {
            Debug.Log("Moving From Inventory");

            //Encapsulate in Method Eventually Probably
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (UtilityInventory.TransferWholeStackBetweenInventorySlots(currentStorage.inventoryEntries, inventoryEntries[inventoryPosition], inventoryEntries[inventoryPosition].quantityHeld))
                {
                    currentStorage.UpdateUI();
                    SetItemInteractionPanel(false);
                    UpdateUI();
                    return;
                }
            }

            else if (UtilityInventory.CheckSameUseThenEmpty(currentStorage.inventoryEntries, inventoryEntries[inventoryPosition].resource, out InventoryEntry _entry))
            {
                UtilityInventory.TransferBetweenInventorySlots(inventoryEntries[inventoryPosition], _entry);

                currentStorage.UpdateUI();
                SetItemInteractionPanel(false);
                UpdateUI();
                return;

            }
        }


        InitialiseButtonSelection(inventoryPosition);

        SetItemInteractionPanel(true);
        //Send to Hotbar (When Implimented)

    }


    //Returns Total of any particular BuildingMaterial in PlayerInventory. Useful for UI & building recipes.
    public int TotalOfTypeInInventory(BuildingMaterials resourceType)
    {
        int amountHeld = 0;

        foreach (InventoryEntry entry in inventoryEntries)
        {
            if (entry.resourceType == resourceType)
                amountHeld += entry.quantityHeld;
        }

        return amountHeld;
    }

    //Updates all attached UI. Does not Initialise.
    void UpdateUI()
    {
        if (!inventoryPanel.activeInHierarchy)
            return;

        for (int i = 0; i < inventoryEntries.Count; i++)
        {
            if (inventoryEntries[i].resource != null)
            {
                GUIInventorySlots[i].image.sprite = inventoryEntries[i].resource.icon;
                if (inventoryEntries[i].quantityHeld > 1)
                    GUIInventorySlots[i].quantity.text = inventoryEntries[i].quantityHeld.ToString();
                else
                    GUIInventorySlots[i].quantity.text = null;
            }
            else
            {
                GUIInventorySlots[i].image.sprite = GameManager.instance.blankIcon;
                GUIInventorySlots[i].quantity.text = null;

            }
        }
    }

    public void InitialiseButtonSelection(int ItemSlot)
    {
        currentButton = ItemSlot;

        if (inventoryEntries[ItemSlot].resource.isEquippable)
            equipButton.SetActive(true);
        else
            equipButton.SetActive(false);

        if (inventoryEntries[ItemSlot].resource.isUsable)
            useButton.SetActive(true);
        else
            useButton.SetActive(false);

    }

    public void EquipItem()
    {
        tool = inventoryEntries[currentButton].resource.GetComponent<ResourceTool>();

        PlayerEquipmentManager.instance.EquipItem(tool, inventoryEntries[currentButton]);
    }

    void UnequipItem()
    {
        PlayerEquipmentManager.instance.UnequipItem(tool);
        tool = null;
    }

    public void ConfirmDrop()
    {
        discardPanel.SetActive(true);
    }

    public void DiscardItem()
    {
        UtilityInventory.ResetInventorySlot(inventoryEntries[currentButton]);
        discardPanel.SetActive(false);
        SetItemInteractionPanel(false);
    }

    public void CancelDiscard()
    {
        discardPanel.SetActive(false);
        SetItemInteractionPanel(false);
    }

    public void UseItem()
    {
        inventoryEntries[currentButton].resource.Use();
        UtilityInventory.DecrementInventorySlot(inventoryEntries[currentButton]);
    }

    public void DropItem()
    {
        InventoryEntry _entry = inventoryEntries[currentButton];

        GameObject _dropped = Instantiate(droppedItemsPrefab, droppedItemSpawnPoint.position, droppedItemSpawnPoint.rotation);

        ResourceTool _entryTool = _entry.resource.GetComponent<ResourceTool>();
        if (_entryTool != null)
        {
            _dropped.AddComponent<ResourceTool>();
            ResourceTool _tool = _dropped.GetComponent<ResourceTool>();

            _tool.currentDurability = _entryTool.currentDurability;
            _tool.maxDurability = _entryTool.maxDurability;
            _tool.isEquipped = false;
        }
        else
            _dropped.AddComponent<Resource>();

        Resource _resource = _dropped.GetComponent<Resource>();

        CrosshairTooltip toolTip = _dropped.GetComponent<CrosshairTooltip>();

        CollectableResource _collectableResource = _dropped.GetComponent<CollectableResource>();
        _resource.icon = _entry.resource.icon;
        _resource.maxStackSize = _entry.resource.maxStackSize;
        _resource.resourceType = _entry.resource.resourceType;
        _resource.quantityProduced = _entry.resource.quantityProduced;
        _resource.objectToEquip = _entry.resource.objectToEquip;
        _resource.isEquippable = _entry.resource.isEquippable;
        _resource.isUsable = _entry.resource.isUsable;

        _collectableResource.quantityHeld = _entry.quantityHeld;
        _collectableResource.quantityRange = new Vector2(_entry.quantityHeld, _entry.quantityHeld);
        _collectableResource.collectionAmount = _entry.quantityHeld;
        _collectableResource.collectionRange = new Vector2(_entry.quantityHeld, _entry.quantityHeld);
        _collectableResource.requiredTool = PlayerTool.None;

        if (_collectableResource.quantityHeld > 1)
            toolTip.toolTipText = _entry.resource.resourceName + (" x ") + _collectableResource.quantityHeld;
        else
            toolTip.toolTipText = _entry.resource.resourceName;

        DiscardItem();
    }

    public void DropItem(InventoryEntry _entry)
    {
        GameObject _dropped = Instantiate(droppedItemsPrefab, droppedItemSpawnPoint.position, droppedItemSpawnPoint.rotation);

        ResourceTool _entryTool = _entry.resource.GetComponent<ResourceTool>();
        if (_entryTool != null)
        {
            _dropped.AddComponent<ResourceTool>();
            ResourceTool _tool = _dropped.GetComponent<ResourceTool>();

            _tool.currentDurability = _entryTool.currentDurability;
            _tool.maxDurability = _entryTool.maxDurability;
            _tool.isEquipped = false;
        }
        else
            _dropped.AddComponent<Resource>();

        Resource _resource = _dropped.GetComponent<Resource>();

        CrosshairTooltip toolTip = _dropped.GetComponent<CrosshairTooltip>();

        CollectableResource _collectableResource = _dropped.GetComponent<CollectableResource>();
        _resource.icon = _entry.resource.icon;
        _resource.maxStackSize = _entry.resource.maxStackSize;
        _resource.resourceType = _entry.resource.resourceType;
        _resource.quantityProduced = _entry.resource.quantityProduced;
        _resource.objectToEquip = _entry.resource.objectToEquip;
        _resource.isEquippable = _entry.resource.isEquippable;
        _resource.isUsable = _entry.resource.isUsable;

        _collectableResource.quantityHeld = _entry.quantityHeld;
        _collectableResource.quantityRange = new Vector2(_entry.quantityHeld, _entry.quantityHeld);
        _collectableResource.collectionAmount = _entry.quantityHeld;
        _collectableResource.collectionRange = new Vector2(_entry.quantityHeld, _entry.quantityHeld);
        _collectableResource.requiredTool = PlayerTool.None;

        if (_collectableResource.quantityHeld > 1)
            toolTip.toolTipText = _entry.resource.resourceName + (" x ") + _collectableResource.quantityHeld;
        else
            toolTip.toolTipText = _entry.resource.resourceName;
    }

    //UI Handling Methods
    #region UI Handling


    void SetItemInteractionPanel(bool _state)
    {
        itemInteractionPanel.SetActive(_state);

    }

    public void EmptySlot(int slotNumber)
    {
        GUIInventorySlots[slotNumber].image.sprite = GameManager.instance.blankIcon;
        GUIInventorySlots[slotNumber].quantity.text = null;
    }

    public void ToggleInventory(bool active)
    {
        canToggleInventory = !active;

        if (active == true)
            MouseHandling.MouseToCanvasMode();
        else
            MouseHandling.MouseToFPSMode();

        inventoryPanel.SetActive(active);
        PlayerInventory.instance.itemInteractionPanel.SetActive(false);
    }

    public void ToggleInventoryAndEquipment()
    {
        if (!canToggleInventory)
            return;

        inventoryPanel.SetActive(!inventoryPanel.activeInHierarchy);
        equipmentPanel.SetActive(!equipmentPanel.activeInHierarchy);

        PlayerInventory.instance.itemInteractionPanel.SetActive(false);

        if (inventoryPanel.activeInHierarchy)
            MouseHandling.MouseToCanvasMode();
        else
            MouseHandling.MouseToFPSMode();
    }

    public void ToggleInventoryAndEquipment(bool active)
    {
        canToggleInventory = !active;
        inventoryPanel.SetActive(active);
        equipmentPanel.SetActive(active);

        PlayerInventory.instance.itemInteractionPanel.SetActive(false);

    }

    #endregion
}


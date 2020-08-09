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
    [SerializeField] GameObject droppedItemsPrefab;
    [SerializeField] Transform droppedItemSpawnPoint;

    //Storage Variables
    [HideInInspector] public StorageBase currentStorage;
    [HideInInspector] public bool canToggleInventory = true;

    //UI Variables
    [Header("UI Variables")]
    ItemInteractionPanel itemInteractionPanel;
    [SerializeField] GameObject inventoryPanel;

    //Currently Initialised Button
    int currentButton;


    private void Start()
    {
        itemInteractionPanel = ItemInteractionPanel.instance;

        for (int i = 0; i < GUIInventorySlots.Length; i++)
        {
            EmptySlot(i);
        }

        ToggleInventory();
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
            itemInteractionPanel.CloseItemPanel();
            return;
        }

        currentButton = inventoryPosition;

        //First, Check for Currently Accessed Storage
        if (currentStorage != null && currentStorage.canUseAsStorage)
        {
            Debug.Log("Moving From Inventory");

            //Encapsulate in Method Eventually Probably
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (inventoryEntries[currentButton].resource.isEquipped)
                    EquipItemButton();

                if (UtilityInventory.TransferWholeStackBetweenInventorySlots(currentStorage.inventoryEntries, inventoryEntries[inventoryPosition], inventoryEntries[inventoryPosition].quantityHeld))
                {

                    currentStorage.UpdateUI();
                    UpdateUI();
                    return;
                }
            }

            else if (UtilityInventory.CheckSameUseThenEmpty(currentStorage.inventoryEntries, inventoryEntries[inventoryPosition].resource, out InventoryEntry _entry))
            {
                if (inventoryEntries[currentButton].resource.isEquipped)
                    EquipItemButton();

                UtilityInventory.TransferBetweenInventorySlots(inventoryEntries[inventoryPosition], _entry);

                currentStorage.UpdateUI();
                UpdateUI();
                return;

            }
        }

        itemInteractionPanel.PopulateItemPanel(inventoryEntries[inventoryPosition]);

    }


    //Returns Total of any particular BuildingMaterial in PlayerInventory. Useful for UI & building recipes.
    public int TotalOfTypeInInventory(E_ResourceType resourceType)
    {
        int amountHeld = 0;

        foreach (InventoryEntry entry in inventoryEntries)
        {
            if (entry.resourceType == resourceType)
                amountHeld += entry.quantityHeld;
        }

        return amountHeld;
    }

    public void EquipItemButton()
    {
        PlayerEquipmentManager.instance.EquipItem(inventoryEntries[currentButton].resource);
        itemInteractionPanel.CloseItemPanel();

    }

    public void DiscardItem()
    {
        UtilityInventory.ResetInventorySlot(inventoryEntries[currentButton]);
        itemInteractionPanel.CloseItemPanel();
    }

    public void UseItemButton()
    {
        inventoryEntries[currentButton].resource.Use();
        UtilityInventory.DecrementInventorySlot(inventoryEntries[currentButton]);

        if (inventoryEntries[currentButton].quantityHeld <= 0)
        {
            itemInteractionPanel.CloseItemPanel();
        }
    }


    public void DropItemButton()
    {
        InventoryEntry _entry = inventoryEntries[currentButton];

        GameObject _dropped = Instantiate(droppedItemsPrefab, droppedItemSpawnPoint.position, droppedItemSpawnPoint.rotation);

        Resource _resource = _dropped.GetComponent<Resource>();

        CrosshairTooltip toolTip = _dropped.GetComponent<CrosshairTooltip>();

        CollectableResource _collectableResource = _dropped.GetComponent<CollectableResource>();
        _resource.icon = _entry.resource.icon;
        _resource.maxStackSize = _entry.resource.maxStackSize;
        _resource.resourceType = _entry.resource.resourceType;
        _resource.quantityProduced = 1;
        _resource.objectToEquip = _entry.resource.objectToEquip;
        _resource.isEquippable = _entry.resource.isEquippable;
        _resource.isUsable = _entry.resource.isUsable;
        _resource.currentDurability = _entry.resource.currentDurability;
        _resource.maxDurability = _entry.resource.maxDurability;
        _resource.resourceName = _entry.resource.resourceName;
        _resource.resourceText = _entry.resource.resourceText;

        _resource.isEquipped = false;

        _collectableResource.quantityHeld = _entry.quantityHeld;
        _collectableResource.quantityRange = new Vector2Int(_entry.quantityHeld, _entry.quantityHeld);
        _collectableResource.collectionAmount = _entry.quantityHeld;
        _collectableResource.collectionRange = new Vector2Int(_entry.quantityHeld, _entry.quantityHeld);
        _collectableResource.requiredTool = E_ToolType.None;

        if (_collectableResource.quantityHeld > 1)
            toolTip.toolTipText = _entry.resource.resourceName + (" x ") + _collectableResource.quantityHeld;
        else
            toolTip.toolTipText = _entry.resource.resourceName;

        itemInteractionPanel.CloseItemPanel();

        DiscardItem();
    }


    //TODO: Duplication Bug with Multiple Produced Item Quantities. 
    public void DropItem(InventoryEntry _entry)
    {
        GameObject _dropped = Instantiate(droppedItemsPrefab, droppedItemSpawnPoint.position, droppedItemSpawnPoint.rotation);

        Resource _resource = _dropped.GetComponent<Resource>();

        CrosshairTooltip toolTip = _dropped.GetComponent<CrosshairTooltip>();

        CollectableResource _collectableResource = _dropped.GetComponent<CollectableResource>();
        _resource.icon = _entry.resource.icon;
        _resource.maxStackSize = _entry.resource.maxStackSize;
        _resource.resourceType = _entry.resource.resourceType;
        _resource.quantityProduced = 1;
        _resource.objectToEquip = _entry.resource.objectToEquip;
        _resource.isEquippable = _entry.resource.isEquippable;
        _resource.isUsable = _entry.resource.isUsable;
        _resource.currentDurability = _entry.resource.currentDurability;
        _resource.maxDurability = _entry.resource.maxDurability;
        _resource.resourceName = _entry.resource.resourceName;
        _resource.resourceText = _entry.resource.resourceText;

        _resource.isEquipped = false;

        _collectableResource.quantityHeld = _entry.quantityHeld;
        _collectableResource.quantityRange = new Vector2Int(_entry.quantityHeld, _entry.quantityHeld);
        _collectableResource.collectionAmount = _entry.quantityHeld;
        _collectableResource.collectionRange = new Vector2Int(_entry.quantityHeld, _entry.quantityHeld);
        _collectableResource.requiredTool = E_ToolType.None;

        if (_collectableResource.quantityHeld > 1)
            toolTip.toolTipText = _entry.resource.resourceName + (" x ") + _collectableResource.quantityHeld;
        else
            toolTip.toolTipText = _entry.resource.resourceName;

        itemInteractionPanel.CloseItemPanel();

        DiscardItem();
    }

    //UI Handling Methods
    #region UI Handling

    public void EmptySlot(int slotNumber)
    {
        GUIInventorySlots[slotNumber].image.sprite = GameManager.instance.blankIcon;
        GUIInventorySlots[slotNumber].quantity.text = null;
    }

    public void ToggleInventory()
    {
        if (!canToggleInventory)
            return;

        if (inventoryPanel.activeInHierarchy == true)
        {
            inventoryPanel.SetActive(false);
            MouseHandling.MouseToFPSMode();
        }
        else
        {
            inventoryPanel.SetActive(true);
            MouseHandling.MouseToCanvasMode();
        }

    }

    public void CloseInventoryButton()
    {
        if (currentStorage)
            currentStorage.CloseStorage();
        else
            ToggleInventory(false);
    }

    public void ToggleInventory(bool active)
    {
        canToggleInventory = !active;

        inventoryPanel.SetActive(active);

        if (active == true)
            MouseHandling.MouseToCanvasMode();
        else
            MouseHandling.MouseToFPSMode();
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

                if (inventoryEntries[i].resource.currentDurability < inventoryEntries[i].resource.maxDurability)
                {
                    GUIInventorySlots[i].durabilitySlider.gameObject.SetActive(true);
                    GUIInventorySlots[i].durabilitySlider.maxValue = inventoryEntries[i].resource.maxDurability;
                    GUIInventorySlots[i].durabilitySlider.value = inventoryEntries[i].resource.currentDurability;
                }

                if (inventoryEntries[i].resource.isEquipped)
                    GUIInventorySlots[i].equippedIndicator.SetActive(true);
                else
                    GUIInventorySlots[i].equippedIndicator.SetActive(false);


            }
            else
            {
                GUIInventorySlots[i].image.sprite = GameManager.instance.blankIcon;
                GUIInventorySlots[i].quantity.text = null;
                GUIInventorySlots[i].durabilitySlider.gameObject.SetActive(false);
                GUIInventorySlots[i].equippedIndicator.SetActive(false);
            }
        }
    }

    #endregion
}


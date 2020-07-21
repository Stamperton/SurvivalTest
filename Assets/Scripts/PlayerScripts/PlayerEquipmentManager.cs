using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    #region Singleton
    public static PlayerEquipmentManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Debug.LogError("Too Many PlayerEquipmentManagers In Scene");
            Destroy(this.gameObject);
        }
    }


    #endregion

    Dictionary<PlayerTool, GameObject> toolDictionary = new Dictionary<PlayerTool, GameObject>();

    [Header("Inventory Elements")]
    public InventoryEntry headSlot;
    public GUIInventorySlot headSlotGUI;
    public InventoryEntry torsoSlot;
    public GUIInventorySlot torsoSlotGUI;
    public InventoryEntry handSlot;
    public GUIInventorySlot handSlotGUI;
    public InventoryEntry legSlot;
    public GUIInventorySlot legSlotGUI;


    [Header("Tool GameObjects")]
    public GameObject axe;
    public GameObject pickaxe;
    public GameObject spear;

    public InventoryEntry tempEquipmentSlot;

    private void Start()
    {
        toolDictionary.Add(PlayerTool.Axe, axe);
        toolDictionary.Add(PlayerTool.Pickaxe, pickaxe);
        toolDictionary.Add(PlayerTool.Spear, spear);

        ResetEquipmentState();
    }

    private void Update()
    {
        //ONLY FOR TESTING, NOT EVEN REMOTELY NICE ON YOUR PC
        UpdateUI();
    }

    public void EquipItem(ResourceTool toolToEquip, InventoryEntry originInventoryEntry)
    {
        ResetEquipmentState();

        switch (toolToEquip.equipmentSlot)
        {


            case PlayerEquipmentSlot.Head:

                if (headSlot.resource != null)
                {
                    tempEquipmentSlot.resource = headSlot.resource;
                    tempEquipmentSlot.quantityHeld = headSlot.quantityHeld;
                    UtilityInventory.ResetInventorySlot(headSlot);
                }

                UtilityInventory.TransferBetweenInventorySlots(originInventoryEntry, headSlot);
                break;

            case PlayerEquipmentSlot.Torso:
                if (torsoSlot.resource != null)
                {
                    tempEquipmentSlot.resource = torsoSlot.resource;
                    tempEquipmentSlot.quantityHeld = torsoSlot.quantityHeld;
                    UtilityInventory.ResetInventorySlot(torsoSlot);
                }
                //torsoSlot.resource = toolToEquip;
                UtilityInventory.TransferBetweenInventorySlots(originInventoryEntry, torsoSlot);
                break;
            case PlayerEquipmentSlot.Hands:
                if (handSlot.resource != null)
                {
                    tempEquipmentSlot.resource = handSlot.resource;
                    tempEquipmentSlot.quantityHeld = handSlot.quantityHeld;
                    UtilityInventory.ResetInventorySlot(handSlot);
                }
                //handSlot.resource = toolToEquip;
                UtilityInventory.TransferBetweenInventorySlots(originInventoryEntry, handSlot);
                break;
            case PlayerEquipmentSlot.Legs:
                if (legSlot.resource != null)
                {
                    tempEquipmentSlot.resource = legSlot.resource;
                    tempEquipmentSlot.quantityHeld = legSlot.quantityHeld;
                    UtilityInventory.ResetInventorySlot(legSlot);
                }
                //legSlot.resource = toolToEquip;
                UtilityInventory.TransferBetweenInventorySlots(originInventoryEntry, legSlot);
                break;
            default:
                break;
        }

        if (tempEquipmentSlot.resource != null)
        {
            UtilityInventory.TransferBetweenInventorySlots(tempEquipmentSlot, originInventoryEntry);
        }


        toolDictionary.TryGetValue(toolToEquip.objectToEquip, out GameObject _tool);

        toolToEquip.isEquipped = true;

        if (toolToEquip.objectToEquip != PlayerTool.None)
            _tool.SetActive(true);


    }

    public void UnequipItem(ResourceTool itemToUnequip)
    {
        itemToUnequip.isEquipped = false;

        switch (itemToUnequip.equipmentSlot)
        {
            case PlayerEquipmentSlot.Head:
                headSlot.resource = null;
                break;
            case PlayerEquipmentSlot.Torso:
                torsoSlot.resource = null;
                break;
            case PlayerEquipmentSlot.Hands:
                handSlot.resource = null;
                break;
            case PlayerEquipmentSlot.Legs:
                legSlot.resource = null;
                break;
            default:
                break;
        }

        ResetEquipmentState();
    }

    void ResetEquipmentState()
    {
        UtilityInventory.ResetInventorySlot(tempEquipmentSlot);

        axe.SetActive(false);
        pickaxe.SetActive(false);
        spear.SetActive(false);
    }

    void UpdateUI()
    {
        if (headSlot.resource != null)
            headSlotGUI.image.sprite = headSlot.resource.icon;
        else
            headSlotGUI.image.sprite = GameManager.instance.blankIcon;

        if (torsoSlot.resource != null)
            torsoSlotGUI.image.sprite = torsoSlot.resource.icon;
        else
            torsoSlotGUI.image.sprite = GameManager.instance.blankIcon;

        if (handSlot.resource != null)
            handSlotGUI.image.sprite = handSlot.resource.icon;
        else
            handSlotGUI.image.sprite = GameManager.instance.blankIcon;

        if (legSlot.resource != null)
            legSlotGUI.image.sprite = legSlot.resource.icon;
        else
            legSlotGUI.image.sprite = GameManager.instance.blankIcon;

    }
}

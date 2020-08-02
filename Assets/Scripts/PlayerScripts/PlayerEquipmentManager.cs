using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    ItemInteractionPanel itemPanel;

    Dictionary<E_ToolType, GameObject> toolDictionary = new Dictionary<E_ToolType, GameObject>();

    public E_ToolType equippedTool;

    [Header("Inventory Elements")]
    public InventoryEntry headSlot;
    public InventoryEntry torsoSlot;
    public InventoryEntry handSlot;
    public InventoryEntry legSlot;


    [Header("Tool GameObjects")]
    public GameObject axe;
    public GameObject pickaxe;
    public GameObject spear;

    private void Start()
    {
        itemPanel = ItemInteractionPanel.instance;

        InitDictionaries();

        ResetEquipmentState();
    }

    void InitDictionaries()
    {
        toolDictionary.Add(E_ToolType.Axe, axe);
        toolDictionary.Add(E_ToolType.Pickaxe, pickaxe);
        toolDictionary.Add(E_ToolType.Spear, spear);
    }


    public void DamageItem(E_EquipmentSlot _slot)
    {
        switch (_slot)
        {
            case E_EquipmentSlot.Head:
                headSlot.resource.currentDurability--;
                break;
            case E_EquipmentSlot.Torso:
                torsoSlot.resource.currentDurability--;
                break;
            case E_EquipmentSlot.Hands:
                handSlot.resource.currentDurability--;
                break;
            case E_EquipmentSlot.Legs:
                legSlot.resource.currentDurability--;
                break;
            default:
                break;
        }
    }

    public bool CheckToolType(E_ToolType _tool)
    {
        if (equippedTool == _tool)
            return true;
        else
            return false;
    }

    public void EquipItem(Resource toolToEquip)
    {
        if (toolToEquip.objectToEquip == equippedTool)
        {
            UnequipItem(toolToEquip);
            return;
        }

        switch (toolToEquip.equipmentSlot)
        {
            case E_EquipmentSlot.Head:
                if (headSlot.resource != null)
                    UnequipItem(headSlot.resource);
                headSlot.resource = toolToEquip;
                break;
            case E_EquipmentSlot.Torso:
                if (torsoSlot.resource != null)
                    UnequipItem(torsoSlot.resource);
                torsoSlot.resource = toolToEquip;
                break;
            case E_EquipmentSlot.Hands:
                if (handSlot.resource != null)
                    UnequipItem(handSlot.resource);
                handSlot.resource = toolToEquip;
                break;
            case E_EquipmentSlot.Legs:
                if (legSlot.resource != null)
                    UnequipItem(legSlot.resource);
                legSlot.resource = toolToEquip;
                break;
            default:
                break;
        }

        equippedTool = toolToEquip.objectToEquip;

        toolDictionary.TryGetValue(toolToEquip.objectToEquip, out GameObject _tool);

        toolToEquip.isEquipped = true;

        if (toolToEquip.objectToEquip != E_ToolType.None)
            _tool.SetActive(true);
    }


    public void UnequipItem(Resource itemToUnequip)
    {
        Debug.Log("Equipment Manager Unequip");

        itemToUnequip.isEquipped = false;

        ResetEquipmentState();

        switch (itemToUnequip.equipmentSlot)
        {
            case E_EquipmentSlot.Head:
                headSlot.resource = null;
                break;
            case E_EquipmentSlot.Torso:
                torsoSlot.resource = null;
                break;
            case E_EquipmentSlot.Hands:
                handSlot.resource = null;
                break;
            case E_EquipmentSlot.Legs:
                legSlot.resource = null;
                break;
            default:
                break;
        }


    }


    void ResetEquipmentState()
    {
        equippedTool = E_ToolType.None;

        axe.SetActive(false);
        pickaxe.SetActive(false);
        spear.SetActive(false);
    }
}


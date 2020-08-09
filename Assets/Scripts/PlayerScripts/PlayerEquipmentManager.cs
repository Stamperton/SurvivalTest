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

    ItemInteractionPanel itemPanel;

    [SerializeField] Transform toolSocket;
    GameObject currentTool;

    public E_ToolType equippedTool;

    [Header("Inventory Elements")]
    public InventoryEntry headSlot;
    public InventoryEntry torsoSlot;
    public InventoryEntry handSlot;
    public InventoryEntry legSlot;


    private void Start()
    {
        itemPanel = ItemInteractionPanel.instance;

        ResetEquipmentState();
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

        toolToEquip.isEquipped = true;

        currentTool = Instantiate(toolToEquip.gameObject);

        currentTool.gameObject.SetActive(true);

        for (int i = 0; i < currentTool.transform.childCount; i++)
            currentTool.transform.GetChild(i).gameObject.SetActive(true);


        currentTool.transform.SetParent(toolSocket);
        currentTool.transform.localPosition = Vector3.zero;
        currentTool.transform.localRotation = Quaternion.identity;

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

        if (currentTool != null)
            Destroy(currentTool.gameObject);

    }
}


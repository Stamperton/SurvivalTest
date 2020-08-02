using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [Header("Basic Resource Info")]
    public string resourceName;
    [TextArea]public string resourceText;
    public Sprite icon;
    public E_ResourceType resourceType;
    public int maxStackSize;
    public int quantityProduced;

    [Header("Usable Variables")]
    public bool isUsable;
    public ItemUseType[] useTypes;

    [Header("Equippable Variables")]
    public bool isEquippable;
    public E_EquipmentSlot equipmentSlot;
    public E_ToolType objectToEquip;
    public int maxDurability;
    public int currentDurability;

    [HideInInspector]
    public bool isEquipped;


    public void Use()
    {
        for (int i = 0; i < useTypes.Length; i++)
        {
            switch (useTypes[i].useType)
            {
                case E_ItemUseType.Health:
                    PlayerStatsManager.instance.AdjustHealth(useTypes[i].useValue);
                    break;
                case E_ItemUseType.Hunger:
                    PlayerStatsManager.instance.AdjustHunger(useTypes[i].useValue);
                    break;
                default:
                    break;
            }
        }

    }
}

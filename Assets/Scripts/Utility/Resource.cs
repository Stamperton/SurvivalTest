using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public string resourceName;
    public Sprite icon;
    public BuildingMaterials resourceType;
    public int maxStackSize;
    public int quantityProduced;

    public bool isUsable;
    public UseType useType;
    public float useValue;

    public bool isEquippable;
    public PlayerTool objectToEquip;


    public void Use()
    {
        switch (useType)
        {
            case UseType.Health:
                PlayerStatsManager.instance.AdjustHealth(useValue);
                break;
            case UseType.Hunger:
                PlayerStatsManager.instance.AdjustHunger(useValue);
                break;
            default:
                break;
        }
    }
}

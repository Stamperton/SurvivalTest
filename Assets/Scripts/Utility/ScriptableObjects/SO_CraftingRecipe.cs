using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewResource", menuName = "Crafting Recipe")]
public class SO_CraftingRecipe : ScriptableObject
{
    public string recipeName;
    public Resource resourceToCraft;
    public ResourceCost[] costOfRecipe;

    public float craftingTime = 0;
}

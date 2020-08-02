using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewResource", menuName = "Planting Recipe")]
public class SO_PlantingRecipe : ScriptableObject
{
    public string recipeName;
    public E_ResourceType requiredSeed;
    public Resource seed;
    public GameObject[] plantGrowStates;
    public Resource plantResource;
    public Vector2 seedAmountRange = new Vector2(1, 1);
    public float growingTime = 0;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewResource", menuName = "Planting Recipe")]
public class SO_PlantingRecipe : ScriptableObject
{
    public string recipeName;
    public BuildingMaterials requiredSeed;
    public GameObject plantToGrow;
    public Resource plantResource;
    public Resource plantSeed;
    public Vector2 seedAmountRange = new Vector2(1,1);
    public float growingTime = 0;
}

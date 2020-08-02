using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GUIInventorySlot : MonoBehaviour
{
    public Image image;
    public Text quantity;
    public GameObject equippedIndicator;
    public Slider durabilitySlider;

    private void Start()
    {
        //image = GetComponentInChildren<Image>();
        //quantity = GetComponentInChildren<Text>();
    }
}

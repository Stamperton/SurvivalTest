using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GUIInventorySlot : MonoBehaviour
{
    public Image image;
    public Text quantity;

    private void Start()
    {
        image = GetComponent<Image>();
        quantity = GetComponentInChildren<Text>();
    }
}

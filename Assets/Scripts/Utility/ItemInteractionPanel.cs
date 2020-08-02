using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInteractionPanel : MonoBehaviour
{
    #region Singleton
    public static ItemInteractionPanel instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Debug.Log("Multiple ItemInteractionPanels in Scene");
            Destroy(gameObject);
        }
    }
    #endregion

    [SerializeField] GameObject UIPanel;

    [SerializeField] Image itemIcon;
    [SerializeField] Text itemTitle;
    [SerializeField] Text textBox;
    [SerializeField] Text useText;

    [SerializeField] Button useButton;
    [SerializeField] Button equipButton;
    [SerializeField] Text equipText;
    [SerializeField] Button dropButton;


    // Start is called before the first frame update
    void Start()
    {
        equipText = equipButton.GetComponentInChildren<Text>();

        ResetInteractionVariables();
        UIPanel.SetActive(false);
    }

    public void PopulateItemPanel(InventoryEntry _entry)
    {
        if (_entry.resource == null)
            return;

        UIPanel.SetActive(true);

        itemIcon.sprite = _entry.resource.icon;
        itemTitle.text = _entry.resource.resourceName;
        textBox.text = _entry.resource.resourceText;
        useText.text = null;

        if (_entry.resource.isEquippable)
        {
            equipButton.interactable = true;
            useText.text = ("Durability : " + _entry.resource.currentDurability + " / " + _entry.resource.maxDurability);
        }

        if (_entry.resource.isEquipped)
            equipText.text = "Unequip";
        else
            equipText.text = "Equip";

        if (_entry.resource.isUsable)
        {
            useButton.interactable = true;

            foreach (ItemUseType useType in _entry.resource.useTypes)
            {
                switch (useType.useType)
                {
                    case E_ItemUseType.Health:
                        if (useType.useValue > 0)
                            useText.text += ("Heals " + useType.useValue + " HP\n");
                        else
                            useText.text += ("Harms " + useType.useValue + " HP\n");
                        break;
                    case E_ItemUseType.Hunger:
                        if (useType.useValue > 0)
                            useText.text += ("Replenishes " + useType.useValue + " Hunger\n");
                        else
                            useText.text += ("Causes " + useType.useValue + " Hunger\n");
                        break;
                    default:
                        break;
                }

            }
        }
    }


    public void ResetInteractionVariables()
    {
        itemIcon.sprite = GameManager.instance.blankIcon;
        itemTitle.text = null;
        textBox.text = null;
        useText.text = null;

        useButton.interactable = false;
        equipButton.interactable = false;
    }

    public void CloseItemPanel()
    {
        ResetInteractionVariables();
        UIPanel.SetActive(false);
    }
}

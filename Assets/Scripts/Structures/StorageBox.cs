using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageBox : StorageBase, IInteractable
{
    Animator anim;

    public Text storageTitleText;

    public bool isLocked = false;
    public Resource keyType;
    [SerializeField] bool consumesKey = false;

    [SerializeField] string storageName;

    private void Start()
    {
        if (storageTitleText != null)
            storageTitleText.text = storageName;

        anim = GetComponent<Animator>();

        inventoryEntriesGUI = GetComponentsInChildren<GUIInventorySlot>();

        //Initialise Canvas
        UpdateUI();

        canvasGUI.gameObject.SetActive(false);
    }

    //TESTING PURPOSES ONLY DO NOT KEEP
    private void Update()
    {
        UpdateUI();
    }

    //Toggles Canvas and Game Mode.
    public override void Interact()
    {
        if (isLocked)
        {
            if (UtilityInventory.CheckForSameUseInventorySpace(PlayerInventory.instance.inventoryEntries, keyType, out InventoryEntry _entry) == false)
                return;
            if (consumesKey)
                UtilityInventory.DecrementInventorySlot(_entry);
            isLocked = false;
        }



        base.Interact();
        anim.SetBool("isOpen", true);
    }

    //Closes Box. Duh
    public override void CloseStorage()
    {
        base.CloseStorage();
        anim.SetBool("isOpen", false);
    }

}

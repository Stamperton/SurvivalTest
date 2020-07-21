using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageBox : StorageBase, IInteractable
{
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        //boxCanvas = GetComponentInChildren<Canvas>();

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

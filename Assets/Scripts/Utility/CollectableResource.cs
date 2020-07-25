using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableResource : SubjectBase, IInteractable
{
    public PlayerTool requiredTool;

    TextWithImage textToSend = new TextWithImage();

    public Resource resource;
    public Vector2 quantityRange = new Vector2(1, 1);
    public Vector2 collectionRange = new Vector2(1, 1);

    [HideInInspector] public int quantityHeld;
    [HideInInspector] public int collectionAmount;

    private void Start()
    {
        if (resource == null)
            resource = GetComponent<Resource>();

        quantityHeld = Random.Range((int)quantityRange.x, (int)quantityRange.y);
    }

    public virtual void Interact()
    {
        if (requiredTool != PlayerTool.None)
            if (PlayerEquipmentManager.instance.handSlot.resource == null || PlayerEquipmentManager.instance.handSlot.resource.objectToEquip != requiredTool)
                return;

        collectionAmount = Random.Range((int)collectionRange.x, (int)collectionRange.y);
        if (collectionAmount > quantityHeld)
            collectionAmount = quantityHeld;

        for (int i = 0; i < collectionAmount; i++)
        {
            if (PlayerInventory.instance.CollectSomething(resource))
            {
                quantityHeld--;
            }
        }

        textToSend.text = resource.resourceName;
        if (collectionAmount > 1)
        {
            textToSend.text += " x " + collectionAmount;
        }

        Notify(textToSend);

        if (quantityHeld <= 0)
        {
            this.gameObject.SetActive(false);
            return;
        }
    }
}

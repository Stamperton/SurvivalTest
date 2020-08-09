using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableResource : SubjectBase, IInteractable
{
    public E_ToolType requiredTool;

    TextWithImage textToSend = new TextWithImage();

    public Resource resource;
    public Vector2Int quantityRange = new Vector2Int(1, 1);
    public Vector2Int collectionRange = new Vector2Int(1, 1);

    [HideInInspector] public int quantityHeld;
    [HideInInspector] public int collectionAmount;

    private void Start()
    {
        if (resource == null)
            resource = GetComponentInParent<Resource>();

        quantityHeld = Random.Range(quantityRange.x, quantityRange.y);

        AddObserver(FindObjectOfType<MessageManager>());
    }

    public virtual void Interact()
    {
        if (requiredTool != E_ToolType.None)
            if (!PlayerEquipmentManager.instance.CheckToolType(requiredTool))
                return;


        collectionAmount = Random.Range(collectionRange.x, collectionRange.y);
        if (collectionAmount > quantityHeld)
            collectionAmount = quantityHeld;

        for (int i = 0; i < collectionAmount; i++)
        {
            if (PlayerInventory.instance.CollectSomething(resource))
            {
                quantityHeld--;

                if (requiredTool != E_ToolType.None)
                {
                    PlayerEquipmentManager.instance.DamageItem(E_EquipmentSlot.Hands);
                }
            }
        }

        textToSend.text = resource.resourceName;

        if (collectionAmount > 1)
        {
            textToSend.text += " x " + collectionAmount;
        }

        Notify(textToSend);
        Debug.Log("textToSend");

        GrowingPlot _plot = GetComponentInParent<GrowingPlot>();
        if (_plot != null)
        {
            _plot.OnHarvestPlant();
        }

        if (quantityHeld <= 0)
        {
            this.gameObject.SetActive(false);
            return;
        }
    }
}

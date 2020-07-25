using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairTooltip : MonoBehaviour
{
    public string toolTipText;
    string originalText;

    CollectableResource _collectableResource;

    private void Start()
    {
        _collectableResource = GetComponent<CollectableResource>();

        originalText = toolTipText;

        UpdateText();
    }

    public void UpdateText()
    {
        toolTipText = originalText;

        if (toolTipText == "")
        {
            toolTipText = GetComponent<Resource>().resourceName;
        }

        if (_collectableResource != null)
        {
            switch (_collectableResource.requiredTool)
            {
                case PlayerTool.None:
                    break;
                case PlayerTool.Pickaxe:
                    if (PlayerEquipmentManager.instance.currentEquipment != PlayerTool.Pickaxe)
                    {
                        toolTipText += "\n(Requires Pickaxe)";
                    }
                    break;
                case PlayerTool.Axe:
                    if (PlayerEquipmentManager.instance.currentEquipment != PlayerTool.Axe)
                        toolTipText += "\n(Requires Axe)";
                    break;
                case PlayerTool.Spear:
                    break;
                case PlayerTool.Crowbar:
                    if (PlayerEquipmentManager.instance.currentEquipment != PlayerTool.Crowbar)
                        toolTipText += "\n(Requires Crowbar)";
                    break;
                case PlayerTool.Gun:
                    break;
                default:
                    break;
            }
        }
    }
}

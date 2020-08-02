using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairTooltip : MonoBehaviour
{
    [TextArea]
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
                case E_ToolType.None:
                    break;
                case E_ToolType.Pickaxe:
                    if (PlayerEquipmentManager.instance.equippedTool != E_ToolType.Pickaxe)
                    {
                        toolTipText += "\n(Requires Pickaxe)";
                    }
                    break;
                case E_ToolType.Axe:
                    if (PlayerEquipmentManager.instance.equippedTool != E_ToolType.Axe)
                        toolTipText += "\n(Requires Axe)";
                    break;
                case E_ToolType.Spear:
                    break;
                case E_ToolType.Crowbar:
                    if (PlayerEquipmentManager.instance.equippedTool != E_ToolType.Crowbar)
                        toolTipText += "\n(Requires Crowbar)";
                    break;
                case E_ToolType.Gun:
                    break;
                default:
                    break;
            }
        }
    }
}

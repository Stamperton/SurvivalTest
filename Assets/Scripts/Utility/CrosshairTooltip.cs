using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairTooltip : MonoBehaviour
{
    public string toolTipText;

    private void Start()
    {
        if (toolTipText == "")
        {
            toolTipText = GetComponent<Resource>().resourceName;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    public string headline;
    public string description;
    public string otherInfos;

    private void OnMouseEnter()
    {
        TooltipManager._instance.SetAndShowTooltip(headline, description,otherInfos);
    }

    private void OnMouseExit()
    {
        TooltipManager._instance.HideTooltip();
    }
}

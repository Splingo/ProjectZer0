using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartFightButton : MonoBehaviour
{
    public TextMeshProUGUI buttonText;

    public void ToggleText()
    {
        if (buttonText.text == "Start Fight")
        {
            buttonText.text = "Ongoing Wave";
        }
        else
        {
            buttonText.text = "Start Fight";
        }
    }
}

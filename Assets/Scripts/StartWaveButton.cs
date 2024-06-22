using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StartWaveButton : MonoBehaviour
{
    public TextMeshProUGUI buttonText;
    public Button startWaveButton;

    public void setButtonText(string text)
    {
       buttonText.text = text;
    }
}

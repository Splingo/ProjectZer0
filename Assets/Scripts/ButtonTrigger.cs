using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonTrigger : MonoBehaviour
{

    public GameObject panel;
    public void TogglePanel()
    {
        panel.SetActive(!panel.activeSelf);
    }    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

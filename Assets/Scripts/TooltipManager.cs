using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.VersionControl;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager _instance;

    public TextMeshProUGUI tooltip;

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition;
    }

    public void SetAndShowTooltip(string message)
    {
        gameObject.SetActive(true);
        tooltip.text = message;
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
        tooltip.text = string.Empty;
    }
}

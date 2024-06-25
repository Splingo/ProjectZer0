using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void SetAndShowTooltip(string headline, string description, string otherInfos)
    {
        gameObject.SetActive(true);
        tooltip.text = headline + "\n" + description + "\n" + otherInfos;
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
        tooltip.text = string.Empty;
    }
}

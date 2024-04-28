using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
[System.Serializable]

public class Score : MonoBehaviour
{
    public TextMeshProUGUI EnemiesKilled;
    // Start is called before the first frame update
    void Start()
    {
        EnemiesKilled.text = PlayerPrefs.GetInt("EnemiesKilled", 0).ToString();
    }
}


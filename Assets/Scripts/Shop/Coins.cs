using System;
using TMPro;
using UnityEngine;

public class Coins : MonoBehaviour
{
    [SerializeField] internal int coins = 0;
    [SerializeField] private TextMeshProUGUI text;
    private void Awake()
    {
        
        if (PlayerPrefs.HasKey("coin"))
            coins = PlayerPrefs.GetInt("coins");
        else
            PlayerPrefs.SetInt("coin", coins);
    }

    private void Update()
    {
        text.text = $"{coins}";
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("coins", coins);
    }
}

using System;
using TMPro;
using UnityEngine;

public class DeathUIControl : MonoBehaviour
{
    public static DeathUIControl Instance { get; set; }
    
    [SerializeField]
    private TextMeshProUGUI deathText;
    [SerializeField]
    private string baseText;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        
        Instance = this;
        
        HandleUpdateUI(0);
    }

    public void HandleUpdateUI(int deathCounter)
    {
        deathText.text = deathCounter.ToString() + baseText;
    }
}

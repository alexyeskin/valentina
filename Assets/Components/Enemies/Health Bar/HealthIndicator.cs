using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthIndicator : MonoBehaviour
{
    [SerializeField]
    private Slider healthBar;
    
    [SerializeField]
    private TextMeshPro healthPoints;
    
    private Stats stats;
    
    private void Awake() {
        stats = GetComponentInParent<Stats>();
        stats.HealthChanged += OnHealthChanged;
    }
    
    private void OnDestroy() {
        stats.HealthChanged -= OnHealthChanged;
    }
    
    private void Start() {
        healthBar.maxValue = stats.maxHealth;
        healthBar.value = stats.maxHealth;
    }
    
    void Update()
    {
        
    }
    
    private void OnHealthChanged(int currentHealth) {
        healthBar.value = currentHealth;
        healthPoints.text = currentHealth.ToString();
    }
}

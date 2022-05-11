using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Nothing {}

public class Stats: MonoBehaviour {
    public int maxHealth = 100;
    public int damage = 10;
    
    private int previousCurrentHealth = 0;
    private int _currentHealth;
    public int currentHealth {
        get { return _currentHealth; }
        set {
            previousCurrentHealth = currentHealth;
            _currentHealth = Math.Min(value, maxHealth);
            
            HealthChanged.Invoke(currentHealth);
            if (isDead) {
                Death.Invoke(new Nothing());
            }

            if (previousCurrentHealth > currentHealth) {
                if (HealthDecreased != null) {
                    HealthDecreased.Invoke(previousCurrentHealth - currentHealth);
                }
            }
        }
    }
    
    public bool isDead {
        get { return currentHealth <= 0; }
    }

    public event Action<int> HealthChanged;
    public event Action<int> HealthDecreased;
    public event Action<Nothing> Death;
    public event Action<Nothing> Respawning;
    
    HealthRegeneration HealthRegeneration;
    
    private void Awake() {
        HealthRegeneration = GetComponent<HealthRegeneration>();
    }

    void Start() {
        currentHealth = maxHealth;
    }

    public void takeDamage(int amount) {
        currentHealth -= amount;
    }

    public void heal(int amount) {
        currentHealth += amount;
    }

    public void restoreFullHealth() {
        currentHealth = maxHealth;
    }
    
    public void Respawn() {
        restoreFullHealth();
        Respawning.Invoke(new Nothing());
    }

    public void setCurrentHealth(float amount) {
        currentHealth = (int)Math.Floor(amount);
    }
}

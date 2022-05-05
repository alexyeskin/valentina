using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour {
    [Header("Health")]
    public int maxHealth = 100;

    [Header("Damage")]
    public int damage = 10;
    
    private int _currentHealth;
    public int currentHealth {
        get { return _currentHealth; }
        set {
            _currentHealth = value;

            HealthChanged.Invoke(value);

            if (isDead) {
                die();
            }
        }
    }
    
    public bool isDead {
        get { return currentHealth <= 0; }
    }

    public event Action<int> HealthChanged;

    void Start() {
        _currentHealth = maxHealth;
    }

    public bool takeDamage(int amount) {
        currentHealth -= amount;
        
        if (isDead) {
            return true;
        } else {
            return false;
        }
    }

    public void heal(int amount) {
        currentHealth += amount;
    }

    public void restoreFullHealth() {
        currentHealth = maxHealth;
    }

    public void setCurrentHealth(float amount) {
        currentHealth = (int)Math.Floor(amount);
    }

    private void die() {
        // To fix - Called twice
        Debug.Log("Die");
        foreach (Transform child in transform) {
            if (child.gameObject.name != "Animations") {
                Destroy(child.gameObject);
            }
        }

        Destroy(gameObject, 5f);
    }
}

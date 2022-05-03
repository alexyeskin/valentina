using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats: MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    
    private int currentHealth;
    
    public event Action<int> HealthChanged;
    
    AnimationManager animationManager;
    
    private void Awake() {
        animationManager = GetComponent<AnimationManager>();
    }
    
    void Start()
    {
        currentHealth = maxHealth;
    }
    
    public bool takeDamage(int amount) {
        currentHealth -= amount;
        HealthChanged.Invoke(currentHealth);
        
        if (currentHealth <= 0) {
            die();
            return true;
        } else {
            return false;
        }
    }
    
    public void heal(int amount) {
        currentHealth += amount;
        HealthChanged.Invoke(currentHealth);
    }
    
    public void restoreFullHealth() {
        currentHealth = maxHealth;
        
        // Todo make get, set and put it in set
        HealthChanged.Invoke(currentHealth);
    }
    
    public void setCurrentHealth(float amount) {
        // Todo also remove this shit
        currentHealth = (int)Math.Floor(amount);
        HealthChanged.Invoke(currentHealth);
        
        if (currentHealth <= 0) {
            die();
        }
    }
    
    private void die() {
        // To fix - Called twice
        animationManager.playDeathEffect();
        
        foreach (Transform child in transform)
        {
            if (child.gameObject.name != "DeathEffect") {
                Destroy(child.gameObject);
            }
        }
        
        Chasing chasing = gameObject.GetComponent<Chasing>();
        chasing.stopChasing();
        
        Destroy(gameObject, 3f);
    }
}

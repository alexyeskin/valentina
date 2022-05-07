using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRegeneration: MonoBehaviour {
    Stats stats;
    AnimationManager AnimationManager;
    
    private void Awake() {
        stats = GetComponentInParent<Stats>();
        AnimationManager = GetComponent<AnimationManager>();
        stats.HealthDecreased += OnHealthDecreased;
        stats.HealthChanged += OnHealthChanged;
    }
    
    private void OnDestroy() {
        stats.HealthDecreased -= OnHealthDecreased;
        stats.HealthChanged += OnHealthChanged;
    }
    
    private IEnumerator RestoreCounter;
    IEnumerator restoreCounter() {
        yield return new WaitForSeconds(3);
        startRestoring();
    }
    
    private IEnumerator RestoringHealth;
    IEnumerator restoringHealth() {
        while(true) {
            autorestoreHealth();
            yield return new WaitForSeconds(1);
        }
    }
    
    private void OnHealthDecreased(int amount) {
        if (stats.currentHealth < stats.maxHealth) {
            if (RestoreCounter != null) {
                StopCoroutine(RestoreCounter);
            }
            StopRestoring();
            
            RestoreCounter = restoreCounter();
            StartCoroutine(RestoreCounter);
        }
    }
    
    private void OnHealthChanged(int currentHealth) {
        if (currentHealth == stats.maxHealth) {
            StopRestoring();
        }
    }
    
    private void startRestoring() {
        AnimationManager.playHealEffect();
        RestoringHealth = restoringHealth();
        StartCoroutine(RestoringHealth);
    }

    private void StopRestoring() {
        if (RestoringHealth != null) {
            StopCoroutine(RestoringHealth);
            AnimationManager.stopHealEffect();
        }
    }
    
    private void autorestoreHealth() {
        // After 3 sec 13%
        float healthToRestore = stats.maxHealth * 0.13f;
        int roundedHealthAmount = (int)Math.Floor(healthToRestore);
        stats.heal(roundedHealthAmount);
    }
}
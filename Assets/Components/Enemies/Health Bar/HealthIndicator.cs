using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthIndicator : MonoBehaviour {
    [SerializeField]
    private Slider healthBar;

    [SerializeField]
    private Slider animatedHealthBar;

    [SerializeField]
    private TextMeshPro healthPoints;

    private Stats stats;
    private Animator animator;
    private float lerpTimer;
    private float chipSpeed = 2f;

    private void Awake() {
        stats = GetComponentInParent<Stats>();
        animator = GetComponent<Animator>();
        stats.HealthChanged += OnHealthChanged;
    }

    private void OnDestroy() {
        stats.HealthChanged -= OnHealthChanged;
    }

    private void Start() {
        healthBar.maxValue = stats.maxHealth;
        healthBar.value = stats.maxHealth;

        animatedHealthBar.maxValue = stats.maxHealth;
        animatedHealthBar.value = stats.maxHealth;

        healthPoints.text = stats.maxHealth.ToString();
    }

    void Update() {
        lerpTimer += Time.deltaTime;
        float percentageComplete = lerpTimer / chipSpeed;
        percentageComplete *= percentageComplete;
        animatedHealthBar.value = Mathf.Lerp(
            animatedHealthBar.value,
            healthBar.value,
            percentageComplete
        );
    }

    private void OnHealthChanged(int currentHealth) {
        if (healthBar.value > currentHealth) {
            animator.SetTrigger("damaged");
        }

        healthBar.value = currentHealth;
        healthPoints.text = currentHealth.ToString();

        lerpTimer = 0f;
    }
}

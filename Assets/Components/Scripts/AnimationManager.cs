using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Create this class as singleton and use it not as MonoBehaviour
// Or move it to parent
public class AnimationManager : MonoBehaviour
{
    Animator animator;
    GeneralMovement movement;
    
    [Header("Floating Text")]
    public GameObject floatingTextPrefub;
    
    [Header("Death Effect")]
    public GameObject deathEffectPrefub;
    
    private void Awake() {
        // one animator in all children??????
        animator = GetComponentInParent<Animator>();
        movement = GetComponentInChildren<GeneralMovement>();
    }
    
    public void playAttackAnimation() {
        animator.SetTrigger("attack");
    }
    
    public void playDeathEffect() {
        Instantiate(deathEffectPrefub, transform.position, Quaternion.identity, transform);
    }
    
    public void showFloatingDamageText(int damage) {
        // Create class for enemy and charachter animations
        // or compate with tags (bad solution)
        
        if (floatingTextPrefub) {
            if (floatingTextPrefub.TryGetComponent(out FloatingText floatingText)) {
                floatingText.text = damage.ToString();
            }
            
            Instantiate(floatingTextPrefub, transform.position, Quaternion.identity, transform);
        }
    }
}

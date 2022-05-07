using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Create this class as singleton and use it not as MonoBehaviour
// Or move it to parent
public class AnimationManager : MonoBehaviour {
    Animator animator;
    Stats stats;

    public GameObject floatingTextPrefub;
    public GameObject deathEffectPrefub;
    public GameObject healEffectPrefub;

    private void Awake() {
        animator = GetComponent<Animator>();
        stats = GetComponent<Stats>();
        stats.HealthDecreased += onHealthDecreased;
    }
    
    private void OnDestroy() {
        stats.HealthDecreased -= onHealthDecreased;
    }

    private void Start() {
        if (!floatingTextPrefub) {
            Debug.Log("Set floatingTextPrefub");
        }
    }
    
    IEnumerator test() {
        yield return new WaitForSeconds(3);
        showFloatingDamageText(1000);
    }
    
    private void onHealthDecreased(int amount) {
        showFloatingDamageText(amount);
        
        if (stats.isDead) {
            foreach (Transform child in transform) {
                child.gameObject.SetActive(false);
            }
            playDeathEffect();
            StartCoroutine(test());
        }
    }

    public void playAttackAnimation() {
        // 0.83 animation duration
        animator.SetTrigger("attack");
    }

    public void playDeathEffect() {
        Instantiate(deathEffectPrefub, transform.position, Quaternion.identity, transform);
    }
    
    public void playHealEffect() {
        Instantiate(healEffectPrefub, transform.position, Quaternion.identity, transform);
    }
    
    IEnumerator playAnimationWithDelay() {
        yield return new WaitForSeconds(1);
        foreach (Transform child in transform) {
            if (child.name == "FX Healing AOE(Clone)") {
                Destroy(child.gameObject);
            }
        }
    }
    
    public void stopHealEffect() {
        StartCoroutine(playAnimationWithDelay());
    }

    public void showFloatingDamageText(int damage) {
        // Create class for enemy and charachter animations
        if (floatingTextPrefub.TryGetComponent(out FloatingText floatingText)) {
            floatingText.text = damage.ToString();

            if (!gameObject.CompareTag("Enemy")) {
                floatingText.textColor = new Color32(224, 60, 31, 255);
            } else {
                floatingText.textColor = new Color32(255, 255, 255, 255);
            }
        }

        Instantiate(floatingTextPrefub, transform.position, Quaternion.identity, transform);
    }
}

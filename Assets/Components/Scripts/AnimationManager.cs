using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Create this class as singleton and use it not as MonoBehaviour
// Or move it to parent
public class AnimationManager : MonoBehaviour {
    Animator animator;
    Stats stats;
    
    public GameObject spawningEffectPrefub;
    public GameObject deathEffectPrefub;
    public GameObject healEffectPrefub;

    private void Awake() {
        animator = GetComponentInParent<Animator>();
        stats = GetComponentInParent<Stats>();
        stats.Death += onDeath;
        stats.Respawning += onRespawn;
    }
    
    private void OnDestroy() {
        stats.Death -= onDeath;
        stats.Respawning -= onRespawn;
    }
    
    private void onRespawn(Nothing nothing) {
        playSpawnEffect();
    }

    private void onDeath(Nothing nothing) {
        playDeathEffect();

        foreach (Transform child in stats.transform) {
            if (child.CompareTag("UI") || child.CompareTag("Model"))
                child.gameObject.SetActive(false);
        }
    }

    public void playAttackAnimation() {
        // 0.83 animation duration
        animator.SetTrigger("attack");
    }
    
    public void playSpawnEffect() {
        var position = transform.position;
        position.y += 2;
        Instantiate(spawningEffectPrefub, position, Quaternion.identity, transform);
        
        
        
        // refactor
        // autodestroyed objects
        foreach (Transform child in transform) {
            if (child.name == "FX Spawn(Clone)") {
                Destroy(child.gameObject, 3f);
            }
        }
    }

    public void playDeathEffect() {
        Instantiate(deathEffectPrefub, transform.position, Quaternion.identity, transform);
        
        // refactor
        // autodestroyed objects
        foreach (Transform child in transform) {
            if (child.name == "FX Death(Clone)") {
                Destroy(child.gameObject, 3f);
            }
        }
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
}

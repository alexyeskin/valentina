using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour {
    Stats attackerStats;
    AnimationManager animationManager;
    GeneralMovement movement;

    [SerializeField]
    Joystick joystick;

    List<GameObject> targets = new List<GameObject>();

    bool attackHaveTapped {
        get {
            if (joystick) {
                return joystick.Direction != Vector2.zero;
            } else {
                return false;
            }
        }
    }
    bool canAttack = true;

    private void Awake() {
        attackerStats = gameObject.GetComponentInParent<Stats>();
        attackerStats.HealthChanged += OnAttackerHealthChanged;
        animationManager = GetComponent<AnimationManager>();
        movement = GetComponentInChildren<GeneralMovement>();
    }
    
    private void OnDestroy() {
        attackerStats.HealthChanged -= OnAttackerHealthChanged;
    }

    void Update() {
        if (attackHaveTapped) {
            if (canAttack) {
                StartCoroutine(attack());
            }
        }

        if (gameObject.CompareTag("Enemy")) {
            if (canAttack) {
                if (targets.Count != 0) {
                    StartCoroutine(attack());
                }
            }
        }
    }

    IEnumerator attack() {
        canAttack = false;
        // 0.83 animation duration
        // need to speed up animation if attack cooldown less
        float animationWaitDuration = 0.75f / 1.5f;
        animationManager.playAttackAnimation();
        yield return new WaitForSeconds(animationWaitDuration);
        
        if (targets.Count != 0) {
            GameObject target = findTheClosiestTarget();
            attack(target);
        }
        
        yield return new WaitForSeconds(0.7f - animationWaitDuration);
        canAttack = true;
    }

    void attack(GameObject target) {
        Stats targetStats = target.GetComponentInParent<Stats>();
        
        targetStats.takeDamage(attackerStats.damage);
        
        if (targetStats.isDead) {
            removeTarget(target);
            
            RandomMovement movement = GetComponentInChildren<RandomMovement>();
            if (movement) {
                movement.returnToSpawn();
            }
        }
    }
    
    private void OnAttackerHealthChanged(int currentHealth) {
        if (attackerStats.isDead) {
            removeAllTargets();
        }
    }

    GameObject findTheClosiestTarget() {
        GameObject theClosiestTarget = targets[0];
        float theSmallestDistance = float.MaxValue;

        foreach (var target in targets) {
            float distance = Vector3.Distance(target.transform.position, transform.position);

            if (distance < theSmallestDistance) {
                theSmallestDistance = distance;
                theClosiestTarget = target;
            }
        }

        return theClosiestTarget;
    }

    public void addTarget(GameObject target) {
        targets.Add(target);
    }

    public void removeTarget(GameObject target) {
        targets.Remove(target);
    }
    
    public void removeAllTargets() {
        targets = new List<GameObject>();
    }
}

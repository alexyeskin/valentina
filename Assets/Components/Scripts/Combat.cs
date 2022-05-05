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

    IEnumerator attackCooldown() {
        canAttack = false;
        yield return new WaitForSeconds(0.7f);
        canAttack = true;
    }

    private void Awake() {
        attackerStats = gameObject.GetComponent<Stats>();
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
                if (targets.Count != 0) {
                    GameObject target = findTheClosiestTarget();
                    attack(target);
                }

                // after attack start cooldown and play attack animation
                StartCoroutine(attackCooldown());
                animationManager.playAttackAnimation();
            }
        }

        if (gameObject.CompareTag("Enemy")) {
            if (canAttack) {
                if (targets.Count != 0) {
                    GameObject target = findTheClosiestTarget();
                    attack(target);
                    
                    // play animation only if attack possible
                    StartCoroutine(attackCooldown());
                    animationManager.playAttackAnimation();
                }
            }
        }
    }

    void attack(GameObject target) {
        Stats targetStats = target.GetComponent<Stats>();
        AnimationManager targetAnimationManager = target.GetComponent<AnimationManager>();
        
        targetStats.takeDamage(attackerStats.damage);
        targetAnimationManager.showFloatingDamageText(attackerStats.damage);
        
        if (targetStats.isDead) {
            removeTarget(target);
            
            targetAnimationManager.playDeathEffect();
            
            RandomMovement movement = GetComponentInChildren<RandomMovement>();
            if (movement) {
                movement.returnToSpawn();
            }
        }
    }
    
    private void OnAttackerHealthChanged(int currentHealth) {
        if (attackerStats.isDead) {
            removeAllTargets();
        } else {
            print(currentHealth);
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

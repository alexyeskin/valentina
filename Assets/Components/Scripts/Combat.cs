using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour {
    Stats stats;
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
        stats = gameObject.GetComponent<Stats>();
        animationManager = GetComponent<AnimationManager>();
        movement = GetComponentInChildren<GeneralMovement>();
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
                    
                    StartCoroutine(attackCooldown());
                    animationManager.playAttackAnimation();
                }
            }
        }
    }

    void attack(GameObject target) {
        Stats targetStats = target.GetComponent<Stats>();
        AnimationManager targetAnimationManager = target.GetComponent<AnimationManager>();

        // Get is Dead
        // DEAL WITH COMPLEX DEATH LOGIC
        var isDead = stats.currentHealth <= 0;
        var isTargetDead = false;
        
        // subscribe for current health when target is added, and unsub when is removed;
        if (!isDead) {
            targetAnimationManager.showFloatingDamageText(stats.damage);
            // Todo Remove bool return, subscribe for event
            isTargetDead = targetStats.takeDamage(stats.damage);
            
            if (isTargetDead) {
                targetAnimationManager.playDeathEffect();
            }
        }

        if (isDead || isTargetDead) {
            targets.Remove(target);

            RandomMovement movement = GetComponentInChildren<RandomMovement>();

            if (movement != null) {
                movement.returnToSpawn();
            }
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
}

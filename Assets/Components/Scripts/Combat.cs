using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    AnimationManager animationManager;
    GeneralMovement movement;
    
    [SerializeField]
    Joystick joystick;

    List<GameObject> targets = new List<GameObject>();
    
    bool attackHaveTapped {
        get {
            return joystick.Direction != Vector2.zero;
        }
    }
    bool canAttack = true;

    IEnumerator attackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(0.7f);
        canAttack = true;
    }
    
    private void Awake() {
        animationManager = GetComponent<AnimationManager>();
        movement = GetComponent<GeneralMovement>();
    }

    void Update()
    {
        if (attackHaveTapped) {
            if (canAttack) {
                StartCoroutine(attackCooldown());
                animationManager.playAttackAnimation();
                
                if (targets.Count != 0) {
                    GameObject target = findTheClosiestTarget();
                    attack(target);
                }
            }
        }
    }
    
    void attack(GameObject target) {
        // Todo Get non-target damage from stats
        var damageAmount = Random.Range(1, 25);
        
        // Todo Переместить всю логику с целью в саму цель, зачем тут работать с целью
        AnimationManager animationManager = target.GetComponent<AnimationManager>();
        animationManager.showFloatingDamageText(damageAmount);
        
        Stats targetStats = target.GetComponent<Stats>();
        
        // Todo Remove bool return, subscribe for event
        var isDead = targetStats.takeDamage(damageAmount);
        
        if (isDead) {
            targets.Remove(target);
        }
    }

    GameObject findTheClosiestTarget()
    {
        GameObject theClosiestTarget = targets[0];
        float theSmallestDistance = float.MaxValue;
        
        foreach (var target in targets)
        {
            float distance = Vector3.Distance(target.transform.position, transform.position);
            
            if (distance < theSmallestDistance) {
                theSmallestDistance = distance;
                theClosiestTarget = target;
            }
        }
        
        return theClosiestTarget;
    }
    
    public void addTarget(GameObject target)
    {
        targets.Add(target);
    }

    public void removeTarget(GameObject target)
    {
        targets.Remove(target);
    }
}

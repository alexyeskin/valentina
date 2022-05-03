using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chasing : MonoBehaviour
{
    GeneralMovement movement;
    Transform target;
    
    private bool isChasing = false;
    public float chasingMovementSpeed = 5f;
    
    void Start()
    {
        movement = GetComponent<GeneralMovement>();
    }
    
    void FixedUpdate()
    {
        if (isChasing) {
            followTarget();
        }
        
    }
    
    public void startChasing(Transform target) {
        this.target = target;
        isChasing = true;
        movement.currentMovementSpeed = chasingMovementSpeed;
    }
    
    public void stopChasing() {
        isChasing = false;
    }
    
    void followTarget() {
        float distance = Vector3.Distance(transform.position, target.position);

        // Point have reached
        if (distance < 3.5f) {
            movement.currentMovement = Vector3.zero;
        } else {
            movement.currentMovement = movement.pointToMoveFrom(target.position);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chasing : MonoBehaviour {
    GeneralMovement movement;
    Transform target;
    
    private bool isChasing = false;
    public float chasingMovementSpeed = 5f;

    private void Awake() {
        movement = GetComponent<GeneralMovement>();
    }

    void Start() { }

    void Update() {
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
        movement.currentMovementSpeed = movement.walkMovementSpeed;
        movement.AttackPositionToLookAt = Vector3.zero;
    }

    void followTarget() {
        float distance = Vector3.Distance(transform.position, target.position);

        // Point have reached
        if (distance < 3.5f) {
            movement.currentMovement = Vector3.zero;
        } else {
            movement.currentMovement = movement.pointToMoveFrom(target.position).normalized;
        }
        
        Vector3 newVector = target.transform.position - transform.position;
        movement.AttackPositionToLookAt = newVector;
    }
}

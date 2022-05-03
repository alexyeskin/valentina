using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    GeneralMovement movement;
    Chasing chasing;
    Stats stats;

    public float circleMovingRadius = 5f;
    public float rotationFactorPerFrame = 1f;

    Vector3 startCenterPosition;
    Vector3 randomPoint;
    
    bool isPatrolling = true;

    IEnumerator waiter()
    {
        movement.currentMovement = Vector3.zero;
        movement.currentMovementSpeed = movement.walkMovementSpeed;
        yield return new WaitForSeconds(Random.Range(0.5f, 4f));
        movement.currentMovement = movement.pointToMoveFrom(randomPoint);
    }
    
    private void Awake() {
        movement = GetComponent<GeneralMovement>();
        chasing = GetComponent<Chasing>();
        stats = GetComponent<Stats>();
    }

    void Start()
    {
        startCenterPosition = transform.position;
        randomPoint = randomPointInsideCircle();
        
        StartCoroutine(waiter());
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(startCenterPosition, circleMovingRadius);
    }

    void FixedUpdate()
    {
        if (isPatrolling) {
            patrol();
        } else {
            handleReturning();
        }
    }

    void patrol() {
        float distance = Vector3.Distance(transform.position, randomPoint);

        // Point have reached
        if (distance < 0.05f * movement.currentMovement.magnitude) {
            randomPoint = randomPointInsideCircle();
            StartCoroutine(waiter());
        }
    }
    
    void handleReturning() {
        float distance = Vector3.Distance(transform.position, startCenterPosition);
        
        if (distance > 13.5f) {
            returnToSpawn();
        }
    }
    
    void returnToSpawn() {
        chasing.stopChasing();
        stats.restoreFullHealth();
        movement.currentMovement = movement.pointToMoveFrom(randomPoint);
        startPatrol();
    }
    
    public void startPatrol() {
        isPatrolling = true;
    }
    
    public void stopPatrol() {
        isPatrolling = false;
    }

    private Vector3 randomPointInsideCircle()
    {
        Vector2 startCenterPoint = new Vector2(startCenterPosition.x, startCenterPosition.z);
        Vector2 randomPoint = startCenterPoint + Random.insideUnitCircle * circleMovingRadius;
        return new Vector3(randomPoint.x, transform.position.y, randomPoint.y);
    }
}
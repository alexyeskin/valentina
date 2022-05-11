using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour {
    GeneralMovement movement;
    Stats stats;
    Chasing chasing;

    public float circleMovingRadius = 5f;

    Vector3 startCenterPosition;
    Vector3 randomPoint;

    bool isPatrolling = true;
    bool isHandleReturning = true;
    public bool isReturning = false;
    
    private IEnumerator movingWaiter;
    IEnumerator waiter() {
        isPatrolling = true;
        movement.currentMovement = Vector3.zero;
        yield return new WaitForSeconds(Random.Range(0.5f, 5.5f));
        movement.currentMovement = movement.pointToMoveFrom(randomPoint).normalized;
    }

    private void Awake() {
        movement = GetComponent<GeneralMovement>();
        stats = GetComponentInParent<Stats>();
        chasing = GetComponent<Chasing>();
        stats.Death += onDeath;
        stats.Respawning += onRespawn;
    }
    
    private void OnDestroy() {
        stats.Death -= onDeath;
        stats.Respawning -= onRespawn;
    }

    void Start() {
        startCenterPosition = stats.transform.position;
        randomPoint = randomPointInsideCircle();
        
        movingWaiter = waiter();
        StartCoroutine(movingWaiter);
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(startCenterPosition, circleMovingRadius);
    }

    void Update() {
        if (isPatrolling) {
            patrol();
        }
        
        if (isHandleReturning) {
            handleReturning();
        }
    }

    void patrol() {
        float distance = Vector3.Distance(stats.transform.position, randomPoint);

        // Point have reached
        if (distance < 0.1f * movement.currentMovementSpeed) {
            isReturning = false;
            
            randomPoint = randomPointInsideCircle();
            
            movingWaiter = waiter();
            StartCoroutine(movingWaiter);
        }
    }

    void handleReturning() {
        float distance = Vector3.Distance(stats.transform.position, startCenterPosition);
        
        if (distance > 13.5f) {
            returnToSpawn();
            Debug.Log("to Spawn");
        }
    }

    public void returnToSpawn() {
        isReturning = true;
        
        chasing.stopChasing();
        stats.restoreFullHealth();
        startPatrol();
    }
    
    private void onRespawn(Nothing nothing) {
        stats.transform.position = randomPointInsideCircle();
        chasing.stopChasing();
        
        
        movingWaiter = waiter();
        StartCoroutine(movingWaiter);
        
        isHandleReturning = true;
    }
    
    private void onDeath(Nothing nothing) {
        isHandleReturning = false;
        chasing.stopChasing();
        stopPatrol();
        
        if (movingWaiter != null) {
            StopCoroutine(movingWaiter);
        }
    }

    private void startPatrol() {
        movement.currentMovement = movement.pointToMoveFrom(randomPoint).normalized;
        isPatrolling = true;
    }

    public void stopPatrol() {
        movement.currentMovement = Vector3.zero;
        isPatrolling = false;
    }

    public void Aggro(Transform target) {
        if (!isReturning) {
            Debug.Log("Aggro");
            chasing.startChasing(target);
            stopPatrol();
        }
    }

    private Vector3 randomPointInsideCircle() {
        Vector2 startCenterPoint = new Vector2(startCenterPosition.x, startCenterPosition.z);
        Vector2 randomPoint = startCenterPoint + Random.insideUnitCircle * circleMovingRadius;
        return new Vector3(randomPoint.x, stats.transform.position.y, randomPoint.y);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aggro: MonoBehaviour
{
    Combat combat;
    
    void Start()
    {
        combat = GetComponent<Combat>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            if (other.TryGetComponent(out Chasing chasing)) {
                chasing.startChasing(transform);
            } else {
                Debug.Log("Chasing not found");
            }
            
            if (other.TryGetComponent(out RandomMovement movement)) {
                movement.stopPatrol();
            } else {
                Debug.Log("RandomMovement not found");
            }
            
            combat.addTarget(other.gameObject);
        }
    }
    
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Enemy")) {
            combat.removeTarget(other.gameObject);
        }
    }
}

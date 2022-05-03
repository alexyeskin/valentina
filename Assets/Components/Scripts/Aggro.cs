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
            }
            
            if (other.TryGetComponent(out RandomMovement movement)) {
                movement.stopPatrol();
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

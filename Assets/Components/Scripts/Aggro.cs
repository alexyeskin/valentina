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
            Chasing chasing = other.GetComponentInChildren<Chasing>();
            RandomMovement movement = other.GetComponentInChildren<RandomMovement>();
            Stats stats = other.GetComponent<Stats>();
            
            if (chasing != null) {
                chasing.startChasing(transform);
            }
            
            if (movement != null) {
                movement.stopPatrol();
            }
            
            if (stats.currentHealth > 0) {
                combat.addTarget(other.gameObject);
            } else {
                Debug.Log("Target have't added. It's dead.");
            }
        }
    }
    
    private void OnTriggerExit(Collider other) {
        // Todo also subscribe for Current health event from Stats
        if (other.CompareTag("Enemy")) {
            combat.removeTarget(other.gameObject);
        }
    }
}

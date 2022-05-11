using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aggro : MonoBehaviour {
    Stats characterStats;
    Combat characterCombat;

    void Start() {
        characterStats = GetComponentInParent<Stats>();
        characterCombat = GetComponentInParent<Combat>();
    }

    void Update() {

    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            Stats enemyStats = other.GetComponentInParent<Stats>();

            if (enemyStats.currentHealth > 0 && characterStats.currentHealth > 0) {
                RandomMovement enemyMovement = other.GetComponentInChildren<RandomMovement>();
                Combat enemyCombat = other.GetComponent<Combat>();

                if (enemyMovement) {
                    enemyMovement.Aggro(transform);
                } else {
                    Debug.Log("No component RandomMovement");
                }

                if (!enemyMovement.isReturning) {
                    characterCombat.addTarget(other.gameObject);

                    if (enemyCombat) {
                        enemyCombat.addTarget(gameObject);
                    } else {
                        Debug.Log("No component Combat");
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        // Todo also subscribe for Current health event from Stats
        if (other.CompareTag("Enemy")) {
            Combat enemyCombat = other.GetComponent<Combat>();

            characterCombat.removeTarget(other.gameObject);

            if (enemyCombat) {
                enemyCombat.removeTarget(gameObject);
            }
        }
    }
}

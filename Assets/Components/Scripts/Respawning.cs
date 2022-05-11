using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawning : MonoBehaviour
{
    Stats stats;
    
    public float respawnTime = 3f;
    
    private void Awake() {
        stats = GetComponent<Stats>();
        stats.HealthDecreased += onHealthDecreased;
    }
    
    private void OnDestroy() {
        stats.HealthDecreased -= onHealthDecreased;
    }
    
    IEnumerator respawnCounter() {
        yield return new WaitForSeconds(respawnTime);
        respawn();
    }
    
    private void onHealthDecreased(int amount) {
        if (stats.isDead) {
            StartCoroutine(respawnCounter());
        }
    }
    
    public void respawn() {
        foreach (Transform child in stats.transform) {
            if (child.CompareTag("UI") || child.CompareTag("Model"))
                child.gameObject.SetActive(true);
        }
        
        stats.Respawn();
    }
}

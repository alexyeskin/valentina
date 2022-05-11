using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextPresenter : MonoBehaviour {
    public GameObject floatingTextPrefub;
    private Stats stats;
    
    private Queue<int> textQueue = new Queue<int>();
    int textPositionIndex = 0;

    private void Awake() {
        stats = GetComponentInParent<Stats>();
        stats.HealthDecreased += onHealthDecreased;
    }
    
    private void OnDestroy() {
        stats.HealthDecreased -= onHealthDecreased;
    }

    void Start() {}
    
    private void Update() {
        
    }
    
    private IEnumerator textShowing;
    IEnumerator ShowTextCoroutine() {
        var damageToDisplay = textQueue.Dequeue();
        showFloatingDamageText(damageToDisplay);
        
        textPositionIndex += 1;
        if (textPositionIndex > 4) {
            textPositionIndex = 0;
        }
        yield return new WaitForSeconds(0.37f);
        textPositionIndex = 0;
    }
    
    private void onHealthDecreased(int amount) {
        textQueue.Enqueue(amount);
        if (textShowing != null) {
            StopCoroutine(textShowing);
        }
        textShowing = ShowTextCoroutine();
        StartCoroutine(textShowing);
    }

    private void showFloatingDamageText(int damage) {
        var floatingText = floatingTextPrefub.GetComponentInChildren<FloatingText>();
        floatingText.text = damage.ToString();

        if (!stats.gameObject.CompareTag("Enemy")) {
            floatingText.textColor = new Color32(224, 60, 31, 255);
        } else {
            floatingText.textColor = new Color32(255, 255, 255, 255);
        }

        Vector3 position = textPosition(textPositionIndex);

        Instantiate(floatingTextPrefub, position, Quaternion.identity, transform);
    }

    Vector3 textPosition(int positionIndex) {
        // .   .
        //   .
        // .   .

        // 4   1
        //   0
        // 3   2

        Vector3 position = new Vector3(
            transform.position.x,
            transform.position.y,
            transform.position.z
        );

        switch (positionIndex) {
            case 1:
                position.x += 0.85f;
                position.z += 0.95f;
                break;

            case 2:
                position.x += 0.85f;
                position.z -= 0.95f;
                break;

            case 3:
                position.x -= 0.85f;
                position.z -= 0.95f;
                break;

            case 4:
                position.x -= 0.85f;
                position.z += 0.95f;
                break;

            default: break;
        }

        return position;
    }
}

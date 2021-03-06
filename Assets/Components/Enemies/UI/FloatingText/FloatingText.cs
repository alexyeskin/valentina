using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FloatingText : MonoBehaviour {
    private TextMeshPro textContainter;
    
    float lifetime = 1.5f;
    
    public string text;
    public Color textColor = Color.white;

    private void Awake() {
        textContainter = GetComponent<TextMeshPro>();
    }

    void Start() {
        textContainter.text = text;
        textContainter.color = textColor;
        Destroy(gameObject.transform.parent.gameObject, lifetime);
    }

    private void LateUpdate() {
        Vector3 positionToLookAt = new Vector3(
            transform.position.x,
            2 * transform.position.y - Camera.main.transform.position.y,
            2 * transform.position.z - Camera.main.transform.position.z
        );

        transform.LookAt(positionToLookAt);
    }
}

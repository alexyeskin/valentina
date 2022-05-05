using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {
    private Camera cameraToLookAt;

    private void Awake() {
        cameraToLookAt = Camera.main;
    }

    void LateUpdate() {
        Vector3 positionToLookAt = new Vector3(
            transform.position.x,
            2 * transform.position.y - cameraToLookAt.transform.position.y,
            2 * transform.position.z - cameraToLookAt.transform.position.z
        );

        transform.LookAt(positionToLookAt);
    }
}

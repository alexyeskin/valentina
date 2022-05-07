using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    GeneralMovement movement;
    AnimationManager animationManager;

    [SerializeField]
    Joystick joystick;

    private void Awake() {
        movement = GetComponentInChildren<GeneralMovement>();
        animationManager = GetComponentInParent<AnimationManager>();
    }

    void Update() {
        // handleGravity();
        if (!handleKeyboardPressing()) {
            handleJoystickDragging();
        }
    }

    bool handleKeyboardPressing() {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        
        movement.currentMovement = move;

        if (move.x != 0 || move.y != 0 || move.z != 0) {
            return true;
        } else {
            return false;
        }
    }

    void handleJoystickDragging() {
        Vector3 move = new Vector3(joystick.Direction.normalized.x, 0, joystick.Direction.normalized.y);
        movement.currentMovement = move;
    }

    void handleGravity() {
        if (!movement.isGrounded) {
            float gravity = -0.5f;
            movement.currentMovement.y += gravity;
        }
    }
}
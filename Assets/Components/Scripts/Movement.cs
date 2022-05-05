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
        handleGravity();
        if (!handleKeyboardPressing()) {
            handleJoystickDragging();
        }
    }

    bool handleKeyboardPressing() {
        Vector2 direction = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        movement.currentMovement.x = direction.x;
        movement.currentMovement.z = direction.y;

        if (direction.x != 0 || direction.y != 0) {
            return true;
        } else {
            return false;
        }
    }

    void handleJoystickDragging() {
        movement.currentMovement.x = joystick.Direction.normalized.x;
        movement.currentMovement.z = joystick.Direction.normalized.y;
    }

    void handleGravity() {
        if (!movement.isGrounded) {
            float gravity = -0.5f;
            movement.currentMovement.y += gravity;
        }
    }
}
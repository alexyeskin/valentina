using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralMovement : MonoBehaviour {
    Animator animator;
    int isWalkingHash;

    CharacterController characterController;
    public Transform modelObject;

    public Vector3 currentMovement = Vector3.zero;
    private Vector3 gravityVelocity;
    public Vector3 AttackPositionToLookAt = Vector3.zero;

    public bool isGrounded {
        get {
            return characterController.isGrounded;
        }
    }

    public bool isMovementPressed {
        get {
            return currentMovement.x != 0 || currentMovement.z != 0;
        }
    }

    public float rotationFactorPerFrame = 3f;
    public float walkMovementSpeed = 3f;
    public float currentMovementSpeed = 3f;
    private float gravityValue = -9.81f;

    private void Awake() {
        animator = GetComponentInParent<Animator>();
        characterController = GetComponentInParent<CharacterController>();
    }

    private void Start() {
        isWalkingHash = Animator.StringToHash("isWalking");
    }

    void Update() {
        handleRotation();
        handleMovingAnimation();
        characterController.Move(currentMovement * currentMovementSpeed * Time.deltaTime);
        
        // if (currentMovement != Vector3.zero) {
        //     modelObject.forward = currentMovement;
        // }
        
        handleGravity();
    }
    
    void handleGravity() {
        if (isGrounded && gravityVelocity.y < 0) {
            gravityVelocity.y = 0f;
        }
        
        gravityVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(gravityVelocity * Time.deltaTime);
    }

    public Vector3 pointToMoveFrom(Vector3 position) {
        return position - transform.position;
    }

    public void handleRotation() {
        Vector3 positionToLookAt;

        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentMovement.z;

        Quaternion currentRotation = modelObject.rotation;
        
        if (AttackPositionToLookAt != Vector3.zero) {
            Quaternion targetRotarion = Quaternion.LookRotation(AttackPositionToLookAt);
            modelObject.rotation = Quaternion.Slerp(currentRotation, targetRotarion, rotationFactorPerFrame);
        } else if (isMovementPressed) {
            Quaternion targetRotarion = Quaternion.LookRotation(positionToLookAt);
            modelObject.rotation = Quaternion.Slerp(currentRotation, targetRotarion, rotationFactorPerFrame);
        }
    }

    void handleMovingAnimation() {
        bool isAnimationWalking = animator.GetBool(isWalkingHash);

        if (isMovementPressed && !isAnimationWalking) {
            animator.SetBool(isWalkingHash, true);
        } else if (!isMovementPressed && isAnimationWalking) {
            animator.SetBool(isWalkingHash, false);
        }
    }
}

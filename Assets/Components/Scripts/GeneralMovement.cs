using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralMovement : MonoBehaviour
{
    CharacterController characterController;
    
    public Vector3 currentMovement = Vector3.zero;
    
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
    
    public float rotationFactorPerFrame = 0.5f;
    public float walkMovementSpeed = 3f;
    public float currentMovementSpeed = 3f;
    
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }
    
    void FixedUpdate()
    {
        handleRotation();
        
        characterController.Move(currentMovement * currentMovementSpeed * Time.deltaTime);
    }
    
    public Vector3 pointToMoveFrom(Vector3 position)
    {
        return position - transform.position;
    }
    
    public void handleRotation()
    {
        Vector3 positionToLookAt;

        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentMovement.z;

        Quaternion currentRotation = transform.rotation;

        if (isMovementPressed)
        {
            Quaternion targetRotarion = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotarion, rotationFactorPerFrame);
        }
    }
    
    public void rotateTo(Vector3 positionToLookAt) {
        transform.LookAt(positionToLookAt);
    }
}

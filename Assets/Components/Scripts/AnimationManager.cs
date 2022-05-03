using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;
    
    GeneralMovement movement;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<GeneralMovement>();
        isWalkingHash = Animator.StringToHash("isWalking");
    }
    
    void Update()
    {
        if (movement != null) {
            handleMovingAnimation();
        }
    }
    
    public void playAttackAnimation() {
        animator.SetTrigger("attack");
    }
    
    void handleMovingAnimation()
    {
        bool isAnimationWalking = animator.GetBool(isWalkingHash);

        if (movement.isMovementPressed && !isAnimationWalking)
        {
            animator.SetBool(isWalkingHash, true);
        }

        else if (!movement.isMovementPressed && isAnimationWalking)
        {
            animator.SetBool(isWalkingHash, false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;
    
    GeneralMovement movement;
    
    [Header("Floating Text")]
    public GameObject floatingTextPrefub;
    public float textYOffset = 3;
    
    [Header("Death Effect")]
    public GameObject deathEffectPrefub;
    
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
    
    public void playDeathEffect() {
        Instantiate(deathEffectPrefub, transform.position, Quaternion.identity, transform);
    }
    
    public void showFloatingDamageText(int damage) {
        if (floatingTextPrefub) {
            if (floatingTextPrefub.TryGetComponent(out FloatingText floatingText)) {
                floatingText.text = damage.ToString();
            }
            Vector3 position = transform.position;
            position.y = textYOffset;
            
            Instantiate(floatingTextPrefub, position, Quaternion.identity, transform);
        }
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

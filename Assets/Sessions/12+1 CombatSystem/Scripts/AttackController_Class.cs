using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CombatSystemPlayerState_Class))]
public class AttackController_Class : MonoBehaviour
{
    [SerializeField] private float chargeSpeed = 0.2f;
    [SerializeField] private float lightAttackCost = 20;
    [SerializeField] private float heavyAttackCost = 50;
    private Animator animator;
    Animator Animator
    {
        get
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }

            return animator;
        }
    }
    public void OnLightAttack(InputAction.CallbackContext ctx)
    {
        bool val = ctx.ReadValueAsButton();
        if (val)
        {
            //Check if stamina
            if (!GetComponent<CombatSystemPlayerState_Class>().ModifyStamina(-lightAttackCost)) 
                return;

            //Attack
            Animator.SetTrigger("Attack");
            Animator.SetBool("HeavyAttack", false);
        }
    }

    public void OnHeavyAttack(InputAction.CallbackContext ctx)
    {
        bool val = ctx.ReadValueAsButton();
        if (val)
        {
            if (!GetComponent<CombatSystemPlayerState_Class>().ModifyStamina(-heavyAttackCost)) 
                return;
            Animator.SetTrigger("Attack");
            Animator.SetBool("HeavyAttack", true);
            Animator.SetFloat("ChargeSpeed", chargeSpeed);
        }
        else
        {
            Animator.SetFloat("ChargeSpeed", 1);
        }
    }
}

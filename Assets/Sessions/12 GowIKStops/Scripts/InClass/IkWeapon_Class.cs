using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class IkWeapon_Class : MonoBehaviour
{
    enum AnimationState
    {
        StandBy,
        Stopped,
        CoolingDown
    }

    [SerializeField] private AnimationState currentState;
    [SerializeField] private float cooldownTime;

    [SerializeField] private MultiParentConstraint weaponToArmChild;
    [SerializeField] private ChainIKConstraint armToWeaponIk;

    private float currentStopTime;
    private float currentStopTimer;

    private float cooldownTimer;
    
    public void Stop(float delay)
    {
        if (currentState != AnimationState.StandBy) return;
        currentState = AnimationState.Stopped;
        currentStopTime = delay;
        weaponToArmChild.weight = 0;
        armToWeaponIk.weight = 1;
    }

    private void Update()
    {
        switch (currentState)
        {
            case AnimationState.Stopped:
                currentStopTimer += Time.deltaTime;
                if (currentStopTimer > currentStopTime)
                {
                    currentStopTimer = 0;
                    currentState = AnimationState.CoolingDown;
                    weaponToArmChild.weight = 1;
                    armToWeaponIk.weight = 0;
                }
                break;
            case AnimationState.CoolingDown:
                cooldownTimer += Time.deltaTime;
                if (cooldownTimer > cooldownTime)
                {
                    cooldownTimer = 0;
                    currentState = AnimationState.StandBy;
                }
                break;
        }
    }
}

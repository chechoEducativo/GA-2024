using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystemPlayerState_Class : MonoBehaviour
{
    [SerializeField] private float baseStamina;
    [SerializeField] private float currentStamina;
    [SerializeField] private float staminaRegenValue;

    private void Awake()
    {
        currentStamina = baseStamina;
    }

    private void Update()
    {
        ModifyStamina(staminaRegenValue * Time.deltaTime);
    }

    public bool ModifyStamina(float value)
    {
        if (value > 0)
        {
            currentStamina += value;
            currentStamina = Mathf.Min(baseStamina, currentStamina);
            return true;
        }
        if (currentStamina > 0)
        { 
            currentStamina += value;
            return true;
        }
        return false;
    }
}

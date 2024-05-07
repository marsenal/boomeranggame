using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] GameObject hitArea;
    [SerializeField] float attackCooldown;
    [SerializeField] Transform hitPosition;
    float timer;
    public bool canAttack;
    public bool isWeaponInHand;

    private void Start()
    {
        isWeaponInHand = true;
    }

    void Update()
    {
        if (isWeaponInHand)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                canAttack = false;
            }
            else
            {
                canAttack = true;
                timer = 0f;
            }
        }
        else
        {
            canAttack = false;
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (canAttack)
        {
            Instantiate(hitArea, hitPosition, false); //another overload for the function - this way we instantiate it not in world space
            timer = attackCooldown;
        }
    }

    public void EnableAttack(bool value)
    {
        isWeaponInHand = value;
    }
}

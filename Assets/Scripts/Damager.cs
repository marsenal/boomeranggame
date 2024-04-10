using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    [SerializeField][Tooltip("This is the time in seconds the attack is 'alive' - duration of attack")] float activeTime;
    [SerializeField] int damageValue;
    [SerializeField][Tooltip("Tick this if the parent object is a temporary object (i.e melee attack, explosion)")] bool isDisappearingDamageSource;

    void Update()
    {
        if (isDisappearingDamageSource)
        {
            activeTime -= Time.deltaTime;
            if (activeTime < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player" && collision.gameObject != transform.parent.gameObject)
        {
            collision.GetComponent<Player>().GetDamaged(damageValue);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && collision.gameObject != GetComponent<RangedAttack>().parentTransform.gameObject)
        {
            collision.gameObject.GetComponent<Player>().GetDamaged(damageValue);
        }
    }
}

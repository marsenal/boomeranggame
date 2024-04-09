using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitArea : MonoBehaviour
{
    [SerializeField][Tooltip("This is the time in seconds the attack is 'alive' - duration of attack")] float activeTime;
    [SerializeField] int damageValue;

    void Update()
    {
        activeTime -= Time.deltaTime;
        if (activeTime < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player" && collision.gameObject != this)
        {
            collision.GetComponent<Player>().GetDamaged(damageValue);
        }
    }
}

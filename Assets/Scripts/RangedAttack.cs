using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    Rigidbody myRigidbody;
    [SerializeField] float throwForce;
    [SerializeField] float checkRadius;
    [SerializeField] Transform hand;
    Vector3 throwDirection;
    Transform startTransform;
    [SerializeField] public Transform parentTransform;
    public bool isWeaponOnFloor;
    public bool isWeaponThrown;
    void Start()
    {
        startTransform = transform;
        //parentTransform = transform.parent;
        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.isKinematic = true;
        Debug.Log(startTransform.position);

    }
    void Update()
    {
        CheckForPlayer();
        if (transform.position != startTransform.position && myRigidbody.velocity == Vector3.zero)
        {
            isWeaponOnFloor = true;
        }
    }

    private void FixedUpdate()
    {
        if (isWeaponThrown)
        {
            
        }
    }

    public void ThrowWeapon()
    {
        if (!isWeaponThrown)
        {
            isWeaponThrown = true;
            myRigidbody.isKinematic = false;
            
            throwDirection = GetComponentInParent<Player>().transform.forward;
            myRigidbody.constraints = RigidbodyConstraints.None;

            transform.parent = null;
            myRigidbody.AddForce(throwForce * throwDirection, ForceMode.Force);
        }
    }

    private void PickUpWeapon()
    {
        transform.position = hand.position;
        transform.parent = parentTransform;
        Debug.Log(startTransform.position);
        myRigidbody.isKinematic = true;
        myRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        isWeaponThrown = false;
    }

    private void CheckForPlayer()
    {
       if( Physics.CheckSphere(transform.position, checkRadius, LayerMask.GetMask("Player")) && isWeaponThrown)
        {
           foreach (Collider collider in Physics.OverlapSphere(transform.position, checkRadius, LayerMask.GetMask("Player")) )
            {
                if (collider == parentTransform.GetComponent<CapsuleCollider>())
                {
                    PickUpWeapon();
                }
            }

        }
    }
}

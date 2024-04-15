using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class RangedAttack : MonoBehaviour
{
    Rigidbody myRigidbody;
    [SerializeField] float throwForce;
    [SerializeField] float maxThrowDistance;
    float throwIntensity = 0f;

    [SerializeField] float checkRadius;
    [SerializeField] Transform hand;
    Vector3 throwDirection;
    Transform startTransform;
    Vector3 startThrowPosition;
    float deltaDistance;
    [SerializeField] public Transform parentTransform;

    public bool isWeaponThrown;

    enum WeaponState
    {
        Inactive,
        Thrown,
        ComingBack,
        Lost
    }

    [SerializeField] WeaponState myState;
    void Start()
    {
        Physics.IgnoreCollision(GetComponent<SphereCollider>(), parentTransform.GetComponent<CapsuleCollider>());
        startTransform = transform;
        //parentTransform = transform.parent;
        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.isKinematic = true;

        myState = WeaponState.Inactive;

    }
    void Update()
    {
        CheckForPlayer();

        Debug.Log("Duration of weapon throw:" + throwIntensity);
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y,0f,2f), transform.position.z);
        //myRigidbody.velocity = new Vector3(Mathf.Clamp(myRigidbody.velocity.x, -15f, 15f), Mathf.Clamp(myRigidbody.velocity.y, 0f, 0f), Mathf.Clamp(myRigidbody.velocity.z, -15f, 15f));

        StateMachine();
    }

    private void AniMachine()
    {
        switch (myState)
        {
            case WeaponState.Inactive:
                break;
            case WeaponState.Thrown:
                break;
            case WeaponState.ComingBack:
                break;
            case WeaponState.Lost:
                break;
        }
    }

    private void StateMachine()
    {
        if (myState == WeaponState.Thrown)
        {

            transform.forward = myRigidbody.velocity.normalized;

            //Debug.Log("Velocity is: " + myRigidbody.velocity.normalized);
            deltaDistance = Mathf.Abs((transform.position - startThrowPosition).magnitude);
            // Debug.Log("Current delta position magnitude: " + deltaDistance + "\n maximum throwing distance: " + maxThrowDistance);


            if (deltaDistance >= maxThrowDistance * throwIntensity)
            {

                //myRigidbody.AddRelativeForce(Vector3.right * throwForce);
                myRigidbody.AddRelativeForce(Vector3.back * throwForce * 2f * throwIntensity, ForceMode.Force);

              //  myRigidbody.AddRelativeTorque((new Vector3(3f, 0f, 0f)) * 10000f, ForceMode.Force);


                Debug.Log(transform.up * 100f);
                myState = WeaponState.ComingBack;
            }

            /* if (myRigidbody.velocity.magnitude <= maxThrowDistance)
             {
                 myRigidbody.AddRelativeForce(Vector3.back * throwForce * 2f, ForceMode.Impulse); 
                 myState = WeaponState.ComingBack;
             }*/
        }
        else if (myState == WeaponState.ComingBack)
        {

            // myRigidbody.AddRelativeTorque((new Vector3(3f, 0f, 0f) + Vector3.up) * 10000f, ForceMode.Force);
            myRigidbody.constraints = RigidbodyConstraints.None;
            transform.forward = myRigidbody.velocity.normalized;
            Debug.Log("Magnitued of the velocity vector is: " + myRigidbody.velocity.magnitude);
            if (Mathf.Abs(myRigidbody.velocity.magnitude) <= 2f)
            {
                myState = WeaponState.Lost;
            }
        } 
        else if (myState == WeaponState.Lost && myRigidbody.velocity.magnitude <= 2f) //this makes it that lost weapon doesn't keep tumbling around
        {
            myRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    public void ThrowWeapon(InputAction.CallbackContext context)
    {
        /*if (context.started)
        {
            throwStrength = (float)context.duration;
            Debug.Log("Duration of weapon throw:" + context.duration);
        }*/
        if (context.interaction is PressInteraction)
        {
            if (context.started)
            {
                Debug.Log("Duration of weapon throw:" + context.duration);
            }
            if (context.canceled)
            {
                if (myState == WeaponState.Inactive)
                {
                    throwIntensity = (float)context.duration;
                    throwIntensity = Mathf.Clamp(throwIntensity, 0.5f, 1f);
                    myState = WeaponState.Thrown;
                    myRigidbody.isKinematic = false;

                    startThrowPosition = parentTransform.position;
                    throwDirection = GetComponentInParent<Player>().transform.forward;

                    myRigidbody.constraints = RigidbodyConstraints.FreezeRotationX;
                    myRigidbody.constraints = RigidbodyConstraints.FreezeRotationZ;
                    myRigidbody.constraints = RigidbodyConstraints.FreezePositionY;

                    transform.parent = null;
                    myRigidbody.AddForce(throwForce * throwDirection * throwIntensity, ForceMode.Force);

                    Debug.Log(myRigidbody.velocity);
                    transform.forward = myRigidbody.velocity.normalized;
                }
            }
            else if (context.interaction is HoldInteraction)
            {
                if (myState == WeaponState.Inactive)
                {
                    throwIntensity = (float)context.duration;
                    throwIntensity = Mathf.Clamp(throwIntensity, 0.5f, 1f);
                    myState = WeaponState.Thrown;
                    myRigidbody.isKinematic = false;

                    startThrowPosition = parentTransform.position;
                    throwDirection = GetComponentInParent<Player>().transform.forward;

                    myRigidbody.constraints = RigidbodyConstraints.FreezeRotationX;
                    myRigidbody.constraints = RigidbodyConstraints.FreezeRotationZ;
                    myRigidbody.constraints = RigidbodyConstraints.FreezePositionY;

                    transform.parent = null;
                    myRigidbody.AddForce(throwForce * throwDirection * throwIntensity, ForceMode.Force);

                    Debug.Log(myRigidbody.velocity);
                    transform.forward = myRigidbody.velocity.normalized;
                }
            }
        }

    }

    private void PickUpWeapon()
    {
        transform.position = hand.position;
        transform.parent = parentTransform;
        
        myRigidbody.isKinematic = true;
        myRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        myState = WeaponState.Inactive;
    }

    private void CheckForPlayer()
    {
       if( Physics.CheckSphere(transform.position, checkRadius, LayerMask.GetMask("Player")) && (myState == WeaponState.Lost || myState == WeaponState.ComingBack || myState == WeaponState.Thrown))
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
            if (Mathf.Abs(myRigidbody.velocity.magnitude) <= 2f)
            {
                myState = WeaponState.Lost;
                myRigidbody.constraints = RigidbodyConstraints.FreezeAll;
            }

    }

    public bool IsWeaponDamaging()
    {
        switch (myState)
        {
            case WeaponState.ComingBack:
                return true;
            case WeaponState.Thrown:
                return true;
            case WeaponState.Inactive:
                return false;
            case WeaponState.Lost:
                return false;
            default:
                return false;
        }
    }

}

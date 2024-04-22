using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class RangedAttack : MonoBehaviour
{
    Rigidbody myRigidbody;
    [SerializeField] float maxSpeed;
    [SerializeField] float throwForce;
    [SerializeField] float maxThrowDuration;
    public float throwStrength = 0f;
    [SerializeField] float baseStallDuration;
    public float currentStalling;

    [SerializeField] float checkRadius;
    [SerializeField] Transform hand;
    Vector3 throwDirection;
    Transform startTransform;
    Vector3 startThrowPosition;
    public float deltaDistance;
    [SerializeField] public Transform parentTransform;

    public bool isWeaponThrown;

    enum WeaponState
    {
        Inactive,
        Thrown,
        Stalling,
        ComingBack,
        Lost
    }

    [SerializeField] WeaponState myState;
    void Start()
    {
        Physics.IgnoreCollision(GetComponent<SphereCollider>(), parentTransform.GetComponent<CapsuleCollider>());
        startTransform = transform;

        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.isKinematic = true;

        myState = WeaponState.Inactive;
        myRigidbody.maxLinearVelocity = maxSpeed;

    }
    void Update()
    {
        CheckForPlayer();

        Debug.Log("Duration of weapon throw:" + throwStrength);
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y,0f,2f), transform.position.z);

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
            currentStalling = 0f;

            transform.forward = myRigidbody.velocity.normalized;

            deltaDistance += Time.deltaTime;

            if (deltaDistance >= maxThrowDuration * throwStrength)
            {
                myState = WeaponState.Stalling;
            }
        }
        else if (myState == WeaponState.ComingBack)
        {
            myRigidbody.constraints = RigidbodyConstraints.None;

            Debug.Log("Parent transform position is: " + parentTransform.position);

            Vector3 comingBackVector = (parentTransform.position - transform.position).normalized;

            myRigidbody.AddForce(comingBackVector * throwForce * 0.8f * throwStrength, ForceMode.VelocityChange);

            myRigidbody.constraints = RigidbodyConstraints.None;
            transform.forward = myRigidbody.velocity.normalized;

            Debug.Log("Velocity vector: " + myRigidbody.velocity);
        } 
        else if (myState == WeaponState.Lost && myRigidbody.velocity.magnitude <= 2f) //this makes it that lost weapon doesn't keep tumbling around
        {
            myRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }
        else if (myState == WeaponState.Stalling)
        {
            currentStalling += Time.deltaTime;
            Vector3 comingBackVector = (parentTransform.position - transform.position).normalized;

            if (currentStalling >= baseStallDuration / throwStrength)
            {
                myState = WeaponState.ComingBack;
            }
        }
    }

    public void ThrowWeapon(InputAction.CallbackContext context)
    {
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
                    deltaDistance = 0f;

                    throwStrength = (float)context.duration;
                    throwStrength = Mathf.Clamp(throwStrength, 0.5f, 1f);
                    myState = WeaponState.Thrown;
                    myRigidbody.isKinematic = false;

                    startThrowPosition = parentTransform.position;
                    throwDirection = GetComponentInParent<Player>().transform.forward;

                    myRigidbody.constraints = RigidbodyConstraints.FreezeRotationX;
                    myRigidbody.constraints = RigidbodyConstraints.FreezeRotationZ;
                    myRigidbody.constraints = RigidbodyConstraints.FreezePositionY;

                    transform.parent = null;
                    myRigidbody.AddForce(throwForce * throwDirection * throwStrength, ForceMode.VelocityChange);

                    Debug.Log(myRigidbody.velocity);
                    transform.forward = myRigidbody.velocity.normalized;
                }
            }
            else if (context.interaction is HoldInteraction)
            {
                if (myState == WeaponState.Inactive)
                {
                    deltaDistance = 0f;

                    throwStrength = (float)context.duration;
                    throwStrength = Mathf.Clamp(throwStrength, 0.5f, 1f);
                    myState = WeaponState.Thrown;
                    myRigidbody.isKinematic = false;

                    startThrowPosition = parentTransform.position;
                    throwDirection = GetComponentInParent<Player>().transform.forward;

                    myRigidbody.constraints = RigidbodyConstraints.FreezeRotationX;
                    myRigidbody.constraints = RigidbodyConstraints.FreezeRotationZ;
                    myRigidbody.constraints = RigidbodyConstraints.FreezePositionY;

                    transform.parent = null;
                    myRigidbody.AddForce(throwForce * throwDirection * throwStrength, ForceMode.VelocityChange);

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

        deltaDistance = 0f;
    }

    private void CheckForPlayer()
    {
       if( Physics.CheckSphere(transform.position, checkRadius, LayerMask.GetMask("Player")) 
            && (myState == WeaponState.Lost || myState == WeaponState.ComingBack /*|| myState == WeaponState.Thrown*/)
            && deltaDistance >= 0.05f)
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
        {
            myRigidbody.AddForce(myRigidbody.velocity * throwForce * 0.01f, ForceMode.VelocityChange);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            if (myState != WeaponState.Inactive)
            {
                LoseWeapon();
            }
        }
    }

    private void LoseWeapon()
    {
        myRigidbody.AddForce(-myRigidbody.velocity * throwForce * 0.01f, ForceMode.VelocityChange);
        myRigidbody.constraints = RigidbodyConstraints.None;
        myState = WeaponState.Lost;
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

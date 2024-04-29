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

    [SerializeField] float maxPullBackTime;
    private float pullbackCounter = 0f;

    public int wallsHit;
    bool canBounceOffWall;

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
        Physics.IgnoreCollision(GetComponent<BoxCollider>(), parentTransform.GetComponent<CapsuleCollider>());
        startTransform = transform;

        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.isKinematic = true;

        myState = WeaponState.Inactive;
        myRigidbody.maxLinearVelocity = maxSpeed;

    }
    void Update()
    {
        CheckForPlayer();

       // Debug.Log("Duration of weapon throw:" + throwStrength);
    }

    private void FixedUpdate()
    {
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
            myRigidbody.constraints = RigidbodyConstraints.FreezePositionY;

            Debug.Log("Parent transform position is: " + parentTransform.position);

            Vector3 comingBackVector = (parentTransform.position - transform.position).normalized;

            myRigidbody.AddForce(comingBackVector * throwForce * 0.8f * throwStrength, ForceMode.VelocityChange);

            transform.forward = myRigidbody.velocity.normalized;

            pullbackCounter += Time.deltaTime;

            if (pullbackCounter > maxPullBackTime)
            {
                myState = WeaponState.Lost;
                pullbackCounter = 0f;
            }

            // Debug.Log("Velocity vector: " + myRigidbody.velocity);
        }
        else if (myState == WeaponState.Lost) //this makes it that lost weapon doesn't keep tumbling around
        {
            transform.forward = myRigidbody.velocity.normalized;
            myRigidbody.AddRelativeForce(-transform.forward * 2f, ForceMode.VelocityChange);
            canBounceOffWall = false;

            if (myRigidbody.velocity.magnitude <= 2f) { 
            myRigidbody.constraints = RigidbodyConstraints.FreezeAll; }
        }
        else if (myState == WeaponState.Stalling)
        {
            transform.forward = myRigidbody.velocity.normalized;
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
                //Debug.Log("Duration of weapon throw:" + context.duration);
            }
            if (context.canceled)
            {
                if (myState == WeaponState.Inactive)
                {
                    canBounceOffWall = true;

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
                    canBounceOffWall = true;

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
        if (context.interaction is HoldInteraction && myState == WeaponState.Lost) //this doesn't work - holding throw button when weap is lost pulls it back
        {
            if (context.canceled)
            {
                Debug.Log("Weapon is lost, pulling it back");
                myState = WeaponState.ComingBack;
            }
        }

    }

    private void PickUpWeapon()
    {
        transform.position = hand.position;
        transform.parent = parentTransform;

        transform.forward = parentTransform.forward;
        
        myRigidbody.isKinematic = true;
        myRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        myState = WeaponState.Inactive;

        deltaDistance = 0f;

        throwStrength = 0f;

        wallsHit = 0;
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
        if (collision.gameObject.tag == "Wall" && canBounceOffWall && (myState == WeaponState.Thrown || myState == WeaponState.Stalling))
        {
            myRigidbody.AddForce(myRigidbody.velocity * throwForce * 0.005f, ForceMode.VelocityChange);
            wallsHit++;
            if (wallsHit>2)
            {
                LoseWeapon(collision.transform);
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            if (myState != WeaponState.Inactive)
            {
                LoseWeapon(other.transform);
            }
        }
    }

    private void LoseWeapon(Transform originOfHit)
    {
        Vector3 deflectionVector = transform.position - originOfHit.position;
        Debug.Log("Vector of deflection is: " + (transform.position - originOfHit.position));
        myRigidbody.AddRelativeForce(-transform.forward * 0.2f, ForceMode.VelocityChange);
        myRigidbody.AddForce( deflectionVector * throwForce * 0.2f, ForceMode.VelocityChange);
        // myRigidbody.constraints = RigidbodyConstraints.None;
        myRigidbody.constraints = RigidbodyConstraints.FreezeRotationX;
        myRigidbody.constraints = RigidbodyConstraints.FreezeRotationZ;
        myRigidbody.constraints = RigidbodyConstraints.FreezePositionY;

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
            case WeaponState.Stalling:
                return true;
            default:
                return false;
        }
    }

    private void LimitArea()
    {

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, 0f, 2f), transform.position.z);
        if (transform.position.y < 0f)
        {
            transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        }
    }

}

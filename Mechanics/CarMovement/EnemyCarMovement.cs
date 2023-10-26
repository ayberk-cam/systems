using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCarMovement : MonoBehaviour
{
    [SerializeField] float maxReverseSpeed;
    [SerializeField] float brakeAcceleration;
    [SerializeField] float acceleration;
    [SerializeField] float reverseAcceleration;
    [SerializeField] float turnCoeff;
    
    [SerializeField] float verticalGrip;
    [SerializeField] float maxTurnAngle;
    [SerializeField] float lateralGripMin = 1f;
    [SerializeField] float lateralGripMax = 2f;

    [Range(0,1)]
    [SerializeField] float lateralGripLerpFactor = 0.5f;
    [Range(0, 1)]
    [SerializeField] float steerLerpFactor = 0.5f;
    [Range(0,1)]
    [SerializeField] float driftSpeedPercentage = 0.75f;
    [SerializeField] float steerDirection;
    [SerializeField] float lateralGrip;
    [SerializeField] float maxSpeed;

    private float lateralGripMinLimitMin = 0.5f;
    private float lateralGripMinLimitMax = 11f;
    private float lateralGripMaxLimit = 14f;
    private Vector3 carDirection;
    private Rigidbody rb;
    private Vector3 verticalVelocity;
    private float verticalSpeed;
    private Vector3 lateralVelocity;
    private float lateralSpeed;
    private Vector3 relativePosiiton;
    private float tireTrackSpeedLimit = 120f;
    private float accConst = 1;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        rb.constraints &= ~RigidbodyConstraints.FreezeAll;
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Start()
    {
        rb.centerOfMass = Vector3.zero;
        rb.inertiaTensorRotation = Quaternion.identity;
    }

    private void FixedUpdate()
    {
        if(GameManager.isGameOver)
             return;

        verticalSpeed = Vector3.Dot(transform.forward, rb.velocity);

        verticalVelocity = verticalSpeed * transform.forward;

        lateralSpeed = Vector3.Dot(transform.right, rb.velocity);

        lateralVelocity = lateralSpeed * transform.right;

        UpdateTireTrack();

        Rotate();

        Friction();

        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    private void OnDisable() 
    {
        rb.velocity = Vector3.zero;

        UpdateTireTrack();

        StopAllCoroutines();
    }

    private void UpdateTireTrack()
    {
        if(lateralVelocity.magnitude >= tireTrackSpeedLimit)
        {
            carEffects.UpdateTireTrack(true);
        }
        else 
        {
            carEffects.UpdateTireTrack(false);
        }
    }

    public void ChangeDirection(int inputDirection)
    {
        steerDirection = Mathf.Lerp(steerDirection,(float)inputDirection, steerLerpFactor);
    }

    public void Accelerate(float deltaTime, float rate=1)
    {
        if(verticalSpeed >= 0)
        {
            rb.velocity += rate* acceleration * deltaTime * (transform.forward);

            if (rb.velocity.magnitude >= maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }
        }
        else
        {
            rb.velocity += 2 *rate* acceleration * deltaTime * (transform.forward);
        }
    }

    private void Rotate()
    {   
        float turnAngle = steerDirection * Mathf.Min(Time.fixedDeltaTime * turnCoeff * verticalVelocity.magnitude, maxTurnAngle) * Mathf.Sign(verticalSpeed);

        transform.rotation *= Quaternion.AngleAxis(turnAngle, Vector3.up);

        carDirection = transform.forward;
    }

    private void Friction()
    {
        if(rb.velocity.magnitude > driftSpeedPercentage * maxSpeed)
        {
            lateralGrip = Mathf.Lerp(lateralGrip, lateralGripMin, lateralGripLerpFactor);
        }
        else
        {
            lateralGrip = Mathf.Lerp(lateralGrip, lateralGripMax, lateralGripLerpFactor);
        }
        
        rb.velocity += Mathf.Min(lateralGrip * Time.fixedDeltaTime, lateralVelocity.magnitude) * lateralVelocity * -1;
    }

    private void FindOutDirection()
    {
        relativePosiiton = transform.InverseTransformPoint(aimPosition);

        if(relativePosiiton.x < -15)
        {
            ChangeDirection(-1);
        }
        else if(relativePosiiton.x > 15)
        {
            ChangeDirection(1);
        }
        else
        {
            ChangeDirection(0);
        }

        if(relativePosiiton.z > 0)
        {
            Accelerate(Time.fixedDeltaTime,accConst);
        }
        else
        {
            Reverse();
        }
    }

    public void Reverse()
    {
        if(verticalSpeed > 0)
        {
            rb.velocity += Mathf.Min(brakeAcceleration * Time.fixedDeltaTime, verticalVelocity.magnitude) * transform.forward * -1;
        }
        else
        {
            ChangeDirection((int)((Random.Range(0,1)-0.5f) * 2f));

            rb.velocity += reverseAcceleration * Time.fixedDeltaTime * transform.forward * -1;

            if(rb.velocity.magnitude > maxReverseSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxReverseSpeed;
            }
        }
    }

    public void AssignTarget(Vector3 newTarget)
    {
        aimPosition = newTarget;

        FindOutDirection();
    }

    public void UpdateStats(float speedMultiplier, float handling)
    {
        float playerSpeed = WorldManager.instance.playerCar.GetComponent<PlayerCar>().GetBaseSpeed();
        float minSpeed = WorldManager.instance.minSpeed;
        float globalMaxSpeed = WorldManager.instance.maxSpeed;

        maxSpeed = speedMultiplier * playerSpeed;
        maxReverseSpeed = maxSpeed * 0.5f;

        brakeAcceleration = maxSpeed * 1.5f;
        reverseAcceleration = maxSpeed * 0.4f;

        acceleration = maxSpeed * 0.6f;

        lateralGripMin = Mathf.Lerp(lateralGripMinLimitMin, lateralGripMinLimitMax, Mathf.Pow(handling / 100f, 2f));
        lateralGripMax = lateralGripMin + 2.5f;
    }

    public void UpdateLateralGrip(float handling)
    {
        lateralGripMin = Mathf.Lerp(lateralGripMinLimitMin, lateralGripMinLimitMax, Mathf.Pow(handling / 100f, 2f));
        lateralGripMax = lateralGripMin + 2.5f;
    }
}

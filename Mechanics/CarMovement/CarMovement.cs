using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [SerializeField] float maxSpeed;
    [SerializeField] float maxReverseSpeed;
    [SerializeField] float brakeAcceleration;
    [SerializeField] float acceleration;
    [SerializeField] float accelerationMin;
    [SerializeField] float accelerationMax;

    [Range(0, 1)]
    [SerializeField] float accelerationLerpFactor;
    [SerializeField] float reverseAcceleration;
    [SerializeField] float turnCoeff;

    [SerializeField] float verticalGrip;
    [SerializeField] float maxTurnAngleMax;
    [SerializeField] float maxTurnAngleMin;
    [SerializeField] float maxTurnAngle;
    [SerializeField] float lateralGripMin = 1f;
    [SerializeField] float lateralGripMax = 2f;
    [SerializeField] float steerDirection;
    [SerializeField] float lateralGrip;
    [SerializeField] float tireTrackSpeedLimit = 120f;

    private float lateralGripMinLimitMin = 0.5f;
    private float lateralGripMinLimitMax = 11f;
    private float maxSpeedLimit = 350f;
    private float minSpeedLimit = 180f;
    private float globalMaxSpeed = 200f;

    [Range(0, 1)]
    [SerializeField] float lateralGripLerpFactor = 0.5f;
    [Range(0, 1)]
    [SerializeField] float steerLerpFactor = 0.5f;
    [Range(0, 1)]
    [SerializeField] float driftSpeedPercentage = 0.75f;
    [Range(0, 1)]
    [SerializeField] float turnAngleSpeedPercentage = 0.75f;

    private Vector3 carDirection;
    private Rigidbody rb;
    private Vector3 verticalVelocity;
    private float verticalSpeed;
    private Vector3 lateralVelocity;
    private float lateralSpeed;
    
    private static float stopSpeed = 10f;
    private static float stopCountTime = 1f;
    private float stopCountTimer = 0f;

    private Vector3 input;
    private Coroutine stopRoutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        rb.centerOfMass = Vector3.zero;
        rb.inertiaTensorRotation = Quaternion.identity;
    }

    private void Update()
    {
        input = new Vector3(WorldManager.instance.inputManager.GetInput().x,0,WorldManager.instance.inputManager.GetInput().y);
    }

    private void FixedUpdate()
    {
        if (GameManager.isGameOver)
        {
            verticalSpeed = 0;
            verticalVelocity = Vector3.zero;
            lateralSpeed = 0;
            lateralVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;
            return;
        }
            
        if (rb.velocity.magnitude < stopSpeed)
        {
            stopCountTimer += Time.fixedDeltaTime;
        }
        else
        {
            stopCountTimer = 0;
        }

        verticalSpeed = Vector3.Dot(transform.forward, rb.velocity);
        verticalVelocity = verticalSpeed * transform.forward;
        lateralSpeed = Vector3.Dot(transform.right, rb.velocity);
        lateralVelocity = lateralSpeed * transform.right;

        UpdateTireTrack();

        Rotate();

        Friction();
    }

    public void SetCarEffects(CarEffects carEffects)
    {
        this.carEffects = carEffects;
    }

    private void UpdateTireTrack()
    {
        if(carEffects)
        {
            if (lateralVelocity.magnitude >= tireTrackSpeedLimit)
            {
                carEffects.UpdateTireTrack(true);
            }
            else
            {
                carEffects.UpdateTireTrack(false);
            }
        }
    }

    public void ChangeDirection(float inputDirection)
    {
        if (inputDirection == 0)
        {
            acceleration = Mathf.Lerp(acceleration, accelerationMax, accelerationLerpFactor);
        }
        else
        {
            acceleration = Mathf.Lerp(acceleration, accelerationMin, accelerationLerpFactor);
        }

        steerDirection = Mathf.Lerp(steerDirection, (float)inputDirection, steerLerpFactor);

        // if (inputDirection == 0)
        // {
        //     acceleration = Mathf.Lerp(acceleration, accelerationMax, accelerationLerpFactor);
        // }
        // else
        // {
        //     acceleration = Mathf.Lerp(acceleration, accelerationMin, accelerationLerpFactor);
        // }

        // steerDirection = Mathf.Lerp(steerDirection, (float)inputDirection, steerLerpFactor);
    }

    void Movement()
    {
        // Vector2 input = WorldManager.instance.inputManager.GetInput();

        // Vector3 input3 = new Vector3(input.x, 0, input.y);

        Vector3 newPos = transform.position + input*maxSpeed*Time.fixedDeltaTime;
        Vector3 velocityVec = new Vector3(input.x,0,input.z);
        if (input!=Vector3.zero)
        {
            rb.velocity = velocityVec*maxSpeed;
            RotateTowards(input,Time.fixedDeltaTime,rotationSpeed);

        }
        else
        {
            RotateTowards(rb.velocity,Time.fixedDeltaTime,rotationSpeed);
        }
        
    }

    void RotateTowards(Vector3 direction, float deltaTime, float rotationSpeed)
    {
        Quaternion currRotation = transform.rotation;
        float signedAngle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);
        float rotateAngle = rotationSpeed * deltaTime;

        if (Mathf.Abs(signedAngle) >= rotateAngle)
        {
            transform.rotation = currRotation * Quaternion.Euler(Vector3.up * rotateAngle * Mathf.Sign(signedAngle));
        }
        else
        {
            transform.rotation = currRotation * Quaternion.Euler(Vector3.up * signedAngle);
        }
        // var targetAngle = Mathf.Atan2(input.x,input.z)*Mathf.Rad2Deg;
        // float currentVelocity;
        // currentVelocity = rb.velocity.magnitude;
        // var anglez = Mathf.SmoothDampAngle(transform.eulerAngles.y,targetAngle,ref currentVelocity,0.05f);
        // transform.rotation=Quaternion.Euler(0,targetAngle,0);

    }

    void StopRoutine()
    {
        if (stopRoutine!=null)
        {
            StopCoroutine(stopRoutine);
            stopRoutine=StartCoroutine(StopGradually());
        }
        else
        {
            stopRoutine=StartCoroutine(StopGradually());
        }
    }

    IEnumerator StopGradually()
    {
        isStop=true;
        while(rb.velocity.magnitude>=0.01f&&input.magnitude<=0)
        {
            rb.velocity*=.7f;
            yield return new WaitForSeconds(.1f);
        }
        
    }
    public void Accelerate(float deltaTime,float rate=1)
    {
        if (verticalSpeed >= 0)
        {
            if (input.magnitude>0)
            {
                isStop=false;
                rb.velocity = Vector3.Lerp(rb.velocity,rate*acceleration * deltaTime * (transform.forward)*input.magnitude*maxSpeed,Time.fixedDeltaTime);    
            }
            else
            {
                if (!isStop)
                {
                    StopRoutine();    
                }
                
            }
            
            // rb.velocity*= Mathf.Lerp(0,1,input.magnitude);
            // Vector3 velocityVec= new Vector3(input.x,0,input.y);
            // rb.velocity= input*maxSpeed;

            if (rb.velocity.magnitude >= maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }
        }
        else
        {
            ChangeDirection(0);
            rb.velocity += 2 * rate * acceleration * deltaTime * (transform.forward);
            // rb.velocity*=input.magnitude;
        }
    }

    private void Rotate()
    {
        maxTurnAngle = Mathf.Lerp(maxTurnAngleMin, maxTurnAngleMax, rb.velocity.magnitude / globalMaxSpeed); // * Mathf.Min(1f, rb.velocity.magnitude / globalMaxSpeed);

        float turnAngle = steerDirection * maxTurnAngle * Mathf.Sign(verticalSpeed);
        // rb.MoveRotation(transform.rotation * Quaternion.AngleAxis(turnAngle, transform.up));
        transform.rotation *= Quaternion.AngleAxis(turnAngle, transform.up);

        carDirection = transform.forward;

        // if (verticalSpeed>35f)
        // {
        //     maxTurnAngle = Mathf.Lerp(maxTurnAngleMax, maxTurnAngleMin, rb.velocity.magnitude / globalMaxSpeed)*Mathf.Min(1f,rb.velocity.magnitude / globalMaxSpeed);
        // }
        // else
        // {
        //     maxTurnAngle = maxTurnAngleMin;
        // }

        // float turnAngle = steerDirection * maxTurnAngle * Mathf.Sign(verticalSpeed);
        // carDirection = transform.forward;
    }

    private void Friction()
    {
        //if (rb.velocity.magnitude > driftSpeedPercentage * maxSpeed)
        //{
        //    lateralGrip = Mathf.Lerp(lateralGrip, lateralGripMin, lateralGripLerpFactor);
        //}
        //else
        //{
        //    lateralGrip = Mathf.Lerp(lateralGrip, lateralGripMax, lateralGripLerpFactor);
        //}

        lateralGrip = Mathf.Lerp(lateralGripMax, lateralGripMin, rb.velocity.magnitude / globalMaxSpeed);

        rb.velocity += Mathf.Min(lateralGrip * Time.fixedDeltaTime, lateralVelocity.magnitude) * lateralVelocity * -1;
    }
    
    public void Reverse()
    {
        if (verticalSpeed > 0.1f)
        {
            // rb.velocity += Mathf.Min(brakeAcceleration * StatsManager.instance.gradeSkillBrakeMultiplier * Time.fixedDeltaTime, verticalVelocity.magnitude) * transform.forward * -1;
            // ChangeDirection(0);
            rb.velocity = Vector3.zero;
            rb.AddForce(transform.forward * -1 * force, ForceMode.Acceleration);
            ChangeDirection(-1);
        }
        else
        {
            ChangeDirection(-1);

            rb.velocity += reverseAcceleration * Time.fixedDeltaTime * transform.forward * -1;

            if (rb.velocity.magnitude > maxReverseSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxReverseSpeed;
            }
        }
    }

    public Vector3 GetVelocity()
    {
        return rb.velocity;
    }

    public void UpdateCarMovementStats(CarStats carStats)
    {
        maxSpeed = carStats.maxSpeed;
        maxReverseSpeed = carStats.maxSpeed * 0.5f;

        brakeAcceleration = carStats.maxSpeed * 1.5f;
        reverseAcceleration = carStats.maxSpeed * 0.4f;

        accelerationMin = carStats.maxSpeed * 0.2f;
        accelerationMax = carStats.maxSpeed * 0.6f;
        // accelerationMax = acceleration + acceleration*AttackManager.instance.GetAccelerationMax();
       
        accelerationLerpFactor = (carStats.maxSpeed - minSpeedLimit) / (maxSpeedLimit - minSpeedLimit);

        lateralGripMin = Mathf.Lerp(lateralGripMinLimitMin, lateralGripMinLimitMax, Mathf.Pow(carStats.handling / 100f, 2f)) + 1.82f;
        lateralGripMax = lateralGripMin + 2.5f -0.5f;
    }

    public void UpdateLateralGrip(CarStats carStats)
    {
        lateralGripMin = Mathf.Lerp(lateralGripMinLimitMin, lateralGripMinLimitMax, Mathf.Pow(carStats.handling / 100f, 2f)) + 1.82f;
        lateralGripMax = lateralGripMin + 2.5f -.5f;
        maxSpeed = carStats.maxSpeed;
    }

    public float GetSpeedRatio()
    {
        return verticalSpeed / maxSpeed;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcCarMovement : MonoBehaviour
{
    [HideInInspector] public int spawnIndex = 0;

    private List <Transform> wayPoints = new List<Transform>();

    private Transform targetWayPoint;

    private NPC nPC;

    private int targetWayPointIndex = 0;
    private int lastWayPointIndex;

    private float minDistance = 50f;

    private float movementSpeed = 100f;
    private float rotationSpeed = 2f;
    private float maxSpeed = 60f;

    private bool gotWayPoints = false;

    Quaternion rotationToTarget;
    Rigidbody myBody;

    public void Setup()
    {
        wayPoints = WorldManager.instance.wayPoints;

        lastWayPointIndex = wayPoints.Count -1;

        gotWayPoints = true;

        nPC = gameObject.GetComponent<NPC>();
    }

    private void OnEnable()
    {
        if(!gotWayPoints)
            return;

        myBody = gameObject.GetComponent<Rigidbody>();

        targetWayPointIndex = spawnIndex + 1;

        UpdateTargetWayPoint();
    }

    private void FixedUpdate()
    {
        if (GameManager.isGameOver)
            return;

        if (!nPC.isDisabled)
        {
            float movementStep = movementSpeed * Time.fixedDeltaTime;
            float rotationStep = rotationSpeed * Time.fixedDeltaTime;

            Vector3 directionToTarget = targetWayPoint.position - transform.position;

            if(directionToTarget != Vector3.zero)
            {
                rotationToTarget = Quaternion.LookRotation(directionToTarget);
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, rotationToTarget, rotationStep);

            float distance = Vector3.Distance(transform.position, targetWayPoint.position);

            CheckDistanceToWayPoint(distance);

            myBody.velocity += transform.forward * movementStep;

            if(myBody.velocity.magnitude >= maxSpeed)
            {
                myBody.velocity = maxSpeed * transform.forward;
            }
        }
    }

    private void CheckDistanceToWayPoint(float currentDistance)
    {
        if(currentDistance <= minDistance)
        {
            targetWayPointIndex++;

            UpdateTargetWayPoint();
        }
    }

    private void UpdateTargetWayPoint()
    {
        if(targetWayPointIndex > lastWayPointIndex)
        {
            targetWayPointIndex = 0;
        }

        targetWayPoint = wayPoints[targetWayPointIndex];
    }
}

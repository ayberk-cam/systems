using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastController : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] Rigidbody rigidBody;

    private Vector3 newPos;
    private Vector3 moveVector;

    private void Start()
    {
        moveVector = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
    }

    private void FixedUpdate()
    {
        Move(Time.fixedDeltaTime);
    }

    public void Move(float deltaTime)
    {
        CheckRayCast();
        newPos = transform.position + moveVector.normalized * movementSpeed * deltaTime;
        rigidBody.MovePosition(newPos);
    }

    
    public void CheckRayCast()
    {
        int layerMask = 1 << 8;
        RaycastHit hit;
        //Debug.DrawRay(transform.position, new Vector3(0, 0, 1f) * 100, Color.green);
        //Debug.DrawRay(transform.position, new Vector3(0, 0, -1f) * 100, Color.white);
        //Debug.DrawRay(transform.position, new Vector3(1f, 0, 0) * 100, Color.cyan);
        //Debug.DrawRay(transform.position, new Vector3(-1f, 0, 0) * 100, Color.gray);

        if (Physics.Raycast(transform.position, new Vector3(0, 0, 1f), out hit, 5, layerMask))
        {
            //Debug.DrawRay(transform.position, new Vector3(0, 0, 1f) * 100, Color.magenta);
            if (moveVector.x > 0)
            {
                moveVector = new Vector3(moveVector.x + Random.Range(0, 0.5f), 0, -moveVector.z - Random.Range(0, 0.5f));
            }
            else
            {
                moveVector = new Vector3(moveVector.x - Random.Range(0, 0.5f), 0, -moveVector.z - Random.Range(0, 0.5f));
            }
            //return newVector;
        }
        else if (Physics.Raycast(transform.position, new Vector3(0, 0, -1f), out hit, 5, layerMask))
        {
            //Debug.DrawRay(transform.position, new Vector3(0, 0, -1f) * 100, Color.magenta);
            if (moveVector.x > 0)
            {
                moveVector = new Vector3(moveVector.x + Random.Range(0, 0.5f), 0, -moveVector.z + Random.Range(0, 0.5f));
            }
            else
            {
                moveVector = new Vector3(moveVector.x - Random.Range(0, 0.5f), 0, -moveVector.z + Random.Range(0, 0.5f));
            }
            //return newVector;
        }
        else if (Physics.Raycast(transform.position, new Vector3(1f, 0, 0), out hit, 5, layerMask))
        {
            //Debug.DrawRay(transform.position, new Vector3(1f, 0, 0) * 100, Color.magenta);
            if (moveVector.z < 0)
            {
                moveVector = new Vector3(-moveVector.x - Random.Range(0, 0.5f), 0, moveVector.z - Random.Range(0, 0.5f));
            }
            else
            {
                moveVector = new Vector3(-moveVector.x - Random.Range(0, 0.5f), 0, moveVector.z + Random.Range(0, 0.5f));
            }
            //return newVector;
        }
        else if (Physics.Raycast(transform.position, new Vector3(-1f, 0, 0), out hit, 5, layerMask))
        {
            //Debug.DrawRay(transform.position, new Vector3(-1f, 0, 0) * 100, Color.magenta);
            if(moveVector.z < 0)
            {
                moveVector = new Vector3(-moveVector.x + Random.Range(0, 0.5f), 0, moveVector.z - Random.Range(0, 0.5f));
            }
            else
            {
                moveVector = new Vector3(-moveVector.x + Random.Range(0, 0.5f), 0, moveVector.z + Random.Range(0, 0.5f));
            }
            //return newVector;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabController : MonoBehaviour
{
 public Transform pointA; // Reference to the first point
    public Transform pointB; // Reference to the second point
    public float speed = 2f; // Movement speed

    private Transform target; // Current target point
    private bool movingToB; // Flag to check direction 

    void Start()
    {
        // Set the initial target to point B
        target = pointB;
    }

    void Update()
    {
        // Move towards the target point
        MoveTowardsTarget();
    }

    void MoveTowardsTarget()
    {
        // Move the enemy towards the target point
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);

        // Check if the enemy has reached the target point
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            // Switch direction
            if (movingToB)
            {
                target = pointA;
            }
            else
            {
                target = pointB;
            }
            movingToB = !movingToB; // Toggle direction
        }
    }
}

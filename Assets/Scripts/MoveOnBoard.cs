using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnBoard : MonoBehaviour
{
    public Transform[] waypoints;
    private float speed = 50f;

    [HideInInspector]
    public int waypointIndex = 0;

    public bool moveAllowed = false;
    
    void Update()
    {
        if(moveAllowed)
        {
            Move();
        }
    }

    private void Move()
    {
        if(waypointIndex <= waypoints.Length - 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, waypoints[waypointIndex].transform.position, speed * Time.deltaTime);

            if((int)transform.position.x == (int)waypoints[waypointIndex].transform.position.x)
            {
                waypointIndex += 1;
            }
        }
    }
}

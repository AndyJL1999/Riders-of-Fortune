using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is attached to the 'PositionPointer' game object. (Found in the 'Map' game object.)
public class MoveOnBoard : MonoBehaviour
{
    public Transform[] waypoints;
    private float speed = 100f;

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
        if(waypointIndex <= waypoints.Length - 1)//move as long as there are waypoints
        {
            transform.position = Vector2.MoveTowards(transform.position, waypoints[waypointIndex].transform.position, speed * Time.deltaTime);

            if((int)transform.position.x == (int)waypoints[waypointIndex].transform.position.x)//check if player has reached certain point to continue moving
            {
                waypointIndex += 1;
            }
        }
    }
}

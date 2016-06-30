using UnityEngine;
using System.Collections;

public class PlatformScript : MonoBehaviour 
{
    private float moveSpeed = 1;    //Move speed
    private Vector3 start;          //Start position
    private Vector3 end;            //End position
    private int dir = 1;            //Direction it's currently moving in

    private RaycastHit hit;         //Information about what it collided with

	// Use this for initialization
	void Start () 
    {
        start = transform.position;                                             //Set start
        end = new Vector3(transform.position.x + 3, transform.position.y);      //Set end to be 3 to the right of start
	}
	
	// Update is called once per frame
	void Update () 
    {
        transform.Translate(moveSpeed * dir * Time.deltaTime, 0, 0);    //Move platform
        
        if (transform.position.x >= end.x) dir = -1;    //If it reaches the end, head back to the start
        if (transform.position.x <= start.x) dir = 1;   //If it reaches the start, head back to the end

        if (Physics.BoxCast(transform.position, new Vector3(0.8f, 0), Vector3.up, out hit, Quaternion.identity, 0.18f))     //If platform collides with something above it
        {
            hit.transform.SendMessage("xMove", moveSpeed * dir);    //Send movement message
        }
	}


}

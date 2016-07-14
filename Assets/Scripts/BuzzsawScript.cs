using UnityEngine;
using System.Collections;

public class BuzzsawScript : MonoBehaviour
{
    private float moveSpeed = 1;    //Move speed
    private float rotSpeed = 10;    //Rotation speed
    private Vector3 start;          //Start position
    private Vector3 end;            //End position
    private int dir = 1;            //Direction it's currently moving in

    private RaycastHit hit;         //Information about what it collided with

    // Use this for initialization
    void Start()
    {
        start = transform.position;                                             //Set start
        end = new Vector3(transform.position.x, transform.position.y + 2);      //Set end to be 2 above the start
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, moveSpeed * dir * Time.deltaTime, 0, Space.World);   //Move saw in relation to the world
        transform.Rotate(new Vector3 (0,0,1), rotSpeed);            //Rotate saw

        if (transform.position.y >= end.y) dir = -1;    //If it reaches the end, head back to the start
        if (transform.position.y <= start.y) dir = 1;   //If it reaches the start, head back to the end
    }
}

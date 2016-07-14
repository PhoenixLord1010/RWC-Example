using UnityEngine;
using System.Collections;

public class FireballScript : MonoBehaviour 
{
    private float moveSpeed = 0;    //Horizontal movement value
    private float jumpSpeed = 0;    //Vertical movement value

    private int direction;      //The direction it's travelling in
    private float timer;        //Timer until it gets destroyed

    private RaycastHit hit;     //Information about what the player collided with

	// Use this for initialization
	void Start () 
    {
        timer = Time.time;  //Assigned a starting time
	}
	
	// Update is called once per frame
	void Update () 
    {
        //Falling
        if (jumpSpeed > -5 && !isGrounded())    //If not at maximum fall speed and not grounded
        {
            jumpSpeed -= 0.2f;  //Fall faster
        }

        if (jumpSpeed < -5) jumpSpeed = -5;     //Cap fall speed
        
        //Movement
        for (int i = 0; i < 20; i++)    //Repeat this code 20 times
        {
            if (isGrounded() && hit.collider.gameObject.tag != "Player" && hit.collider.gameObject.tag != "Enemy" && !hit.collider.isTrigger)   //If it hits the ground and not the player, an enemy, or a trigger
            {
                jumpSpeed = 3;  //Bounce
            }

            if (Physics.SphereCast(transform.position, 0.1f, Vector3.left, out hit, 0.01f) && hit.collider.gameObject.tag != "Player" && hit.collider.gameObject.tag != "Enemy" && !hit.collider.isTrigger)     //If it hits a wall and not the player, an enemy, or a trigger
            {
                Destroy(gameObject);    //Destroy this object
            }
            if (Physics.SphereCast(transform.position, 0.1f, Vector3.right, out hit, 0.01f) && hit.collider.gameObject.tag != "Player" && hit.collider.gameObject.tag != "Enemy" && !hit.collider.isTrigger)    //If it hits a wall and not the player, an enemy, or a trigger
            {
                Destroy(gameObject);    //Destroy this object
            }

            transform.Translate(moveSpeed * 0.05f * Time.deltaTime, jumpSpeed * 0.05f * Time.deltaTime, 0);     //Move fireball
        }

        //Timer
        if (Time.time - timer > 5)  //If 5 seconds have passed
        {
            Destroy(gameObject);        //Destroy this object
        }
	}

    //Collision
    bool isGrounded()
    {
        return Physics.SphereCast(transform.position, 0.1f, Vector3.down, out hit, 0.01f);  //If it collides with something below it
    }

    //Player will call this function to set the direction (-1 or 1)
    void SetDirection(int dir)
    {
        direction = dir;    //Set the direction
    }

    //Player will call this function to set the speed
    void SetSpeed(float speed)
    {
        if (direction == 1) moveSpeed = (speed * 0.4f) + 3;     //Set movespeed by adding 40% of the player's speed
        else moveSpeed = (speed * 0.4f) - 3;
    }
}
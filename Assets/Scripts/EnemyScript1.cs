using UnityEngine;
using System.Collections;

public class EnemyScript1 : MonoBehaviour 
{
    private float moveSpeed = 1;    //Horizontal movement value
    private float jumpSpeed = 0;    //Vertical movement value 
    private int direction = 1;      //The direction the enemy is walking (-1 or 1)
    
    public Vector3 mousePos;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        //Falling
        if (jumpSpeed > -9 && !isGrounded())    //If not a maximum fall speed and not grounded
        {
            jumpSpeed -= 0.2f;      //Fall faster
        }
        
        for (int i = 0; i < 10; i++)    //Repeat the following code 10 times
        {
            if (Physics.BoxCast(transform.position, new Vector3(0, 0.15f), Vector3.left, Quaternion.identity, 0.16f))   //If colliding with something to the left
            {
                direction = 1;      //Start going right
            }
            if (Physics.BoxCast(transform.position, new Vector3(0, 0.15f), Vector3.right, Quaternion.identity, 0.16f))  //If colliding with something to the right
            {
                direction = -1;     //Start going left
            }

            if (!Physics.BoxCast(new Vector3(transform.position.x - 0.32f, transform.position.y), new Vector3(0.15f, 0), Vector3.down, Quaternion.identity, 0.16f) && isGrounded()) //If it reaches an edge on the left
            {
                direction = 1;      //Start going right
            }
            if (!Physics.BoxCast(new Vector3(transform.position.x + 0.32f, transform.position.y), new Vector3(0.15f, 0), Vector3.down, Quaternion.identity, 0.16f) && isGrounded()) //If it reaches an edge on the right
            {
                direction = -1;     //Start going left
            }
        
            if (jumpSpeed < 0 && isGrounded()) jumpSpeed = 0;   //Land  

            transform.Translate(moveSpeed * 0.1f * direction * Time.deltaTime, jumpSpeed * 0.1f * Time.deltaTime, 0);   //Move enemy
        }
	}

    bool isGrounded()
    {
        return Physics.BoxCast(transform.position, new Vector3(0.15f, 0), Vector3.down, Quaternion.identity, 0.16f);    //If player collides with something below it
    }

    //Collision with triggers
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Attack")  //If colliding with a projectile
        {
            Destroy(gameObject);        //Destroy
            Destroy(other.gameObject);  //Destroy projectile
        }
    }
}

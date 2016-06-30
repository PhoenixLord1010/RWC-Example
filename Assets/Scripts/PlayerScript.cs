using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
    private float xSpeed = 0;               //Horizontal movement value
    private float maxSpeed = 3;             //Horizontal movement cap
    private float accel = 0.12f;            //Acceleration value
    private float decel = 0.09f;            //Deceleration value
    private int isRight = 1;                //Which way is the player facing?
    
    private float ySpeed = 0;               //Vertical movement value
    private float jumpHeight = 7;          //Jump height
    private float gravity = 0.6f;           //Rate at which player falls
    private int jump = 1;                   //How many times can the player jump in midair?

    private int maxHealth = 5;              //Max health
    public Slider health;                   //The health bar itself

    public Transform prefab;                //Fireball prefab
    private Transform clone;                //Used to clone a prefab

    private RaycastHit hit;                 //Information about what the player collided with
    private float x;                        //Value for additional x movement (i.e. moving platforms)

    public int level;                       //The current level


	// Use this for initialization
	void Start ()
    {
        health.value = maxHealth;                   //Set health on start to max value
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Basic Movement
        if (Input.GetKey("a"))                              //If the "a" key is held down
        {
            if (!Physics.Raycast(transform.position, Vector3.left, 0.16f))      //If the player isn't colliding on the left
            {
                if (xSpeed > -maxSpeed) xSpeed -= accel;                      //Increase speed by acceleration value
                if (xSpeed < -maxSpeed) xSpeed = -maxSpeed;                   //Cap speed in case it goes over
            }
            isRight = 0;                                        //Player is now facing left
        }
        if (Input.GetKey("d"))                              //If the "d" key is held down
        {
            if (!Physics.Raycast(transform.position, Vector3.right, 0.16f))     //If the player isn't colliding on the right
            {
                if (xSpeed < maxSpeed) xSpeed += accel;                       //Increase speed by acceleration value
                if (xSpeed > maxSpeed) xSpeed = maxSpeed;                     //Cap speed in case it goes over
            }
            isRight = 1;                                        //Player is now facing right
        }
        
        
        // Jump
        if (Input.GetKeyDown("space"))          //If the "space" key is pressed down
        {
            if (castDown())                   //If the player is grounded
            {
                ySpeed = jumpHeight;            //Player begins jump
            }
            else
            {
                if (jump > 0)                   //If there are more jumps left
                {
                    ySpeed = jumpHeight;        //Player begins additional jump
                    jump--;                     //Jump count goes down
                }
            }
        }

        // Shoot Fireball
        if (Input.GetKeyDown("p"))          //If the "p" key is pressed down
        {
            clone = Instantiate(prefab, transform.position, transform.rotation) as Transform;   //Create a fireball prefab
            clone.SendMessage("SetDirection", isRight);         //Send the player's direction
            clone.SendMessage("SetSpeed", xSpeed);           //Send the player's speed
        }

        // Idle
        if (!Input.GetKey("a") && !Input.GetKey("d"))       //If neither "a" or "d" keys are held
        {
            if (xSpeed > 0) xSpeed -= decel;              //Start to slow down the player's movement
            if (xSpeed < 0) xSpeed += decel;              //Start to slow down the player's movement
            if (Mathf.Abs(xSpeed) < decel) xSpeed = 0;    //Stop the player's movement when it's slow enough
        }
        
        // Falling
        if (ySpeed > -9 && !castDown())        //If the player isn't grounded and has yet to reach it's maximum fall speed
        {
            if (Input.GetKey("space") && ySpeed > 0)         //If the "space" key is held and the player is rising
            {
                ySpeed -= gravity * 0.5f;        //Player drops based on half of gravity 
            }
            else
            {
                ySpeed -= gravity;               //Player drops based on gravity
            }
        }


        //Collision stuff
        for (int i=0; i < 20; i++)
        {
            if (castUp() && ySpeed > 0)     //If player collides with something above it
            {
                ySpeed = 0.5f;          //Slow down the jump speed

                if (hit.transform.tag == "Brick")
                {
                    Destroy(hit.transform.gameObject);
                }
            }
            if (castDown() && ySpeed < 0)   //If player collides with something below it
            {
                ySpeed = 0;             //Player's Y movement halts
                jump = 1;               //Amount of aerial jumps resets
            }
            if (castLeft() && xSpeed < 0)   //If player collides with something to the left
            {
                xSpeed = 0;             //Stop moving to the side
            }
            if (castRight() && xSpeed > 0)  //If player collides with something to the right
            {
                xSpeed = 0;             //Stop moving to the side
            }

            //Movement
            transform.Translate((xSpeed + x) * 0.05f * Time.deltaTime, ySpeed * 0.05f * Time.deltaTime, 0);   //Move player
        }


        //Dead
        if (health.value == 0)                  //If health is zero
        {
            SceneManager.LoadScene(level);      //Reload level
        }

        x = 0;  //Reset additional x value
	}


    bool castUp()
    {
        return Physics.BoxCast(transform.position, new Vector3(0.15f, 0), Vector3.up, out hit, Quaternion.identity, 0.16f, 1, QueryTriggerInteraction.Ignore);      //If player collides with something above it
    }

    bool castDown()
    {
        return Physics.BoxCast(transform.position, new Vector3(0.15f, 0), Vector3.down, out hit, Quaternion.identity, 0.16f, 1, QueryTriggerInteraction.Ignore);    //If player collides with something below it
    }

    bool castLeft()
    {
        return Physics.BoxCast(transform.position, new Vector3(0, 0.15f), Vector3.left, out hit, Quaternion.identity, 0.16f, 1, QueryTriggerInteraction.Ignore);    //If player collides with something left of it
    }

    bool castRight()
    {
        return Physics.BoxCast(transform.position, new Vector3(0, 0.15f), Vector3.right, out hit, Quaternion.identity, 0.16f, 1, QueryTriggerInteraction.Ignore);   //If player collides with something right of it
    }


    void xMove(float i)         //Moving platforms and the like will call this and send x velocity
    {   
        x = i;              //Set additional x velocity
    }
    

    //Collision with triggers
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Health")         //If colliding with a health pickup and the health counter has reset
        {
            if(health.value < maxHealth) health.value++;        //If the healthbar isn't full, increase health
            Destroy(other.gameObject);                          //Destroy the health pickup
        }

        if (other.tag == "Pit")                             //If colliding with a pit
        {
            SceneManager.LoadScene(level);                      //Reload level
        }

        if (other.name == "Goal")                           //If colliding with the goal
        {
            SceneManager.LoadScene(level + 1);                  //Load next level
        }
    }

    //Collision with non-triggers
    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Enemy")     //If colliding with Enemy
        {
            if (castDown())
            {
                if (hit.transform.gameObject == other.gameObject)
                {
                    ySpeed = jumpHeight;                             //Player begins jump
                    Destroy(hit.transform.gameObject);               //Destroy enemy
                }
                else
                {
                    OnHit(other.transform.position);
                }
            }
            else
            {
                OnHit(other.transform.position);
            }
        }
    }

    void OnHit(Vector3 pos)
    { 
        health.value--;                         //Lose health

        if (pos.x - transform.position.x > 0)   //If player is to the left of the enemy
        {
            xSpeed = -2;     //Bump player over to the left
        }
        else
        {
            xSpeed = 2;      //Bump player over to the right
        }
        ySpeed = 2;          //Pop player into the air
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
    private float moveSpeed = 0;
    private float maxSpeed = 3;
    private float accel = 0.12f;
    private float decel = 0.09f;
    private int isRight = 1;
    
    private float jumpSpeed = 0;
    private float jumpHeight = 2.4f;
    private int jump = 1;

    private int healthbar = 5;
    public Slider health;
    private int healthCt = 0;

    public Transform prefab;
    private Transform clone;

    private int level = 0;

	// Use this for initialization
	void Start ()
    {
        health.value = 3;
	}
	
	// Update is called once per frame
	void Update ()
    {
        DontDestroyOnLoad(gameObject);

        if (healthCt > 0) healthCt--;
        
        //Basic Movement
        if (Input.GetKey("a"))
        {
            if (!Physics.Raycast(transform.position, Vector3.left, 0.16f))
            {
                if (moveSpeed > -maxSpeed) moveSpeed -= accel;
                if (moveSpeed < -maxSpeed) moveSpeed = -maxSpeed;
            }
            isRight = 0;
        }
        if (Input.GetKey("d"))
        {
            if (!Physics.Raycast(transform.position, Vector3.right, 0.16f))
            {
                if (moveSpeed < maxSpeed) moveSpeed += accel;
                if (moveSpeed > maxSpeed) moveSpeed = maxSpeed;
            }
            isRight = 1;
        }
        
        
        //Jump
        if (Input.GetKeyDown("space"))
        {
            if (isGrounded())
            {
                jumpSpeed = jumpHeight;
            }
            else
            {
                if (jump > 0)
                {
                    jumpSpeed = jumpHeight;
                    jump--;
                }
            }
        }

        //Shoot Fireball
        if (Input.GetKeyDown("p"))
        {
            clone = Instantiate(prefab, transform.position, transform.rotation) as Transform;
            clone.SendMessage("SetDirection", isRight);
            clone.SendMessage("SetSpeed", moveSpeed);
        }

        //Idle
        if (!Input.GetKey("a") && !Input.GetKey("d"))
        {
            if (moveSpeed > 0) moveSpeed -= decel;
            if (moveSpeed < 0) moveSpeed += decel;
            if (Mathf.Abs(moveSpeed) < decel) moveSpeed = 0;
        }
        
        //Falling
        if (jumpSpeed > -9 && !isGrounded())
        {
            if (Input.GetKey("space") && jumpSpeed > 0)
            {
                jumpSpeed -= 0.04f;
            }
            else
            {
                jumpSpeed -= 0.08f;
            }
        }


        for (int i=0; i < 20; i++)
        {
            if (jumpSpeed < 0 && isGrounded())  //Landed
            {
                jumpSpeed = 0;
                jump = 1;
            }
            if (Physics.BoxCast(transform.position, new Vector3(0.15f, 0), Vector3.up, Quaternion.identity, 0.16f, 1, QueryTriggerInteraction.Ignore) && jumpSpeed > 0) jumpSpeed = 0.2f;
            if (Physics.BoxCast(transform.position, new Vector3(0, 0.15f), Vector3.left, Quaternion.identity, 0.16f, 1, QueryTriggerInteraction.Ignore) && moveSpeed < 0) moveSpeed = 0;
            if (Physics.BoxCast(transform.position, new Vector3(0, 0.15f), Vector3.right, Quaternion.identity, 0.16f, 1, QueryTriggerInteraction.Ignore) && moveSpeed > 0) moveSpeed = 0;

            //Movement
            transform.Translate(moveSpeed * 0.05f * Time.deltaTime, jumpSpeed * 0.05f * Time.fixedDeltaTime, 0, Space.World);
        }
	}

    bool isGrounded()
    {
        return Physics.BoxCast(transform.position, new Vector3(0.15f, 0), Vector3.down, Quaternion.identity, 0.16f, 1, QueryTriggerInteraction.Ignore);
        //return Physics.Raycast(transform.position, Vector3.down, 0.16f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Health" && healthCt == 0)
        {
            health.value++;
            healthCt = 5;
            Destroy(other.gameObject);
        }
        
        if (other.tag == "Enemy" && jumpSpeed <= 0)
        {
            jumpSpeed = jumpHeight;
            Destroy(other.gameObject);
        }
        
        if (other.tag == "Brick" && jumpSpeed > 0)
        {
            Destroy(other.gameObject);
        }

        if (other.name == "Goal")
        {
            level++;
            transform.position = new Vector3(0, 0, 0);
            SceneManager.LoadScene(level);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Enemy")
        {
            health.value--;
            if (other.transform.position.x - transform.position.x > 0) moveSpeed = -3;
            else moveSpeed = 3;
            jumpSpeed = 3;
        }
    }
}

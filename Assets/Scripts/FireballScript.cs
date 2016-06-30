using UnityEngine;
using System.Collections;

public class FireballScript : MonoBehaviour 
{
    private float moveSpeed = 0;
    private float jumpSpeed = 0;

    private int direction;
    private float timer;

    private RaycastHit hit;

	// Use this for initialization
	void Start () 
    {
        timer = Time.time;
	}
	
	// Update is called once per frame
	void Update () 
    {
        //Falling
        if (jumpSpeed > -5 && !isGrounded())
        {
            jumpSpeed -= 0.2f;
        }

        if (jumpSpeed < -5) jumpSpeed = -5;
        
        //Movement
        for (int i = 0; i < 20; i++)
        {
            if (isGrounded() && hit.collider.gameObject.tag != "Player" && hit.collider.gameObject.tag != "Enemy" && !hit.collider.isTrigger)
            {
                jumpSpeed = 4;
            }

            if (Physics.SphereCast(transform.position, 0.1f, Vector3.left, out hit, 0.01f) && hit.collider.gameObject.tag != "Player" && hit.collider.gameObject.tag != "Enemy" && !hit.collider.isTrigger) Destroy(gameObject);
            if (Physics.SphereCast(transform.position, 0.1f, Vector3.right, out hit, 0.01f) && hit.collider.gameObject.tag != "Player" && hit.collider.gameObject.tag != "Enemy" && !hit.collider.isTrigger) Destroy(gameObject);

            transform.Translate(moveSpeed * 0.05f * Time.deltaTime, jumpSpeed * 0.05f * Time.deltaTime, 0);
        }

        //Timer
        if (Time.time - timer > 5) Destroy(gameObject);
	}

    bool isGrounded()
    {
        return Physics.SphereCast(transform.position, 0.1f, Vector3.down, out hit, 0.01f);
    }

    void SetDirection(int dir)
    {
        direction = dir;
    }

    void SetSpeed(float speed)
    {
        if (direction == 1) moveSpeed = (speed * 0.4f) + 3;
        else moveSpeed = (speed * 0.4f) - 3;
    }
}
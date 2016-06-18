using UnityEngine;
using System.Collections;

public class EnemyScript1 : MonoBehaviour 
{
    private int moveSpeed = 1;
    private int direction = 1;
    private float jumpSpeed = 0;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        //Falling
        if (jumpSpeed > -9 && !isGrounded())
        {
            jumpSpeed -= 0.2f;
        }
        
        if (Physics.BoxCast(transform.position, new Vector3(0, 0.15f), Vector3.left, Quaternion.identity, 0.16f)) direction = 1;
        if (Physics.BoxCast(transform.position, new Vector3(0, 0.15f), Vector3.right, Quaternion.identity, 0.16f)) direction = -1;

        for (int i = 0; i < 10; i++)
        {
            if (jumpSpeed < 0 && isGrounded()) jumpSpeed = 0;

            transform.Translate(moveSpeed * 0.1f * direction * Time.deltaTime, jumpSpeed * 0.1f * Time.deltaTime, 0);
        }
	}

    bool isGrounded()
    {
        return Physics.BoxCast(transform.position, new Vector3(0.15f, 0), Vector3.down, Quaternion.identity, 0.16f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Attack")
        {
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
    }
}

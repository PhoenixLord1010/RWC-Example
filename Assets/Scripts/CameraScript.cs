using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour 
{
    private GameObject player;      //The Player game object
    private float moveX;            //Horizontal movement value
    private float moveY;            //Vertical movement value

	// Use this for initialization
	void Start () 
    {
        player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () 
    {
        for (int i = 0; i < 50; i++)    //Repeat the following code 50 times
        {
            if (player.transform.position.x > transform.position.x + 1)     //If player passes the right boundary
            {
                moveX = 1;      //Move the camera to the right
            }
            else
            {
                if (player.transform.position.x < transform.position.x - 1)     //If the player passes the left boundary
                {
                    moveX = -1;     //Move the camera to the right
                }
                else
                {
                    moveX = 0;      //Stop moving the camera
                }
            }

            if (player.transform.position.y > transform.position.y + 1)     //If the player passes the top boundary
            {
                moveY = 1;      //Move the camera up
            }
            else
            {
                if (player.transform.position.y < transform.position.y - 1)     //If the player passes the lower boundary
                {
                    moveY = -2;     //Move the camera down
                }
                else
                {
                    moveY = 0;      //Stop moving the camera
                }
            }
        
            transform.Translate(0.1f * moveX * Time.deltaTime, 0.1f * moveY * Time.deltaTime, 0);   //Move the camera
        }
	}
}

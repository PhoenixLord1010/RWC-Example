using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour 
{
    private GameObject player;
    private float moveX;
    private float moveY;
    private Camera cam;

	// Use this for initialization
	void Start () 
    {
        player = GameObject.Find("Player");
        cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        for (int i = 0; i < 30; i++)
        {
            if (player.transform.position.x > transform.position.x + 1)
            {
                moveX = 1;
            }
            else if (player.transform.position.x < transform.position.x - 1)
            {
                moveX = -1;
            }
            else
            {
                moveX = 0;
            }
        
            transform.Translate(0.1f * moveX * Time.deltaTime, 0, 0);
        }
	}
}

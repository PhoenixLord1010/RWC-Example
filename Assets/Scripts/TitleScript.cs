using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleScript : MonoBehaviour
{
    public int ct = 90;     
    public Renderer rend;
    
    // Use this for initialization
	void Start ()
    {
        rend = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (ct > 0)
        {
            ct--;
        }
        else
        {
            ct = 90;
        }

        if (ct > 20)
        {
            rend.enabled = true;    //Make it visible
        }
        else
        {
            rend.enabled = false;   //Make it invisible
        }

        
        if (Input.GetKeyDown("return"))     //If "enter" is pressed down
        {
            SceneManager.LoadScene(1);      //Load next scene
        }
	}
}

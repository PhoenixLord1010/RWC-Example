using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleScript : MonoBehaviour
{
	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown("return"))     //If "enter" is pressed down
        {
            SceneManager.LoadScene(1);      //Load next scene
        }
	}
}

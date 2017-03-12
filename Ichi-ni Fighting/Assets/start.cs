using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class start : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject.Find("Loading").GetComponent<SpriteRenderer>().enabled = true;
            SceneManager.LoadScene("Ichi-ni Fighting");
        }
	}
}

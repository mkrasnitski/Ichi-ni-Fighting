using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class init : MonoBehaviour {
    GameObject f;
    Camera cam;
    float camWidth;
    float xScale;
    float yScale;
    float zScale;

	void Start () {
        f = GameObject.Find("floor");
        cam = Camera.main;
        camWidth = cam.orthographicSize * cam.aspect / 2;
        xScale = f.transform.localScale.x;
        yScale = f.transform.localScale.y;
        zScale = f.transform.localScale.z;
	}
    private void Update()
    {
        camWidth = cam.orthographicSize * cam.aspect / 2;
        GameObject.Find("floor").transform.localScale = new Vector3(camWidth / xScale, yScale, zScale);
        string w = getWinner();
        if(w != "")
        {
            SceneManager.LoadScene("Start");
        }
    }

    private string getWinner()
    {
        string w = "";
        for(int i = 1; i < 3; i++)
        {
            if(w == "")
            {
                w = GameObject.Find("player" + i).GetComponent<master>().winner;
            }
        }
        return w;
    }
}

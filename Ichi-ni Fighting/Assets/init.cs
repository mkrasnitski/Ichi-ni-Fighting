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
    public string winner;
    public bool stop = false;
    public bool doneAnimations = true;
    static int numRounds = 3;
    static int maxWins = numRounds / 2 + 1;
    int p1Wins = 0;
    int p2Wins = 0;

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
        if(winner != "")
        {
            switch (winner.Trim("player".ToCharArray()))
            {
                case "1":
                    p1Wins++;
                    if(doneAnimations) stop = true;
                    break;
                case "2":
                    p2Wins++;
                    if(doneAnimations) stop = true;
                    break;
            }
            if(p1Wins == maxWins || p2Wins == maxWins)
            {
                SceneManager.LoadScene("Start");
            }
            winner = "";
        }
    }
}

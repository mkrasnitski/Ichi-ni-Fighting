using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class init : MonoBehaviour {
    GameObject f;
    GameObject can;
    Transform t1;
    Transform t2;
    GameObject h1;
    GameObject h2;
    Camera cam;
    float camWidth;
    float canWidth;
    float canHeight;
    float canXOffset = 200;
    float canYOffset = 175;
    float hWidth;
    float hHeight;
    float hY;
    float xScale;
    float yScale;
    float zScale;
    public string winner;
    public bool stop = false;
    public bool doneAnimations = true;
    static int numRounds = 3;
    static int maxWins = numRounds / 2 + 1;
    static int p1Wins = 0;
    static int p2Wins = 0;

	void Start ()
    {
        f = GameObject.Find("floor");
        cam = Camera.main;
        can = GameObject.Find("Canvas");
        t1 = can.transform.Find("Player1Score");
        t2 = can.transform.Find("Player2Score");
        h1 = GameObject.Find("healthBarBorder1");
        h2 = GameObject.Find("healthBarBorder2");
        hWidth = h1.GetComponent<SpriteRenderer>().bounds.size.x;
        hY = h1.transform.position.y - h1.GetComponent<SpriteRenderer>().bounds.size.y + 0.05f;
        camWidth = cam.orthographicSize * cam.aspect / 2;
        xScale = f.transform.localScale.x;
        yScale = f.transform.localScale.y;
        zScale = f.transform.localScale.z;
	}
    private void Update()
    {
        camWidth = cam.orthographicSize * cam.aspect / 2;
        GameObject.Find("floor").transform.localScale = new Vector3(camWidth / xScale, yScale, zScale);
        t1.position = new Vector3(h1.transform.position.x + (hWidth + 1f) / 2 - 0.2f, hY, 0);
        t2.position = new Vector3(h2.transform.position.x - (hWidth + 1f) / 2, hY, 0);
        t1.GetComponent<Text>().text = p1Wins.ToString();
        t2.GetComponent<Text>().text = p2Wins.ToString();

        if (winner != "")
        {
            switch (winner.Trim("player".ToCharArray()))
            {
                case "1":
                    if (doneAnimations)
                    {
                        p1Wins++;
                        endRound();
                    }
                    break;
                case "2":
                    if (doneAnimations)
                    {
                        p2Wins++;
                        endRound();
                    }
                    break;
            }
            winner = "";
        }
    }

    private void endRound()
    {
        stop = true;
        if (p1Wins == maxWins || p2Wins == maxWins)
        {
            SceneManager.LoadScene("Start");
        }
        else
        {
            SceneManager.LoadScene("Ichi-ni Fighting");
        }
    }
}

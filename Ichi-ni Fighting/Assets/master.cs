using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class master : MonoBehaviour {

    Character c;
    Rigidbody2D rb;
    Animator anim;
    Sprite[] subSprites;
    string p1_name = "player1";
    string p2_name = "player2";

    void Start ()
    { 
        rb = GetComponent<Rigidbody2D>();   
        anim = GetComponent<Animator>();
        PolygonCollider2D[] pc = gameObject.GetComponents<PolygonCollider2D>();
        c = new Character(8f, 27.5f, 3, rb, anim, name, pc);
        //GameObject.Find("Main Camera").GetComponent<Camera>().pixelRect = new Rect(0, 0, 500, 1028);

        Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), GameObject.Find("player2").GetComponent<CircleCollider2D>());
        Physics2D.IgnoreCollision(GetComponent<PolygonCollider2D>(), GameObject.Find("player2").GetComponent<PolygonCollider2D>());
        Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), GameObject.Find("player2").GetComponent<PolygonCollider2D>());
        Physics2D.IgnoreCollision(GetComponent<PolygonCollider2D>(), GameObject.Find("player2").GetComponent<CircleCollider2D>());
        Physics2D.IgnoreCollision(GetComponent<PolygonCollider2D>(), GameObject.Find("floor").GetComponent<BoxCollider2D>());

        if (name == p1_name)
        {
            c = new Character(new KeyCode[] { KeyCode.W, KeyCode.S,
                                              KeyCode.A, KeyCode.D,
                                              KeyCode.G, KeyCode.H }, c);
        }
        else if (name == p2_name)
        {
            c = new Character(new KeyCode[] { KeyCode.UpArrow,   KeyCode.DownArrow,
                                              KeyCode.LeftArrow, KeyCode.RightArrow,
                                              KeyCode.Keypad1,   KeyCode.Keypad2 }, c);
        }
    }

    void Update()
    {
        c.pollInput();
    }

    void FixedUpdate ()
    {
        c.move();
        c.attack();
        c.advanceFrame();
    }

    void LateUpdate()
    {
        c.hurtboxUpdate();
    }
}

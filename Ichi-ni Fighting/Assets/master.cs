using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class master : MonoBehaviour {

    Character c;
    Rigidbody2D rb;
    Animator anim;
    string p1_name = "player1";
    string p2_name = "player2";
    bool hit = false;
    float camWidth;

    public float currentDamage;

    void Start ()
    { 
        rb = GetComponent<Rigidbody2D>();   
        anim = GetComponent<Animator>();
        camWidth = 2f * Camera.main.orthographicSize * Camera.main.aspect;
        PolygonCollider2D[] pc = transform.Find("Hurt").GetComponents<PolygonCollider2D>();
        BoxCollider2D[] bc = transform.Find("Hit").GetComponents<BoxCollider2D>();
        c = new Character(8f, 27.5f, 3, rb, anim, name, pc, bc);

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

    void IgnoreCollisions()
    {
        Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), GameObject.Find("player2").GetComponent<CircleCollider2D>());
        Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), GameObject.Find("player2").GetComponent<PolygonCollider2D>());
        Physics2D.IgnoreCollision(GetComponent<PolygonCollider2D>(), GameObject.Find("player2").GetComponent<PolygonCollider2D>());
        Physics2D.IgnoreCollision(GetComponent<PolygonCollider2D>(), GameObject.Find("player2").GetComponent<CircleCollider2D>());
        Physics2D.IgnoreCollision(GetComponent<PolygonCollider2D>(), GameObject.Find("floor").GetComponent<BoxCollider2D>());
    }

    void Update()
    {
        c.pollInput();
        IgnoreCollisions();
    }

    void FixedUpdate ()
    {
        currentDamage = c.attack();
        ClampMovement();
        c.move();
        c.doDamage();
        c.advanceFrame();
    }

    void LateUpdate()
    {
        c.boxUpdate();
    }    

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision is BoxCollider2D && !hit)
        {
            c.ishit();
        }
    }

    void ClampMovement()
    {
        float limit = camWidth / 2 - 1;
        GetComponent<Transform>().position = new Vector3(Mathf.Clamp(transform.position.x, -limit, limit), transform.position.y, transform.position.z);
    }
}

using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class master : MonoBehaviour {

    Character c;
    Rigidbody2D rb;
    Animator anim;
    PolygonCollider2D[] pc;
    BoxCollider2D[] bc;
    string p1_name = "player1";
    string p2_name = "player2";
    float camWidth;

    public float currentDamage;
    public bool canHit;
    public bool hitStun;
    public string winner;

    void Start ()
    { 
        rb = GetComponent<Rigidbody2D>();   
        anim = GetComponent<Animator>();
        camWidth = 2f * Camera.main.orthographicSize * Camera.main.aspect;
        canHit = true;
        pc = transform.Find("Hurt").GetComponents<PolygonCollider2D>();
        bc = transform.Find("Hit").GetComponents<BoxCollider2D>();
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
        winner = c.checkWin();
        if (hitStun) c.hitStun(5);
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
        if (canHit)
        {
            hitStun = true;
        }
        //Is hit
        if (collision is BoxCollider2D)
        {
            c.ishit();
            canHit = false;
        }
        //Is hitting
        if(collision is PolygonCollider2D)
        {
            if (canHit)
            {
                var hitCircle = GameObject.Find("hitCircle");
                foreach (var hitbox in bc)
                {
                    if (hitbox.enabled)
                    {
                        Vector3 p = hitbox.transform.position;
                        hitCircle.transform.position = hitbox.bounds.center + new Vector3((-2*c.getOrientation()+1)*hitbox.bounds.extents.x / 2, 0, 0);
                    }
                }
                hitCircle.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
    }

    void ClampMovement()
    {
        camWidth = 2f * Camera.main.orthographicSize * Camera.main.aspect;
        float limit = camWidth / 2 - 1;
        GetComponent<Transform>().position = new Vector3(Mathf.Clamp(transform.position.x, -limit, limit), transform.position.y, transform.position.z);
    }
}

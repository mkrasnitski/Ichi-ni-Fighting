using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class master : MonoBehaviour {

    Character c;
    Rigidbody2D rb;
    Animator anim;

    void Start () {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        c = new Character(8f, 35f, 3, rb, anim, name);
        PolygonCollider2D[] pc = gameObject.GetComponents<PolygonCollider2D>();
        foreach (PolygonCollider2D p in pc)
        {
            //print(p.points.Length);
        }
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), GameObject.Find("player2").GetComponent<BoxCollider2D>());
        if(name == "player1")
        {
            c = new Character(new KeyCode[] { KeyCode.W,
                                              KeyCode.S,
                                              KeyCode.A,
                                              KeyCode.D,
                                              KeyCode.G,
                                              KeyCode.H }, c);
        }
        else if (name == "player2")
        {
            c = new Character(new KeyCode[] { KeyCode.UpArrow,
                                              KeyCode.DownArrow,
                                              KeyCode.LeftArrow,
                                              KeyCode.RightArrow,
                                              KeyCode.Keypad1,
                                              KeyCode.Keypad2 }, c);
        }
    }

    void Update()
    {
        c.pollInput();
    }

    void FixedUpdate () {
        run();
    }

    void run()
    {
        c.move();
        c.attack();
        c.advanceFrame();
    }
}

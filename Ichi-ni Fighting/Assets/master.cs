﻿using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class master : MonoBehaviour {

    Character c;
    Rigidbody2D rb;
    Animator anim;
    Sprite[] subSprites;

    void Start () {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        PolygonCollider2D[] pc = gameObject.GetComponents<PolygonCollider2D>();

        c = new Character(8f, 27.5f, 3, rb, anim, name, pc);

        subSprites = Resources.LoadAll<Sprite>("Characters/"+name+"_Sprites"); ;
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), GameObject.Find("player2").GetComponent<BoxCollider2D>());

        switch (name)
        {
            case "player1":
                c = new Character(new KeyCode[] { KeyCode.W,
                                                  KeyCode.S,
                                                  KeyCode.A,
                                                  KeyCode.D,
                                                  KeyCode.G,
                                                  KeyCode.H }, c);
                break;
            case "player2":
                c = new Character(new KeyCode[] { KeyCode.UpArrow,
                                                  KeyCode.DownArrow,
                                                  KeyCode.LeftArrow,
                                                  KeyCode.RightArrow,
                                                  KeyCode.Keypad1,
                                                  KeyCode.Keypad2 }, c);
                break;
        }
    }

    void Update()
    {
        c.pollInput();
    }

    void FixedUpdate () {
        c.move();
        c.attack();
        c.advanceFrame();
    }

    void LateUpdate()
    {
        c.hurtboxUpdate(this);
    }

    //private void LateUpdate()
    //{
    //    foreach (SpriteRenderer r in GetComponentsInChildren<SpriteRenderer>())
    //    {
    //        var newSprite = Array.Find(subSprites, item => item.name == r.sprite.name);
    //        print(newSprite);
    //        if (newSprite)
    //        {
    //            r.sprite = newSprite;
    //            print("asdf");
    //        }
    //    }
    //}
}

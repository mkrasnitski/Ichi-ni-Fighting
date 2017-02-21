using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Character : MonoBehaviour
{
    //Movement Vars
    private float jumpspeed;
    private float walkspeed;
    private float jumpMult = 1.5f;
    private int jumpsquat = 3;
    private bool canJump = true;
    private string player;
    private int orientation;
    private string state;

    //Attack Vars
    private string[] states = {"idleR", "idleL",
                               "walkR", "walkL",
                               "jumpR", "jumpL",
                               "squatR", "squatL",
                               "punchGroundR", "punchGroundL",
                               "punchAirR", "punchAirL",
                               "punchSquatR", "punchSquatL",
                               "kickGroundR", "kickGroundL",
                               "kickAirR", "kickAirL",
                               "kickSquatR", "kickSquatL"};
    private PolygonCollider2D[] hurtboxes = { };
    private BoxCollider2D[] hitboxes = { };
    
    //Framecounters
    public int framecounter = 0;
    private int count = 0;

    //Controls
    private KeyCode up = KeyCode.W;
    private KeyCode down = KeyCode.S;
    private KeyCode left = KeyCode.A;
    private KeyCode right = KeyCode.D;
    private KeyCode punch = KeyCode.G;
    private KeyCode kick = KeyCode.H;

    //Moves
    private Move punchGround = new Move(2, 4, 15);
    private Move punchAir = new Move(2, 4, 10);
    private Move punchSquat = new Move(2, 4, 5);
    private Move kickGround = new Move(2, 4, 20);
    private Move kickAir = new Move(2, 4, 15);
    private Move kickSquat = new Move(2, 4, 10);

    //General
    Rigidbody2D rb;
    Animator anim;

    public Character()
    {
        walkspeed = 5f;
        jumpspeed = 35f;
        jumpsquat = 3;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        hurtboxes = gameObject.GetComponents<PolygonCollider2D>();
    }

    public Character(float w, float j, int js, Rigidbody2D r, Animator a, string p, PolygonCollider2D[] h)
    {
        walkspeed = w;
        jumpspeed = j;
        jumpsquat = js;
        rb = r;
        anim = a;
        player = p;
        hurtboxes = h;
    }

    public Character(KeyCode[] k, Character c)
    {
        walkspeed = c.walkspeed;
        jumpspeed = c.jumpspeed;
        jumpsquat = c.jumpsquat;
        rb = c.rb;
        anim = c.anim;
        player = c.player;
        hurtboxes = c.hurtboxes;
        
        if (k.Length == 6)
        {
            up = k[0];
            down = k[1];
            left = k[2];
            right = k[3];
            punch = k[4];
            kick = k[5];
        }
    }

    private bool isLow(float f)
    {
        float threshold = 0.005f;
        return Mathf.Abs(f) <= threshold;
    }

    public void pollInput()
    {
        if (canJump)
        {
            if (Input.GetKeyDown(up) && Input.GetKey(left))
            {
                state = "jumpLeft";
                canJump = false;
            }
            if (Input.GetKeyDown(up) && Input.GetKey(right))
            {
                state = "jumpRight";
                canJump = false;
            }
            if (Input.GetKeyDown(up) && isLow(rb.velocity[0]))
            {
                state = "jump";
                canJump = false;
            }
        }
        
        if (Input.GetKeyDown(punch))
        {
            state = "punch";
        }
        if (Input.GetKeyDown(kick))
        {
            state = "kick";
        }

        double p = -Convert.ToDouble(player.Trim("player".ToCharArray()))+3;
        if (GameObject.Find(player).transform.position.x <= GameObject.Find("player" + p).transform.position.x)
        {
            orientation = 0;
        }
        else
        {
            orientation = 1;
        }
    }

    public void move()
    {
        //Left Jump
        if (state == "jumpLeft")
        {
            rb.velocity = new Vector2(-jumpMult*walkspeed, jumpspeed);
            anim.SetBool("jump", true);
        }

        //Right Jump
        if (state == "jumpRight")
        {
            rb.velocity = new Vector2(jumpMult*walkspeed, jumpspeed);
            anim.SetBool("jump", true);
        }

        //Vertical Jump
        if (state == "jump")
        {
            rb.velocity = new Vector2(0f, jumpspeed);
            anim.SetBool("jump", true);
        }
        anim.SetInteger("walk", orientation);

        if (canJump && !(anim.GetBool("punch") || anim.GetBool("kick")))
        {
            rb.velocity = new Vector2(0f, 0f);

            //Squat
            if (Input.GetKey(down))
            {
                anim.SetInteger("walk", 4 + anim.GetInteger("walk") % 2);
            }
            else
            {
                //Walk Left
                if (Input.GetKey(left))
                {
                    rb.velocity = new Vector2(-walkspeed, 0f);
                    anim.SetInteger("walk", 2+orientation);
                }

                //Walk Right
                if (Input.GetKey(right))
                {
                    rb.velocity = new Vector2(walkspeed, 0f);
                    anim.SetInteger("walk", 2+orientation);
                }
            }
        }
        
        //Single Jump only
        if (isLow(rb.velocity[1]))
        {
            canJump = true;
            anim.SetBool("jump", false);
        }
    }

    public void attack()
    {
        Move move = null;
        string moveString = "";
        bool a = anim.GetBool("punch") && anim.GetBool("kick");
        if (anim.GetBool("punch"))
        {
            if (canJump)
            {
                move = punchGround;
            }
            else if (state == "squat")
            {
                move = punchSquat;
            }
            else
            {
                move = punchAir;
                anim.SetBool("landing", false);
            }
            a = true;
            moveString = "punch";

        }
        if (anim.GetBool("kick"))
        {
            if (canJump)
            {
                move = kickGround;
            }
            else if (state == "squat")
            {
                move = kickSquat;
            }
            else
            {
                move = kickAir;
                anim.SetBool("landing", false);
            }

            a = true;
            moveString = "kick";
        }

        if (!a) {
            count = framecounter;
        }
        else
        {
            print(framecounter - count);
            if (framecounter - count >= move.Total)
            {
                anim.SetBool(moveString, false);
                anim.SetBool("landing", true);
            }
        }

        if (state == "punch")
        {
            anim.SetBool("punch", true);
        }
        if (state == "kick")
        {
            anim.SetBool("kick", true);
        }
    }

    public void hurtboxUpdate()
    {
        for (int i = 0; i < hurtboxes.Length; i++)
        {
            hurtboxes[i].enabled = anim.GetCurrentAnimatorStateInfo(0).IsName(states[i]);
        }
    }

    public void advanceFrame()
    {
        framecounter++;
        state = "";
    }
}

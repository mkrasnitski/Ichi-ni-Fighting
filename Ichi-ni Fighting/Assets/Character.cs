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
    private int player_num;
    private string other_player;
    private int orientation;
    private string state = "";
    private float camWidth;

    //Attack Vars
    private static string[] states = {"idleR", "idleL",
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
    private bool hit = false;
    private string moveString = "";
    private float health = 100.0f;
    private float healthDrain = 2.5f;
    private GameObject healthBar;

    //Framecounters
    private int[] framecounters = new int[10];
    private int framecounter = 0;
    private int attackCounter = 0;

    //Controls
    private KeyCode up = KeyCode.W;
    private KeyCode down = KeyCode.S;
    private KeyCode left = KeyCode.A;
    private KeyCode right = KeyCode.D;
    private KeyCode punch = KeyCode.G;
    private KeyCode kick = KeyCode.H;

    //Moves
    private static Move punchGround = new Move(2, 4, 5, 10);
    private static Move punchAir = new Move(2, 4, 10, 11);
    private static Move punchSquat = new Move(2, 4, 5, 12);
    private static Move kickGround = new Move(2, 4, 13, 13);
    private static Move kickAir = new Move(2, 4, 15, 14);
    private static Move kickSquat = new Move(2, 4, 10, 15);
    private static string[] attackTypes = { "punch", "kick" };
    private static Move[] moves = { punchGround, punchAir, punchSquat, kickGround, kickAir, kickSquat };
    private Move currentMove = new Move();

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
        hurtboxes = GetComponents<PolygonCollider2D>();
        hitboxes = GetComponents<BoxCollider2D>();
    }

    public Character(float w, float j, int js, Rigidbody2D r, Animator a, string p, PolygonCollider2D[] hurtB, BoxCollider2D[] hitB)
    {
        walkspeed = w;
        jumpspeed = j;
        jumpsquat = js;
        rb = r;
        anim = a;
        player = p;
        player_num = Convert.ToInt32(player.Trim("player".ToCharArray()));
        other_player = "player" + (3 - player_num);
        hurtboxes = hurtB;
        hitboxes = hitB;
        healthBar = GameObject.Find("healthBar" + player_num);
    }

    public Character(KeyCode[] k, Character c)
    {
        walkspeed = c.walkspeed;
        jumpspeed = c.jumpspeed;
        jumpsquat = c.jumpsquat;
        rb = c.rb;
        anim = c.anim;
        player = c.player;
        player_num = c.player_num;
        other_player = c.other_player;
        hurtboxes = c.hurtboxes;
        hitboxes = c.hitboxes;
        healthBar = c.healthBar;
        
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
        if (canJump && Input.GetKeyDown(up))
        {
            canJump = false;
            if (Input.GetKey(left))
            {
                state = "jumpLeft";
            }
            else if (Input.GetKey(right))
            {
                state = "jumpRight";
            }
            else if (isLow(rb.velocity[0]))
            {
                state = "jump";
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
        
        if (GameObject.Find(player).transform.position.x <= GameObject.Find(other_player).transform.position.x)
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
        if (state == "jumpLeft") rb.velocity = new Vector2(-jumpMult*walkspeed, jumpspeed);
        if (state == "jumpRight") rb.velocity = new Vector2(jumpMult*walkspeed, jumpspeed);
        if (state == "jump") rb.velocity = new Vector2(0f, jumpspeed);
        if (state.Contains("jump"))anim.SetBool("jump", true);

        state = "";
        anim.SetInteger("walk", orientation);

        if (canJump && !(anim.GetBool("punch") || anim.GetBool("kick")))
        {
            rb.velocity = new Vector2(0f, 0f);

            //Squat
            if (Input.GetKey(down))
            {
                anim.SetInteger("walk", 4 + anim.GetInteger("walk") % 2);
                anim.SetBool("squat", true);
            }
            else
            {
                anim.SetBool("squat", false);
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

    public void waitFrames(int frames)
    {

    }

    public float attack()
    {
        if (state == "punch" || state == "kick")
        {
            anim.SetBool(state, true);
            if (moveString == "")
            {
                //Punches first half of move list, kicks second half; 6 moves total
                int attacktype = 3 * Array.IndexOf(attackTypes, state);

                //Squat
                if (anim.GetBool("squat")) currentMove = moves[attacktype + 2];
                //Grounded
                else if (canJump) currentMove = moves[attacktype];
                //Aerial
                else
                {
                    currentMove = moves[attacktype + 1];
                    anim.SetBool("landing", false);
                }
                moveString = state;
                attackCounter = framecounter;
            }
        }
        if (framecounter - attackCounter >= currentMove.Total && moveString != "")
        {
            anim.SetBool(moveString, false);
            anim.SetBool("landing", true);
            moveString = "";
            GameObject.Find(player).GetComponent<master>().canHit = true;
            currentMove = new Move();
        }
        return currentMove.Damage;
    }

    public void doDamage()
    {
        if (hit)
        {
            float hitDamage = GameObject.Find(other_player).GetComponent<master>().currentDamage;
            health -= hitDamage;
            if (health < 0) health = 0;
            hit = false;
            GameObject.Find(other_player).GetComponent<master>().canHit = false;
        }
        animateHealthBar(healthBar, health / 100);
    }

    private void animateHealthBar(GameObject h, float size)
    {
        float h_size = h.transform.localScale.x;
        if(h_size > size)
        {
            h_size -= 0.01f * healthDrain;
            if (h_size < size) h_size = size;
            h.transform.localScale = new Vector3(h_size, 1, 1);
        }
    }

    public void boxUpdate()
    {
        for (int i = 0; i < hurtboxes.Length; i++)
        {
            hurtboxes[i].enabled = anim.GetCurrentAnimatorStateInfo(0).IsName(states[i]);
        }
        for(int i = 0; i < hitboxes.Length; i++)
        {   
            hitboxes[i].enabled = anim.GetCurrentAnimatorStateInfo(0).IsName(states[i + 8]);
        }
    }

    public void ishit()
    {
        if (GameObject.Find(other_player).GetComponent<master>().canHit)
        {
            hit = true;
        }
    }

    public void advanceFrame()
    {
        framecounter++;
        state = "";
    }
}
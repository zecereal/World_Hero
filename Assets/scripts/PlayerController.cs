﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField]
    private string animationState;
    [SerializeField]
    private string frameState;

    public GameObject player;
    public JoypadController joypad;
    public AttackController attackButton;
    public DashController dashButton;
    public Rigidbody2D rb;
    public Animator anim;

    private bool isRight;
    private float movementSpeed;
    private float dashSpeed;
    
    

    // Use this for initialization
    void Start () {
        anim = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        attackButton = FindObjectOfType<AttackController>();
        dashButton = FindObjectOfType<DashController>();
        isRight = true;
        movementSpeed = 100f;
        dashSpeed = 300f;
        
    }

    public void ChangeAnimationState(string state)
    {
        this.animationState = state;
    }

    public void ChangeFrameState(string state)
    {
        this.frameState = state;
    }

    void attack()
    {
        if (attackButton.Pressed) 
        {
            if (isRight)
            {
                anim.Play("attack_pistol_R");
            }
            else
            {
                anim.Play("attack_pistol_L");
            }
        }
    }

    void movement()
    {
        var positionX = Time.deltaTime * movementSpeed * joypad.GetTouchPosition.x;
        var positionY = Time.deltaTime * movementSpeed * joypad.GetTouchPosition.y;
        transform.Translate(positionX, positionY, 0);
        flip();
    }

    private void flip()
    {
        if (joypad.GetTouchPosition.x > 0)
        {
            isRight = true;
            anim.Play("walk_R");
        }
        else if (joypad.GetTouchPosition.x < 0)
        {
            isRight = false;
            anim.Play("walk_L");
        }

        if (joypad.GetTouchPosition.x == 0 && joypad.GetTouchPosition.y == 0)
        {
            if (isRight)
            {
                anim.Play("idle_R");
            }
            else
            {
                anim.Play("idle_L");
            }
        }
    }

    void dash()
    {
        if (dashButton.Pressed)
        {
            
            var positionX = Time.deltaTime * dashSpeed * joypad.GetTouchPosition.x;
            var positionY = Time.deltaTime * dashSpeed * joypad.GetTouchPosition.y;
            transform.Translate(positionX, positionY, 0);

            bool dashWithoutMovement = joypad.GetTouchPosition.x == 0 && joypad.GetTouchPosition.y == 0;

            if (isRight)
            {
                anim.Play("dash_R");
                if (dashWithoutMovement)
                {
                    transform.Translate(10, 0, 0);
                }
            }
            else
            {
                anim.Play("dash_L");
                if (dashWithoutMovement)
                {
                    transform.Translate(-10, 0, 0);
                }
            }

            StartCoroutine(resetVelocity(2.0f));
        }
    }

    private IEnumerator resetVelocity(float waitTime)
    {
        
        yield return new WaitForSeconds(waitTime);
        rb.velocity = new Vector3(0, 0, 0);
        dashButton.Pressed = false;
    }

    // Update is called once per frame
    void Update () {
        movement();
        attack();
        dash();
    }
}


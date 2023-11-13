using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.IK;
using UnityEngine;
using UnityEngine.InputSystem;

public class AwfulPlayerController2 : MonoBehaviour
{
    private bool isGrounded()
    {
        return true;
    }
    float horizontalInput = 0f; //-1 = left, 1 = right
    float speed = 10f;
    float jumpForce = 10f;
    Rigidbody2D rb;
    Animator anim;

    public bool isSliding = false;
    public bool isSwinging = false;

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal"); //old input system btw

        if (horizontalInput == 0 && rb.velocity == Vector2.zero && isGrounded())
        {
            anim.Play("Idle");
        }
        else if (horizontalInput != 0 && isGrounded())
        {
            if(isSliding)
            {
                Slide();
                if(Input.GetKeyUp(KeyCode.LeftShift))
                {
                    isSliding = false;
                    anim.Play("Run");
                    Run();
                }
            }
            else
            {
                if (isSwinging)
                {
                    //handle swinging physics
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        isSwinging = false;
                        anim.Play("Jump");
                        Jump();
                    }
                }
                else
                {
                    anim.Play("Run");
                    Run();
                    if (Input.GetKeyDown(KeyCode.LeftShift))
                    {
                        anim.Play("Slide");
                        isSliding = true;
                    }
                }

            }
        }
        else if(!isGrounded() && rb.velocity.y < 0f)
        {
            anim.Play("Falling");
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isSwinging)
        { //old input system too
            if (isGrounded())
            {
                anim.Play("Jump");
                Jump();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Rope")
        {
            if (isSliding)
            {
                isSwinging = true;
            }
        }
    }

    private void Slide()
    {
        //code for sliding without changing direction while doing so
    }


    private void Run()
    {
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
}

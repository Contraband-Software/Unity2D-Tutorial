using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AwfulPlayerController : MonoBehaviour
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

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal"); //old input system btw

        if(horizontalInput == 0 && rb.velocity == Vector2.zero && isGrounded())
        {
            anim.Play("Idle");
        }
        else if(horizontalInput != 0 && isGrounded())
        {
            anim.Play("Run");
            Run();
        }

        if(Input.GetKeyDown(KeyCode.Space)){ //old input system too
            if (isGrounded())
            {
                anim.Play("Jump");
                Jump();
            }
        }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public Image startImage;
    bool started; //to track if game has started
    bool jumping;
    int turn;

    Rigidbody rb;
    Animator animator;

    [SerializeField]
    float speed;


    [SerializeField]
    float jump;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        started = false;
        jumping = false;

        animator.SetBool("idle", true);

    }

    // Update is called once per frame
    void Update()
    {

        if(!started)
        {
            if (Input.GetMouseButtonDown(0))
            {
                rb.velocity = Vector3.forward * speed;
                started = true;
                turn = 1; //up

                //Starting spawning of platforms
                PlatformSpawnerScript.current.BeginToSpawn();



                animator.SetBool("idle", false);
                startImage.enabled = false;
            }
        }
        else
        {
            if(SwipeManager.IsSwipingUp())
            {
                Jump();
            }
            else if(SwipeManager.IsSwipingLeft())
            {
                Debug.Log("left");
                TurnLeft();
            }
            else if (SwipeManager.IsSwipingRight())
            {
                Debug.Log("Right");
                TurnRight();
            }
            else if (SwipeManager.IsSwipingDown())
            {
                Debug.Log("Slide");
                Slide();
            }
        }

    }

    void Jump()
    {
        if(!jumping)
        {
            jumping = true;
            animator.SetTrigger("jump");
            rb.AddForce(Vector3.up * jump, ForceMode.Impulse);
        }

    }

    void TurnLeft()
    {
        rb.transform.Rotate(0.0f, -90.0f, 0.0f);

        if (turn == 1)
        {
            rb.velocity = Vector3.left * speed;
            turn = 4; //left
        }
        else if (turn == 4)
        {
            rb.velocity = Vector3.back * speed;
            turn = 3; //down
        }
        else if (turn == 3)
        {
            rb.velocity = Vector3.right * speed;
            turn = 2; //right
        }
        else if (turn == 2)
        {
            rb.velocity = Vector3.forward * speed;
            turn = 1; //up
        }
    }

    void TurnRight()
    {
        rb.transform.Rotate(0.0f, 90.0f, 0.0f);

        if(turn == 1)
        {
            rb.velocity = Vector3.right * speed;
            turn = 2; //right
        }
        else if (turn == 2)
        {
            rb.velocity = Vector3.back * speed;
            turn = 3; //down
        }
        else if (turn == 3)
        {
            rb.velocity = Vector3.left * speed;
            turn = 4; //left
        }
        else if (turn == 4)
        {
            rb.velocity = Vector3.forward * speed;
            turn = 1; //up
        }

    }

    void Slide()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "terrain")
        {
            jumping = false;
        }
    }

}

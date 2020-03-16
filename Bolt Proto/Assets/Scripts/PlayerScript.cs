using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    Animator animator;
    Rigidbody rb;

    [SerializeField]
    float speed;

    [SerializeField]
    float jump;

    bool jumping;

    int turn; //where player turns

    float colHeight, colRadius, colCenterY, colCenterZ;

    public Image startImage;
    bool started; //if the game is already started or not


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "terrain")
        {
            jumping = false;
            Debug.Log("Player Landed");
        }
    }


    void TurnRight()
    {
        rb.transform.Rotate(0.0f, 90.0f, 0.0f);

        //according of the position we make it move

        if (turn == 1)
        {
            rb.velocity = Vector3.right * speed;
            turn = 2;//right
        }
        else if (turn == 2)
        {
            rb.velocity = Vector3.back * speed;
            turn = 3;//down
        }
        else if (turn == 3)
        {
            rb.velocity = Vector3.left * speed;
            turn = 4;//left
        }
        else if (turn == 4)
        {
            rb.velocity = Vector3.forward * speed;
            turn = 1;//down
        }
    }



    void Slide()
    {
        animator.SetTrigger("slide");
        CapsuleCollider coll = gameObject.GetComponent<CapsuleCollider>();
        //save the values
        colCenterZ = coll.center.z;
        colHeight = coll.height;
        colCenterY = coll.center.y;
        colRadius = coll.radius;
        
        

        //modify capsule collider
        coll.height = 1.3f;
        coll.radius = 0.8f;
        coll.center = new Vector3(0, 0.5f, 0.2f);
        Invoke("ExitSlide", 2f);

    }


    void Jump()
    {

        if (!jumping)
        {
            jumping = true;
            animator.SetTrigger("jump");
            rb.AddForce(Vector3.up * jump, ForceMode.Impulse);
        }

    }

    void ExitSlide()
    {
        CapsuleCollider coll = gameObject.GetComponent<CapsuleCollider>();
        //modify capsule collider
        coll.radius = colRadius;
        coll.height = colHeight;
        coll.center = new Vector3(0, colCenterY, colCenterZ);
        Invoke("ExitSlide", 2f);

    }


    void TurnLeft()
    {
        rb.transform.Rotate(0.0f, -90.0f, 0.0f);

        //according of the position we make it move

        if (turn == 1)
        {
            turn = 4; //left
            rb.velocity = Vector3.left * speed;
        }
        else if (turn == 4)
        {
            turn = 3; //down
            rb.velocity = Vector3.back * speed;
        }
        else if (turn == 3)
        {
            turn = 2; //right
            rb.velocity = Vector3.right * speed;
        }
        else if (turn == 2)
        {
            turn = 1; //up
            rb.velocity = Vector3.forward * speed;
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();


        animator.SetBool("idle", true);

        jumping = false;
        started = false;

    }

    void Update()
    {
        if(!started)
        {

            if(Input.GetMouseButtonDown(0))
            {
                started = true;
                turn = 1; //up

                rb.velocity = Vector3.forward*speed;

                startImage.enabled = false;

                ScoreManagerScript.current.StartScore();

                //start spawning of platforms
                PlatformSpawnerScript.current.BeginToSpawn();

                animator.SetBool("idle",false);
                
            }

        }
        else
        {
            if(SwipeManager.IsSwipingUp())
            {
                
                Jump();
                Debug.Log("Jump Gesture");

            }
            else if(SwipeManager.IsSwipingLeft())
            {
                
                TurnLeft();
                Debug.Log("Turn left Gesture");

            }
            else if(SwipeManager.IsSwipingRight())
            {
                
                TurnRight();
                Debug.Log("Turn Right Gesture");

            }
            else if(SwipeManager.IsSwipingDown())
            {
                
                Slide();
                Debug.Log("Slide Down Gesture");
            }

            //accelerometer-based gestures.
            float acceleration = Input.acceleration.x * Time.deltaTime * 4;
            transform.Translate(acceleration, 0, 0);
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "fence")
        {
            
            ScoreManagerScript.current.StopScore();
            animator.SetTrigger("fall2");
        }

        if (other.gameObject.tag == "obstacle")
        {
            
            ScoreManagerScript.current.StopScore();
            animator.SetTrigger("fall1");

        }

    }
    
}

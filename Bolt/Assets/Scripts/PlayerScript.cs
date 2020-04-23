using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



/**
 *  The PlayerScript class. Includes all player control and gesture functionalities like jump, turn, sway, etc.
 */
public class PlayerScript : MonoBehaviour
{
    public Image startImage;

    
    bool started; //!< A boolean variable. Checks if the game is already started or not

    bool jumping;

    Rigidbody rb;

    Animator animator;

    [SerializeField]
    float speed;

    [SerializeField]
    float jump;


   
    int turn; //!< accounts for player turns

    public AudioClip diamondFx;

    float colHeight, colRadius, colCenterY, colCenterZ; 


    /**
     * Start is called before the first frame update
     * @see Start()
     */
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        started = false;
        jumping = false;

        animator.SetBool("idle",true);
    }

    /**
     * Update is called once per frame
     * @see Update()
     */
    void Update()
    {
        if(started == false)
        {

            if(Input.GetMouseButtonDown(0))
            {
                turn = 1; //!< up
                rb.velocity = Vector3.forward*speed;
                started = true;
                
                
                PlatformSpawnerScript.current.BeginToSpawn(); //!< start spawning of platforms

                animator.SetBool("idle",false);

                startImage.enabled = false;

                ScoreManagerScript.current.StartScore();
                
            }

        }
        else
        {
            if(PlatformSpawnerScript.current.gameOver == false)
            {
                if(SwipeManager.IsSwipingUp())
                {
                    Debug.Log("Jump");
                    Jump();
                    //jumping=false;
                }
                else if (SwipeManager.IsSwipingDown())
                {
                    Debug.Log("Slide");
                    Slide();
                }
                else if (SwipeManager.IsSwipingRight())
                {
                    Debug.Log("Right");
                    TurnRight();
                }
                else if(SwipeManager.IsSwipingLeft())
                {
                    Debug.Log("left");
                    TurnLeft();
                }

                float acceleration = Input.acceleration.x * Time.deltaTime*4;
                transform.Translate(acceleration, 0, 0);
            }
        }

    }


    /**
    * Makes the player jump
    */
    void Jump()
    {

            if(jumping == false)
            {
                jumping=true;
                animator.SetTrigger("jump");
                rb.AddForce(Vector3.up*jump,ForceMode.Impulse);
            }
            
    }


    /**
     * Trigger function for colliding with obstacle, fence or collecting diamonds
     */
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "obstacle")
        {
            animator.SetTrigger("fall1");

            PlatformSpawnerScript.current.gameOver = true;

            ScoreManagerScript.current.StopScore();
        }
        else if (other.gameObject.tag == "diamond")
        {
            if (PlatformSpawnerScript.current.gameOver == false)
            {
                Destroy(other.gameObject);
                ScoreManagerScript.current.DiamondScore();
                AudioManagerScript.current.PlaySound(diamondFx);
            }
        }
        else if (other.gameObject.tag == "fence")
        {
            animator.SetTrigger("fall2");
            PlatformSpawnerScript.current.gameOver = true;
            ScoreManagerScript.current.StopScore();
        }
       
    }


    /**
     * Makes the player slide 
     */
    void Slide()
    {
        animator.SetTrigger("slide");
        CapsuleCollider coll = gameObject.GetComponent<CapsuleCollider>();
        //save the values
        colHeight = coll.height;
        colRadius = coll.radius;
        colCenterY = coll.center.y;
        colCenterZ = coll.center.z;

        //modify capsule collider
        coll.height = 1.3f;
        coll.radius = 0.8f;
        coll.center = new Vector3(0, 0.5f, 0.2f);
        Invoke("ExitSlide", 2f);

    }


    /**
    * Makes the player turn left
    */
    void TurnLeft()
    {
        rb.transform.Rotate(0.0f, -90.0f, 0.0f);

        //!< according of the position we make it move
        if(turn==1)
        {
            rb.velocity = Vector3.left*speed;
            turn = 4; //left
        }
        else if(turn==4)
        {
            rb.velocity = Vector3.back*speed;
            turn=3;//down
        }
        else if(turn==3)
        {
            rb.velocity = Vector3.right*speed;
            turn=2;//right
        }
        else if(turn==2)
        {
            rb.velocity = Vector3.forward*speed;
            turn=1;//up
        }
    }



    /**
     * Exist slide animation
     */
    void ExitSlide()
    {
        CapsuleCollider coll = gameObject.GetComponent<CapsuleCollider>();
        //modify capsule collider
        coll.height = colHeight;
        coll.radius = colRadius;
        coll.center = new Vector3(0, colCenterY, colCenterZ);
        Invoke("ExitSlide", 2f);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "terrain")
        {
            Debug.Log("Landed");
            jumping = false;
        }
    }

    /**
     * Makes the player turn right
     */
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


    /**
     * Restarts the game
     */
    public void Replay()
    {
        SceneManager.LoadScene(0);
        
    }
    
}

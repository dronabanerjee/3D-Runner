using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * The PlatformSpawnerScript handles the random spawning of platforms
 */
public class PlatformSpawnerScript : MonoBehaviour
{

    public GameObject platform, corner1, corner2, corner3, corner4;

    public bool gameOver;

    Vector3 lastPos; //last platform position
    float size, sizeCorner; //value of z axis that is length of the platform //size of the platform of type corner

    int direction; //to not the actual direction of the ply

    private int counterUp, counterHor; //indicates no of platforms to be created
    float timeForCreation = 0.8f; //perfect timing to wait to create a new platform

    public static PlatformSpawnerScript current;

    public GameObject diamond;

    private void Awake()
    {
        current = this;
    }

    //

    /**
     *  Start is called before the first frame update
     */
    void Start()
    {
        gameOver = true;

        direction = 1; //up

        lastPos = new Vector3(platform.transform.position.x, platform.transform.position.y, platform.transform.position.z - 0.2f);

        Bounds momSize = GetMaxBounds(platform);
        size = momSize.size.z;

        counterUp = 5;
        InvokeRepeating("SpawnInitialVertical", 0.1f, 0.1f);

    }

    // Update is called once per frame
    void Update()
    {

    }

    /**
     * Spawns diamonds on the path randomly
     */
    void CreateDiamonds(Vector3 pos)
    {
        int rand = Random.Range(0,4);
        int randx = Random.Range(0, 8);

        if (rand<1){

            if(randx < 3)
            {
                if(randx == 1)
                    Instantiate(diamond, new Vector3(pos.x + 1f, pos.y + 3f, pos.z), diamond.transform.rotation);
                else
                    Instantiate(diamond, new Vector3(pos.x + 1f, pos.y + 1f, pos.z), diamond.transform.rotation);
            }
            else if(randx >= 3 && randx <= 5)
            {
                if(randx == 4)
                    Instantiate(diamond, new Vector3(pos.x - 1f, pos.y + 3f, pos.z), diamond.transform.rotation);
                else
                    Instantiate(diamond, new Vector3(pos.x - 1f, pos.y + 1f, pos.z), diamond.transform.rotation);
            }
            else
            {
                if(randx == 7)
                    Instantiate(diamond, new Vector3(pos.x, pos.y + 3f, pos.z), diamond.transform.rotation);
                else
                    Instantiate(diamond, new Vector3(pos.x, pos.y + 1f, pos.z), diamond.transform.rotation);
            }
            
        }
    }

    /**
     * Spawns initial vertical platform
     */
    void SpawnInitialVertical()
    {
        Vector3 pos = lastPos;
        pos.z += size;

        lastPos = new Vector3(pos.x, pos.y, pos.z - 0.2f);

        GameObject newObject = GroundVerticalPoolerScript.current.GetPooledObject();
        if (newObject == null)
        {
            return;
        }

        newObject.transform.position = pos;
        newObject.transform.rotation = Quaternion.identity;
        newObject.SetActive(true);
        //GameObject child2 = newObject.transform.GetChild(1).gameObject;
        //child2.tag="terrain";

        CreateDiamonds(pos);


        if (--counterUp <= 0)
        {
            CancelInvoke("SpawnInitialVertical");
        }




    }

    /**
     * Starts the spawning of platforms
     */
    public void BeginToSpawn()
    {
        direction = 1;
        gameOver = false;

        Bounds momSize = GetMaxBounds(platform);

        size = momSize.size.z;

        momSize = GetMaxBounds(corner1);

        sizeCorner = momSize.size.z;

        counterUp = 1;

        InvokeRepeating("SpawnVertical", 0.1f, timeForCreation);
    }

    /**
     * Spawns vertical platforms
     */
    void SpawnVertical()
    {
        Vector3 pos = lastPos;
        pos.z += size;
        lastPos = new Vector3(pos.x, pos.y, pos.z - 0.2f);

        GameObject newObject = GroundVerticalPoolerScript.current.GetPooledObject();
        if (newObject == null)
        {
            return;
        }

        newObject.transform.position = pos;
        newObject.transform.rotation = Quaternion.identity;
        newObject.SetActive(true);

        CreateDiamonds(pos);

        if (--counterUp <= 0)
        {
            CancelInvoke("SpawnVertical");

            if (!gameOver)
            {

                CreateCombinations();
                SpawnCornersHorizontal();
            }
        }
    }

    Bounds GetMaxBounds(GameObject g)
    {
        var b = new Bounds(g.transform.position, Vector3.zero);

        foreach (Renderer r in g.GetComponentsInChildren<Renderer>())
        {
            b.Encapsulate(r.bounds);
        }

        return b;
    }



    /**
     * Spawns the horizontal corners
     */
    void SpawnCornersHorizontal()
    {
        int rand = Random.Range(0, 2);


        if (rand < 1)
        {
            //angle goes to the left
            SpawnCornerLeft();
        }
        else
        {
            //angle goes to the right
            SpawnCornerRight();

        }
    }

    /**
     * Spawns horizontal platforms to the right
     */
    void SpawnHorizontalRight()
    {
        direction = 2;
        Vector3 pos = lastPos;
        pos.x += size;
        lastPos = new Vector3(pos.x - 0.2f, pos.y, pos.z);
        GameObject newObject = GroundVerticalPoolerScript.current.GetPooledObject();
        if (newObject == null)
        {
            return;
        }

        newObject.transform.position = pos;
        newObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        newObject.SetActive(true);

        CreateDiamonds(pos);
        if (--counterHor <= 0)
        {
            CancelInvoke("SpawnHorizontalRight");

            if (!gameOver)
            {
                int rand = Random.Range(0, 10);
               
                if (rand > 1)
                {
                    //empty platform
                    SpawnEmptyHorizontalRight();
                }
                else
                {

                    //lastPos = new Vector3(pos.x, pos.y, pos.z);
                    SpawnCornerUp();
                }
            }
        }
    }

    /**
     * Spawns left corners
     */
    void SpawnCornerLeft()
    {
        Vector3 pos = lastPos;

        //pos.x += (sizeCorner / 2) - 0.2f;
        //pos.z += (sizeCorner / 2) - 0.2f;

        lastPos = new Vector3(pos.x + 5.9f, pos.y, pos.z + 1.9f);

        GameObject newObj = Corner2PoolerScript.current.GetPooledObject();

        if (newObj == null)
        {
            return;
        }

        newObj.transform.position = pos;
        newObj.transform.rotation = Quaternion.Euler(0, 180, 0);
        newObj.SetActive(true);

        if (!gameOver)
        {
            counterHor = 4;
            InvokeRepeating("SpawnHorizontalLeft", 0.1f, timeForCreation);
        }
    }

    /**
     * Spawns right corners
     */
    void SpawnCornerRight()
    {
        Vector3 pos = lastPos;

        pos.x += (sizeCorner / 2) - 0.2f;
        pos.z += (sizeCorner / 2) - 0.2f;

        lastPos = new Vector3(pos.x - 0.2f, pos.y, pos.z);

        GameObject newObj = Corner1PoolerScript.current.GetPooledObject();

        if (newObj == null)
        {
            return;
        }

        newObj.transform.position = pos;
        newObj.transform.rotation = Quaternion.Euler(0, 90, 0);
        newObj.SetActive(true);

         if(!gameOver){

             counterHor = 4;
             InvokeRepeating("SpawnHorizontalRight",0.1f,timeForCreation);
         }

    }


    /**
     * Spawns horizontal platforms to the left
     */
    void SpawnHorizontalLeft()
    {
        direction = 4;
        Vector3 pos = lastPos;
        pos.x -= size;
        lastPos = new Vector3(pos.x + 0.2f, pos.y, pos.z);
        GameObject newObject = GroundVerticalPoolerScript.current.GetPooledObject();
        if (newObject == null)
        {
            return;
        }

        newObject.transform.position = pos;
        newObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        newObject.SetActive(true);

        CreateDiamonds(pos);
        if (--counterHor <= 0)
        {
            CancelInvoke("SpawnHorizontalLeft");

            if (!gameOver)
            {
                int rand = Random.Range(0, 10);
                
                if (rand > 1)
                {
                    //empty platform
                    SpawnEmptyHorizontalLeft();
                }
                else
                {
                    
                    lastPos = new Vector3(pos.x - 9.5f, pos.y, pos.z + 1.9f);

                    SpawnCornerUp();
                }
            }
        }
    }

    /**
     * Spawns empty horizontal platforms to the right randomly
     */
    void SpawnEmptyHorizontalRight()
    {
        direction = 2;
        Vector3 pos = lastPos;
        pos.x += size;
        lastPos = new Vector3(pos.x - 0.2f, pos.y, pos.z);
        GameObject newObject = GroundEmptyVerticalPoolerScript.current.GetPooledObject();
        if (newObject == null)
        {
            return;
        }

        newObject.transform.position = pos;
        newObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        newObject.SetActive(true);
        //normal platform after turning
        pos = lastPos;
        pos.x += size;
        lastPos = new Vector3(pos.x - 0.2f, pos.y, pos.z);
        newObject = GroundVerticalPoolerScript.current.GetPooledObject();
        if (newObject == null)
        {
            return;
        }

        newObject.transform.position = pos;
        newObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        newObject.SetActive(true);

        CreateDiamonds(pos);
        //horizontal platform with an obstacle by random construction
        pos = lastPos;
        pos.x += size;
        lastPos = new Vector3(pos.x - 0.2f, pos.y, pos.z);
        int rand = Random.Range(0, 2);
        if (rand < 1)
        {
            newObject = GroundVerticalObstacle1PoolerScript.current.GetPooledObject();
        }
        else
        {
            newObject = GroundVerticalObstacle2PoolerScript.current.GetPooledObject();
        }
        
        if (newObject == null)
        {
            return;
        }

        newObject.transform.position = pos;
        newObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        newObject.SetActive(true);
        //normal platform
        pos = lastPos;
        pos.x += size;
        lastPos = new Vector3(pos.x - 0.2f, pos.y, pos.z);
        newObject = GroundVerticalPoolerScript.current.GetPooledObject();
        if (newObject == null)
        {
            return;
        }

        newObject.transform.position = pos;
        newObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        newObject.SetActive(true);

        CreateDiamonds(pos);

        lastPos = new Vector3(pos.x, pos.y, pos.z);
        //put corner that goes up
        SpawnCornerUp();
    }



    /**
     * Spawns empty horizontal left platforms randomly
     */
    void SpawnEmptyHorizontalLeft()
    {

        direction = 4;
        Vector3 pos = lastPos;
        pos.x -= size;
        lastPos = new Vector3(pos.x + 0.2f, pos.y, pos.z);
        GameObject newObject = GroundEmptyVerticalPoolerScript.current.GetPooledObject();
        if (newObject == null)
        {
            return;
        }

        newObject.transform.position = pos;
        newObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        newObject.SetActive(true);
        //normal platform after turning
        pos = lastPos;
        pos.x -= size;
        lastPos = new Vector3(pos.x + 0.2f, pos.y, pos.z);
        newObject = GroundVerticalPoolerScript.current.GetPooledObject();
        if (newObject == null)
        {
            return;
        }

        newObject.transform.position = pos;
        newObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        newObject.SetActive(true);

        CreateDiamonds(pos);
        //horizontal platform with an obstacle by random construction
        pos = lastPos;
        pos.x -= size;
        lastPos = new Vector3(pos.x + 0.2f, pos.y, pos.z);
        int rand = Random.Range(0, 2);
        if (rand < 1)
        {
            newObject = GroundVerticalObstacle1PoolerScript.current.GetPooledObject();
        }
        else
        {
            newObject = GroundVerticalObstacle2PoolerScript.current.GetPooledObject();
        }

        if (newObject == null)
        {
            return;
        }

        newObject.transform.position = pos;
        newObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        newObject.SetActive(true);
        //normal platform
        pos = lastPos;
        pos.x -= size;
        lastPos = new Vector3(pos.x + 0.2f, pos.y, pos.z);
        newObject = GroundVerticalPoolerScript.current.GetPooledObject();
        if (newObject == null)
        {
            return;
        }

        newObject.transform.position = pos;
        newObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        newObject.SetActive(true);

        CreateDiamonds(pos);

        lastPos = new Vector3(pos.x - 9.5f, pos.y, pos.z + 1.9f);
        //put corner that goes up
        SpawnCornerUp();
    }
    void SpawnCornerUp()
    {
        Vector3 pos = lastPos;
        if(direction == 2)
        {
            lastPos = new Vector3(pos.x + 1.9f, pos.y, pos.z + 1.8f);
            GameObject newObj = Corner3PoolerScript.current.GetPooledObject();

            if (newObj == null)
            {
                return;
            }

            newObj.transform.position = pos;
            newObj.transform.rotation = Quaternion.Euler(0, -90, 0);
            newObj.SetActive(true);
        }
        else
        {
            lastPos = new Vector3(pos.x, pos.y, pos.z - 0.2f);
            GameObject newObj = Corner4PoolerScript.current.GetPooledObject();

            if (newObj == null)
            {
                return;
            }

            newObj.transform.position = pos;
            newObj.transform.rotation = Quaternion.Euler(0, 0, 0);
            newObj.SetActive(true);
        }

        direction = 1;
        if (!gameOver)
        {
            CreateCombinations();
            counterUp = 5;
            InvokeRepeating("SpawnVertical", 0.1f, timeForCreation);
        }
    }

    /**
     * Spawns the vertical obstacles
     */
    void SpawnObstacleVertical(GameObject newObj)
    {
        Vector3 pos = lastPos;
        pos.z += size;
        lastPos = new Vector3(pos.x, pos.y, pos.z - 0.2f);

        //GameObject newObject = GroundVerticalPoolerScript.current.GetPooledObject();
        if (newObj == null)
        {
            return;
        }

        newObj.transform.position = pos;
        newObj.transform.rotation = Quaternion.identity;
        newObj.SetActive(true);
        //new normal platform
        pos = lastPos;
        pos.z += size;
        lastPos = new Vector3(pos.x, pos.y, pos.z - 0.2f);

        newObj = GroundVerticalPoolerScript.current.GetPooledObject();
        if (newObj == null)
        {
            return;
        }

        newObj.transform.position = pos;
        newObj.transform.rotation = Quaternion.identity;
        newObj.SetActive(true);

        CreateDiamonds(pos);
    }


    /**
     * Spawns obstacles randomly
     */
    void CreateCombinations()
    {
        SpawnInitialVertical();
        SpawnInitialVertical();
        int rand = Random.Range(0, 10);
        if (rand >= 0 && rand < 2)
        {
            SpawnObstacleVertical(GroundEmptyVerticalPoolerScript.current.GetPooledObject());
            SpawnObstacleVertical(GroundVerticalObstacle1PoolerScript.current.GetPooledObject());
        }
        if (rand >= 2 && rand < 4)
        {
            SpawnObstacleVertical(GroundEmptyVerticalPoolerScript.current.GetPooledObject());
            SpawnObstacleVertical(GroundVerticalObstacle2PoolerScript.current.GetPooledObject());
        }
        if (rand >= 4 && rand < 6)
        {
            SpawnObstacleVertical(GroundVerticalObstacle1PoolerScript.current.GetPooledObject());
            SpawnObstacleVertical(GroundVerticalObstacle2PoolerScript.current.GetPooledObject());
        }
        if (rand >= 6 && rand < 8)
        {
            SpawnObstacleVertical(GroundVerticalObstacle1PoolerScript.current.GetPooledObject());
            SpawnObstacleVertical(GroundVerticalObstacle1PoolerScript.current.GetPooledObject());
        }
        if (rand >= 8 && rand < 10)
        {
            SpawnObstacleVertical(GroundVerticalObstacle2PoolerScript.current.GetPooledObject());
            SpawnObstacleVertical(GroundVerticalObstacle2PoolerScript.current.GetPooledObject());
        }
    }
}

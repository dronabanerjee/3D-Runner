/*
TODO fix bugs related to gem creation, randomize gem creation

*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawnerScript : MonoBehaviour
{

    public GameObject platform, corner1, corner2, corner3, corner4;
    public bool gameOver;
    Vector3 lastPos;
    float size, sizeCorner;
    int direction;
    private int counterUp, counterHor;
    float timeForCreation = 0.8f;
    public GameObject diamond;
    public static PlatformSpawnerScript current;

    private void Awake()
    {
        current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameOver = true;
        direction = 1;
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

    void CreateDiamonds (Vector3 pos)
    {
        int rand = Random.Range(0, 4);
        if(rand < 1)
        {
            Instantiate(diamond, new Vector3(pos.x, pos.y + 1f, pos.z), diamond.transform.rotation);
        }
    }


    void SpawnInitialVertical()
    {
        Vector3 pos = lastPos;
        pos.z += size;

        lastPos = new Vector3(pos.x, pos.y, pos.z - 0.2f);

        GameObject newObject = GroundVerticalPoolerScript.current.GetPooledObject();
        if (newObject == null) return;

        newObject.transform.position = pos;
        newObject.transform.rotation = Quaternion.identity;
        newObject.SetActive(true);
        //newObject.tag = "terrain";

        CreateDiamonds(pos);


        if (--counterUp <= 0)
        {
            CancelInvoke("SpawnInitialVertical");
        }
    }

    public void BeginToSpawn()
    {
        direction = 1; //up
        gameOver = false;

        Bounds momSize = GetMaxBounds(platform);
        size = momSize.size.z;

        momSize = GetMaxBounds(corner1);
        sizeCorner = momSize.size.z;
        counterUp = 1;

        InvokeRepeating("SpawnVertical", 0.1f, timeForCreation);

    }

    void SpawnVertical()
    {
        Vector3 pos = lastPos;
        pos.z += size;

        lastPos = new Vector3(pos.x, pos.y, pos.z - 0.2f);

        GameObject newObject = GroundVerticalPoolerScript.current.GetPooledObject();
        if (newObject == null) return;

        newObject.transform.position = pos;
        newObject.transform.rotation = Quaternion.identity;
        newObject.SetActive(true);
        //newObject.tag = "terrain";

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

    void SpawnObstacleVertical(GameObject newObj)
    {
        Vector3 pos = lastPos;
        pos.z += size;

        lastPos = new Vector3(pos.x, pos.y, pos.z - 0.2f);

        //GameObject newObject = GroundVerticalPoolerScript.current.GetPooledObject();
        if (newObj == null) return;

        newObj.transform.position = pos;
        newObj.transform.rotation = Quaternion.identity;
        newObj.SetActive(true);



        pos = lastPos;
        pos.z += size;

        lastPos = new Vector3(pos.x, pos.y, pos.z - 0.2f);

        newObj = GroundVerticalPoolerScript.current.GetPooledObject();
        if (newObj == null) return;

        newObj.transform.position = pos;
        newObj.transform.rotation = Quaternion.identity;
        newObj.SetActive(true);

        CreateDiamonds(pos);

    }

    void CreateCombinations()
    {
        SpawnInitialVertical();
        SpawnInitialVertical();

        int rand = Random.Range(0, 10);

        // 0-1
        if(rand >= 0 && rand < 2)
        {
            //empty
            SpawnObstacleVertical(GroundEmptyVerticalPoolerScript.current.GetPooledObject());

            // obstacle 1
            SpawnObstacleVertical(GroundVerticalObstacle1PoolerScript.current.GetPooledObject());
        }
        //2 - 3
        if(rand > 1 && rand < 4)
        {
            //empty
            SpawnObstacleVertical(GroundEmptyVerticalPoolerScript.current.GetPooledObject());

            //obstacle 2
            SpawnObstacleVertical(GroundVerticalObstacle2PoolerScript.current.GetPooledObject());
        }

        //4 - 5
        if (rand > 3 && rand < 6)
        {
            //obstacle 1
            SpawnObstacleVertical(GroundVerticalObstacle1PoolerScript.current.GetPooledObject());

            //obstacle 2
            SpawnObstacleVertical(GroundVerticalObstacle2PoolerScript.current.GetPooledObject());
        }

        // 6 - 7
        if (rand > 5 && rand < 8)
        {
            //obstacle 1
            SpawnObstacleVertical(GroundVerticalObstacle1PoolerScript.current.GetPooledObject());

            //obstacle 1
            SpawnObstacleVertical(GroundVerticalObstacle1PoolerScript.current.GetPooledObject());

        }

        // 8 - 9
        if (rand > 7 && rand < 10)
        {
            //obstacle 2
            SpawnObstacleVertical(GroundVerticalObstacle2PoolerScript.current.GetPooledObject());

            //obstacle 2
            SpawnObstacleVertical(GroundVerticalObstacle2PoolerScript.current.GetPooledObject());

        }
    }

    void SpawnCornersHorizontal()
    {
        int rand = Random.Range(0, 2); //possible values 0 or 1

        //rand = 0; // only for testing

        if (rand < 1) // goes left
        {
            SpawnCornerLeft();
        }
        else // goes right
        {
            SpawnCornerRight();
        }
    }

    void SpawnCornerLeft()
    {
        Vector3 pos = lastPos;

        //pos.x += (sizeCorner / 2) - 0.2f;
        //pos.z += (sizeCorner / 2) - 0.2f;

        lastPos = new Vector3(pos.x + 5.9f, pos.y, pos.z + 1.9f);
        GameObject newObj = Corner2PoolerScript.current.GetPooledObject();

        if (newObj == null) return;

        newObj.transform.position = pos;
        newObj.transform.rotation = Quaternion.Euler(0, 180, 0);
        newObj.SetActive(true);

        
        if (!gameOver)
        {
            counterHor = 4;
            InvokeRepeating("SpawnHorizontalLeft", 0.1f, timeForCreation);
        }
        
    }

    void SpawnCornerRight()
    {
        Vector3 pos = lastPos;

        pos.x += (sizeCorner / 2) - 0.2f;
        pos.z += (sizeCorner / 2) - 0.2f;

        lastPos = new Vector3(pos.x - 0.2f, pos.y, pos.z);
        GameObject newObj = Corner1PoolerScript.current.GetPooledObject();

        if (newObj == null) return;

        newObj.transform.position = pos;
        newObj.transform.rotation = Quaternion.Euler(0, 90, 0);
        newObj.SetActive(true);

        if (!gameOver)
        {
            counterHor = 4;
            InvokeRepeating("SpawnHorizontalRight", 0.1f, timeForCreation);
        }

    }

    void SpawnCornerUp()
    {
        Vector3 pos = lastPos;

        if(direction == 2)
        {
            lastPos = new Vector3(pos.x + 1.9f, pos.y, pos.z + 1.8f);

            GameObject newObj = Corner3PoolerScript.current.GetPooledObject();

            if (newObj == null) return;

            newObj.transform.position = pos;
            newObj.transform.rotation = Quaternion.Euler(0, -90, 0);
            newObj.SetActive(true);
        }
        else
        {
            lastPos = new Vector3(pos.x, pos.y, pos.z - 0.2f);

            GameObject newObj = Corner4PoolerScript.current.GetPooledObject();

            if (newObj == null) return;

            newObj.transform.position = pos;
            newObj.transform.rotation = Quaternion.Euler(0, 0, 0);
            newObj.SetActive(true);
        }

        direction = 1;

        if(!gameOver)
        {
            CreateCombinations();

            counterUp = 5;
            InvokeRepeating("SpawnVertical", 0.1f, timeForCreation);
        }
    }

    void SpawnHorizontalRight()
    {
        direction = 2;

        Vector3 pos = lastPos;
        pos.x += size;
        lastPos = new Vector3(pos.x - 0.2f, pos.y, pos.z);

        GameObject newObject = GroundVerticalPoolerScript.current.GetPooledObject();
        if (newObject == null) return;

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
                //rand = 2; // just for testing

                if (rand > 1)
                {
                    SpawnEmptyHorizontalRight();
                }
                else
                {
                    //lastPos = new Vector3(pos.x, pos.y, pos.z);
                    //SpawnCornerUp();
                }
            }
        }

    }


    void SpawnHorizontalLeft()
    {
        direction = 4;

        Vector3 pos = lastPos;
        pos.x -= size;
        lastPos = new Vector3(pos.x - 0.2f, pos.y, pos.z);

        GameObject newObject = GroundVerticalPoolerScript.current.GetPooledObject();
        if (newObject == null) return;

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
                //rand = 0; // just for testing

                if (rand > 1)
                {
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


    void SpawnEmptyHorizontalRight()
    {
        direction = 2;

        Vector3 pos = lastPos;
        pos.x += size;
        lastPos = new Vector3(pos.x - 0.2f, pos.y, pos.z);

        GameObject newObject = GroundEmptyVerticalPoolerScript.current.GetPooledObject();
        if (newObject == null) return;

        newObject.transform.position = pos;
        newObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        newObject.SetActive(true);

        

        pos = lastPos;
        pos.x += size;
        lastPos = new Vector3(pos.x - 0.2f, pos.y, pos.z);

        newObject = GroundVerticalPoolerScript.current.GetPooledObject();
        if (newObject == null) return;

        newObject.transform.position = pos;
        newObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        newObject.SetActive(true);

        CreateDiamonds(pos);

        //horizontal platform with obstacle
        pos = lastPos;
        pos.x += size;
        lastPos = new Vector3(pos.x - 0.2f, pos.y, pos.z);

        int rand = Random.Range(0, 2);

        if(rand < 1)
        {
            newObject = GroundVerticalObstacle1PoolerScript.current.GetPooledObject();
        }
        else
        {
            newObject = GroundVerticalObstacle2PoolerScript.current.GetPooledObject();
        }

        if (newObject == null) return;

        newObject.transform.position = pos;
        newObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        newObject.SetActive(true);



        //another normal platform to recover from jumping

        pos = lastPos;
        pos.x += size;
        lastPos = new Vector3(pos.x - 0.2f, pos.y, pos.z);

        newObject = GroundVerticalPoolerScript.current.GetPooledObject();
        if (newObject == null) return;

        newObject.transform.position = pos;
        newObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        newObject.SetActive(true);

        CreateDiamonds(pos);

        lastPos = new Vector3(pos.x, pos.y, pos.z);
        //corner that goes up


        SpawnCornerUp();


    }

    void SpawnEmptyHorizontalLeft()
    {
        direction = 4;

        Vector3 pos = lastPos;
        pos.x -= size;
        lastPos = new Vector3(pos.x + 0.2f, pos.y, pos.z);

        GameObject newObject = GroundEmptyVerticalPoolerScript.current.GetPooledObject();
        if (newObject == null) return;

        newObject.transform.position = pos;
        newObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        newObject.SetActive(true);


        pos = lastPos;
        pos.x -= size;
        lastPos = new Vector3(pos.x + 0.2f, pos.y, pos.z);

        newObject = GroundVerticalPoolerScript.current.GetPooledObject();
        if (newObject == null) return;

        newObject.transform.position = pos;
        newObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        newObject.SetActive(true);

        CreateDiamonds(pos);

        //horizontal platform with obstacle
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

        if (newObject == null) return;

        newObject.transform.position = pos;
        newObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        newObject.SetActive(true);


        //another normal platform to recover from jumping

        pos = lastPos;
        pos.x -= size;
        lastPos = new Vector3(pos.x + 0.2f, pos.y, pos.z);

        newObject = GroundVerticalPoolerScript.current.GetPooledObject();
        if (newObject == null) return;

        newObject.transform.position = pos;
        newObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        newObject.SetActive(true);

        CreateDiamonds(pos);

        lastPos = new Vector3(pos.x - 9.5f, pos.y, pos.z + 1.9f);
        //corner that goes up


        SpawnCornerUp();


    }

}

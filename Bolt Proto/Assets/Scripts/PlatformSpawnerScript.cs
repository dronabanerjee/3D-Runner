using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawnerScript : MonoBehaviour
{
    public bool gameOver;

    Vector3 lastPos; //last platform position
    float size, sizeCorner; //value of z axis that is length of the platform //size of the platform of type corner

    int direction; //to not the actual direction of the ply
    float timeForCreation = 0.8f; //perfect timing to wait to create a new platform

    public GameObject platform, corner1, corner2, corner3, corner4;

    private int counterUp, counterHor; //indicates no of platforms to be created
    

    public static PlatformSpawnerScript current;


    Bounds GetMaxBounds(GameObject g)
    {
        var bound = new Bounds(g.transform.position, Vector3.zero);

        foreach (Renderer r in g.GetComponentsInChildren<Renderer>())
        {
            bound.Encapsulate(r.bounds);
        }

        return bound;
    }

    private void Awake()
    {
        current = this;
    }

    public void BeginToSpawn()
    {
        direction = 1;
        gameOver = false;
        counterUp = 1;

        Bounds Size_m = GetMaxBounds(platform);
        size = Size_m.size.z;
        Size_m = GetMaxBounds(corner1);

        sizeCorner = Size_m.size.z;
        InvokeRepeating("SpawnVertical", 0.1f, timeForCreation);
    }


    void SpawnCornerLeft()
    {
        Vector3 pos = lastPos;

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

    void SpawnHorizontalLeft()
    {
        
        Vector3 pos = lastPos;
        direction = 4;
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

        if (--counterHor <= 0)
        {
            CancelInvoke("SpawnHorizontalLeft");

            if (gameOver == false)
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

        if (gameOver == false)
        {

            counterHor = 4;
            InvokeRepeating("SpawnHorizontalRight", 0.1f, timeForCreation);
        }

    }



    void SpawnCornersHorizontal()
    {
        int rand = Random.Range(0, 2);


        if (rand >= 1)
        {
            //angle goes to the right
            SpawnCornerRight();

        }
        else
        {
            //angle goes to the left
            SpawnCornerLeft();

        }
    }


    void Start()
    {
        lastPos = new Vector3(platform.transform.position.x, platform.transform.position.y, platform.transform.position.z - 0.2f);

        direction = 1; //up
        gameOver = true;
        counterUp = 5;

        Bounds Size_m = GetMaxBounds(platform);

        size = Size_m.size.z;

        InvokeRepeating("SpawnInitialVertical", 0.1f, 0.1f);

    }


    void CreateCombinations()
    {
        SpawnInitialVertical();
        SpawnInitialVertical();


        int rand = Random.Range(0, 10);
        if (rand >= 8 && rand < 10)
        {
            SpawnObstacleVertical(GroundVerticalObstacle2PoolerScript.current.GetPooledObject());
            SpawnObstacleVertical(GroundVerticalObstacle2PoolerScript.current.GetPooledObject());
        }
        if (rand >= 6 && rand < 8)
        {
            SpawnObstacleVertical(GroundVerticalObstacle1PoolerScript.current.GetPooledObject());
            SpawnObstacleVertical(GroundVerticalObstacle1PoolerScript.current.GetPooledObject());
        }
        if (rand >= 4 && rand < 6)
        {
            SpawnObstacleVertical(GroundVerticalObstacle1PoolerScript.current.GetPooledObject());
            SpawnObstacleVertical(GroundVerticalObstacle2PoolerScript.current.GetPooledObject());
        }
        if (rand >= 2 && rand < 4)
        {
            SpawnObstacleVertical(GroundEmptyVerticalPoolerScript.current.GetPooledObject());
            SpawnObstacleVertical(GroundVerticalObstacle2PoolerScript.current.GetPooledObject());
        }
        if (rand >= 0 && rand < 2)
        {
            SpawnObstacleVertical(GroundEmptyVerticalPoolerScript.current.GetPooledObject());
            SpawnObstacleVertical(GroundVerticalObstacle1PoolerScript.current.GetPooledObject());
        }

    }





    void SpawnHorizontalRight()
    {
        
        Vector3 pos = lastPos;
        pos.x += size;
        direction = 2;

        lastPos = new Vector3(pos.x - 0.2f, pos.y, pos.z);
        GameObject newObject = GroundVerticalPoolerScript.current.GetPooledObject();
        if (newObject == null)
        {
            return;
        }

        newObject.transform.position = pos;
        newObject.transform.rotation = Quaternion.Euler(0, 90, 0);
        newObject.SetActive(true);

        if (--counterHor <= 0)
        {
            CancelInvoke("SpawnHorizontalRight");

            if (gameOver == false)
            {
                int rand = Random.Range(0, 10);
               
                if (rand <= 1)
                {
                    //lastPos = new Vector3(pos.x, pos.y, pos.z);
                    SpawnCornerUp();
                }
                else
                {

                    //empty platform
                    SpawnEmptyHorizontalRight();
                    
                }
            }
        }
    }



    void SpawnEmptyHorizontalRight()
    {
        
        Vector3 pos = lastPos;
        pos.x += size;
        direction = 2;
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
        //horizontal platform with an obstacle by random construction
        pos = lastPos;
        pos.x += size;
        lastPos = new Vector3(pos.x - 0.2f, pos.y, pos.z);
        int rand = Random.Range(0, 2);
        if (rand >= 1)
        {
            newObject = GroundVerticalObstacle2PoolerScript.current.GetPooledObject();
        }
        else
        {
            newObject = GroundVerticalObstacle1PoolerScript.current.GetPooledObject();
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

        lastPos = new Vector3(pos.x, pos.y, pos.z);
        //put corner that goes up
        SpawnCornerUp();
    }
    void SpawnEmptyHorizontalLeft()
    {

        
        Vector3 pos = lastPos;
        pos.x -= size;
        direction = 4;
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
        //horizontal platform with an obstacle by random construction
        pos = lastPos;
        pos.x -= size;
        lastPos = new Vector3(pos.x + 0.2f, pos.y, pos.z);
        int rand = Random.Range(0, 2);
        if (rand >= 1)
        {
            newObject = GroundVerticalObstacle2PoolerScript.current.GetPooledObject();
        }
        else
        {
            newObject = GroundVerticalObstacle1PoolerScript.current.GetPooledObject();
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

        lastPos = new Vector3(pos.x - 9.5f, pos.y, pos.z + 1.9f);
        //put corner that goes up
        SpawnCornerUp();
    }
    void SpawnCornerUp()
    {
        Vector3 pos = lastPos;
        if(direction != 2)
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
        else
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

        direction = 1;
        if (gameOver == false)
        {
            counterUp = 5;
            CreateCombinations();
            InvokeRepeating("SpawnVertical", 0.1f, timeForCreation);
        }
    }




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
    }

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


        if (--counterUp <= 0)
        {
            CancelInvoke("SpawnInitialVertical");
        }
    }


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

        if (--counterUp <= 0)
        {
            CancelInvoke("SpawnVertical");

            if (gameOver == false)
            {

                CreateCombinations();
                SpawnCornersHorizontal();
            }
        }
    }

}

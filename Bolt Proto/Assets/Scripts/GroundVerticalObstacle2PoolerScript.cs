using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundVerticalObstacle2PoolerScript : MonoBehaviour
{
    public int pooledAmount = 1;

    public bool willGrow = true;

    public static GroundVerticalObstacle2PoolerScript current;
    public GameObject pooledObject;

    

    List<GameObject> pooledObjects;

    private void Awake()
    {
        current = this;
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        if (willGrow)
        {
            GameObject newObject = (GameObject)Instantiate(pooledObject);

            pooledObjects.Add(newObject);
            return (newObject);
        }

        return null;
    }


    void Start()
    {
        pooledObjects = new List<GameObject>();

        for (int i = 0; i < pooledAmount; i++)
        {
            GameObject newObject = (GameObject)Instantiate(pooledObject);
            newObject.SetActive(false);
            pooledObjects.Add(newObject);
        }


    }



}

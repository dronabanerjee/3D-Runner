using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corner2PoolerScript : MonoBehaviour
{
    public static Corner2PoolerScript current;
    public GameObject pooledObject;

    public int pooledAmount = 5;

    public bool willGrow = true;

    List<GameObject> pooledObjects;

    private void Awake()
    {
        current = this;
    }
    // Start is called before the first frame update
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

}

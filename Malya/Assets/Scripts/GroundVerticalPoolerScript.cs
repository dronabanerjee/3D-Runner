using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundVerticalPoolerScript : MonoBehaviour
{
    public static GroundVerticalPoolerScript current;
    public GameObject pooledObject;
    public int pooledAmount = 10;
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

        for(int i=0; i<pooledAmount; i++)
        {
            GameObject newObject = (GameObject)Instantiate(pooledObject);
            //newObject.tag = "terrain";
            newObject.SetActive(false);
            pooledObjects.Add(newObject);

        }
    }

   
    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if(!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }

        }
        if(willGrow)
        {
            GameObject newObject = (GameObject)Instantiate(pooledObject);
            pooledObjects.Add(newObject);
            return (newObject);
        }
        return null;
    }
}

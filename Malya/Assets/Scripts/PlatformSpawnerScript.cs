using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawnerScript : MonoBehaviour
{

    public GameObject platform;
    public bool gameOver;
    Vector3 lastPos;
    float size;
    int direction;
    private int counterUp;
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

        if(--counterUp <= 0)
        {
            CancelInvoke("SpawnInitialVertical");
        }
    }

    Bounds GetMaxBounds(GameObject g)
    {
        var b = new Bounds(g.transform.position, Vector3.zero);

        foreach (Renderer r in (IEnumerable<Renderer>)g.GetComponentInChildren<Renderer>())
        {
            b.Encapsulate(r.bounds);
        }
        return b;
    }
}

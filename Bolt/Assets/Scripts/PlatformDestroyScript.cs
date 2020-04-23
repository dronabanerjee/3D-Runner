using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/**
 *  PlatformDestroyScript class destroys the platform which have already been used or crossed  
 */
public class PlatformDestroyScript : MonoBehaviour
{
    
    void Destroy()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Invoke("Destroy", 1f);
        }
    }
}

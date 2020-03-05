using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

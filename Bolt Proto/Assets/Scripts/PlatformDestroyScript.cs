using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDestroyScript : MonoBehaviour
{

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Invoke("Destroy", 1f);
        }
    }


    void Destroy()
    {
        gameObject.SetActive(false);
    }

}

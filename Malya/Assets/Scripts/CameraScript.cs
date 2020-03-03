using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    Transform player;

    [SerializeField]
    float maxAngle = 7f;
    private Vector3 offsetPosition;


    // Start is called before the first frame update
    void Start()
    {
        offsetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.TransformPoint(offsetPosition);
        var targetRotation = Quaternion.LookRotation(player.position - new Vector3(transform.position.x, transform.position.y - 2f, transform.position.z));

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxAngle);

    }
}

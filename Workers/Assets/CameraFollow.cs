using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    void Awake()
    {
        if (target == null)
            target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, .1f);
    }
}

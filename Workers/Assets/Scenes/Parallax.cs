using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startPos;
    [SerializeField] private GameObject cam;
    [SerializeField] private float parallaxAmount;
    [SerializeField] [Range(1, 60)] private float movementMultiplier = 40f;

    [Range(0f, 1f)] public float smoothSpeed = .1f;

    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void LateUpdate()
    {
        float movedFromCamera = cam.transform.position.x * (1 - parallaxAmount);
        float dist = cam.transform.position.x * parallaxAmount;
        transform.position = new Vector3(startPos + dist * movementMultiplier, transform.position.y, transform.position.z);
        //transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
 
        if (movedFromCamera > startPos + length)
            startPos += length;
        else if (movedFromCamera < startPos - length)
            startPos -= length;
    }
}

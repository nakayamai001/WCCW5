using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatInAir : MonoBehaviour
{
    public float amplitude = 0.5f; 
    public float frequency = 1f; 

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position; 
    }

    void Update()
    {
        Vector3 tempPos = startPos;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        transform.position = tempPos;
    }
}

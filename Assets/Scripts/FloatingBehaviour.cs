using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingBehaviour : MonoBehaviour
{
    public float amplitude = 0.5f; //Floating range
    public float frequency = 1f; // Floating frequncy
    public float wanderRadiusX = 5f; 
    public float wanderRadiusZ = 3f;
    public float wanderSpeed = 0.1f;

    private Vector3 startPos;
    private Vector3 targetPos;


    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // floating
        Vector3 floatPos = startPos;
        floatPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        // wander
        if (Vector3.Distance(transform.position, targetPos) < 0.5f)
        {
            float randomX = Random.Range(-wanderRadiusX, wanderRadiusX);
            float randomZ = Random.Range(-wanderRadiusZ, wanderRadiusZ);
            Vector3 randomDirection = new Vector3(randomX, 0, randomZ);
            targetPos = startPos + randomDirection;
        }

        transform.position = Vector3.Lerp(transform.position, floatPos + (targetPos - startPos), wanderSpeed * Time.deltaTime);
    }
}

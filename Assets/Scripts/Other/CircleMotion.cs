using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMotion : MonoBehaviour
{
    
    public float rotationSpeed = 100.0f;
    
    

    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        
    }
}

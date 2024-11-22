using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterboundsHandler : MonoBehaviour
{
    public GameObject resetPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.transform.position = resetPos.transform.position;
        }
        if(other.CompareTag("PlayerGiganto"))
        {
            other.transform.position = resetPos.transform.position;
        }
    }
}

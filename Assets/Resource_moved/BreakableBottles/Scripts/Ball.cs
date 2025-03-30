using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(-transform.forward * 400, ForceMode.Force);
    }

    
    void Update()
    {
        Destroy(this.gameObject, 4f);
    }
}

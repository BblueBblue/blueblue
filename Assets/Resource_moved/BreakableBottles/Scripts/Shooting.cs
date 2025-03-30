using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject obj;
    private Transform tr;

    void Start()
    {
        tr = GetComponent<Transform>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Instantiate(obj, tr.position, Quaternion.Euler(tr.rotation.x,tr.rotation.y,tr.rotation.z));
            Debug.Log("Clicked!");
        }
    }
    

}

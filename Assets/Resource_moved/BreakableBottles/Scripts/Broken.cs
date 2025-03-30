using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broken : MonoBehaviour
{
    public GameObject brokenObj;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            Instantiate(brokenObj, transform.position,Quaternion.Euler(transform.rotation.x,transform.rotation.y,transform.rotation.z));
            Destroy(this.gameObject);
        }
    }
}

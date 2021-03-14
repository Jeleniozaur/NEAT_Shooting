using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
    Rigidbody rb;
    Transform target;
    public float fitness = -10;
    float hitPoints = 0;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        target = GameObject.Find("Target").transform;
    }

    private void OnCollisionEnter(Collision collision)
    {
        hitPoints = 10f;
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        gameObject.GetComponent<SphereCollider>().enabled = false;
    }

    private void Update()
    {
        if (-Vector3.Distance(transform.position, target.position) > fitness)
        {
            fitness = -Vector3.Distance(transform.position, target.position) + hitPoints;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    public Vector3 dir;
    public float speed;

    private void Start()
    {
        ChangePosition();
        StartCoroutine(MoveCycleX());
        StartCoroutine(MoveCycleY());
        StartCoroutine(MoveCycleZ());
    }

    private void OnEnable()
    {
        NEAT.OnNextGeneration += ChangePosition;
    }

    private void FixedUpdate()
    {
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.Lerp(gameObject.GetComponent<Rigidbody>().velocity, dir * speed,0.05f);
    }

    private void OnDisable()
    {
        NEAT.OnNextGeneration -= ChangePosition;
    }

    void ChangePosition()
    {
        transform.position = new Vector3(Random.Range(-1.0f, 1f), Random.Range(0.5f, 2f), Random.Range(0f, 1f));
    }

    IEnumerator MoveCycleX()
    {
        yield return new WaitForSeconds(2f);
        dir.x = dir.x * -1f;
        StartCoroutine(MoveCycleX());
    }
    IEnumerator MoveCycleY()
    {
        yield return new WaitForSeconds(1f);
        dir.y = dir.y * -1f;
        StartCoroutine(MoveCycleY());
    }
    IEnumerator MoveCycleZ()
    {
        yield return new WaitForSeconds(3f);
        dir.z = dir.z * -1f;
        StartCoroutine(MoveCycleZ());
    }
}

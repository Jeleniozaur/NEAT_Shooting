using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunScript : MonoBehaviour
{
    public Transform bul;
    public Transform bulletPrefab;
    public float bulletVel = 10f;
    bool shot = false;
    [Range(-1f, 1f)]
    public float xMove, yMove, xRot, yRot;
    public float rotSpeed = 5f;
    public float moveSpeed = 5f;
    Transform bullet;
    NeuralNetwork brain;
    Transform target;
    float willShot;
    Vector2 shotDiff;
    Vector2 dir;
    float dist;

    private void Start()
    {
        transform.position = new Vector3(0, 1.2f, -3f);
        var neat = GameObject.Find("NEAT").GetComponent<NEAT>();
        if (this.transform != neat.population[0])
        {
            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            foreach (Transform child in this.transform)
            {
                child.GetComponent<MeshRenderer>().enabled = false;
            }
        }

        target = GameObject.Find("Target").transform;
        brain = gameObject.GetComponent<NeuralNetwork>();
    }

    public void shoot()
    {
        shotDiff = (target.position - transform.position).normalized;
        bullet = Instantiate(bulletPrefab);
        bullet.position = bul.position;
        bullet.GetComponent<Rigidbody>().velocity = bul.forward * bulletVel;
        shot = true;

        var neat = GameObject.Find("NEAT").GetComponent<NEAT>();
        if (this.transform != neat.population[0])
        {
            bullet.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    void manageBrain()
    {
        //var dir1 = (target.position - bul.position).normalized;
        //var dir2 = (target.position - bul.position + bul.forward).normalized;
        //diff = dir1 - dir2;


        //brain.setInputValue(-1f, 1f, diff.x, brain.inputLayer[0]);
        //brain.setInputValue(-1f, 1f, diff.y, brain.inputLayer[1]);
        //brain.setInputValue(-1f, 1f, diff.z, brain.inputLayer[2]);
        var targetVel = target.GetComponent<Rigidbody>().velocity;
        brain.inputLayer.nodes[0].value = dir.x;
        brain.inputLayer.nodes[1].value = dir.y;
        brain.inputLayer.nodes[2].value = dist;
        //brain.inputLayer.nodes[2].value = targetVel.x;
        //brain.inputLayer.nodes[3].value = targetVel.y;
        //brain.inputLayer.nodes[4].value = targetVel.z;

        //xMove = brain.getOutputValue(-1f, 1f, brain.outputLayer[0]);
        //yMove = brain.getOutputValue(-1f, 1f, brain.outputLayer[1]);
        //xRot = brain.getOutputValue(-1f, 1f, brain.outputLayer[0]);
        //yRot = brain.getOutputValue(-1f, 1f, brain.outputLayer[1]);
        //willShot = brain.getOutputValue(0, 1f, brain.outputLayer[2]);
        xMove = brain.outputLayer.nodes[0].value;
        yMove = brain.outputLayer.nodes[1].value;
        willShot = brain.outputLayer.nodes[2].value;
    }

    private void Update()
    {
        var b = transform.GetChild(0).GetChild(0);
        dir = target.position - b.position;
        dir = new Vector2(Mathf.Clamp(dir.x, -1f, 1f), Mathf.Clamp(dir.y, -1f, 1f));
        dist = Vector2.Distance(b.position, new Vector2(target.position.x, target.position.y));
        //dir.Normalize();
        moveGun();
        manageBrain();
        if(willShot>=0.9f && !shot)
        {
            shoot();
        }
        
        manageFitness();
    }

    void moveGun()
    {
        transform.Rotate(xRot*rotSpeed*Time.deltaTime, yRot*rotSpeed*Time.deltaTime, 0);
        transform.position += new Vector3(xMove * moveSpeed*Time.deltaTime, yMove * moveSpeed*Time.deltaTime, 0);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -1.4f, 1.4f), Mathf.Clamp(transform.position.y, 0.2f, 2f), -3f);
    }

    void manageFitness()
    {
        if (!bullet)
        {
            brain.fitness = -10f;
        }
        else
        {
            brain.fitness = bullet.GetComponent<bulletScript>().fitness;
        }
    }

    private void OnDestroy()
    {
        if(bullet)
        {
            Destroy(bullet.gameObject);
        }
    }
}

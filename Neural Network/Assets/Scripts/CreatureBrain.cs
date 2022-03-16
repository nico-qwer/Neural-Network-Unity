using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureBrain : MonoBehaviour
{
    Transform target;
    public Rigidbody rBody;
    public float fitness = 0f;
    public float collided = 0f;
    float collisionTime = 0f;
    public float multiplicator = 20f;
    
    public NeuralNetwork brain;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Finish").transform;
    }

    void FixedUpdate()
    {
        float[] directions = brain.Compute(new float[]{target.position.x, target.position.z, transform.position.x, transform.position.z});
        Move(Mathf.Clamp(directions[0], -2, 2), Mathf.Clamp(directions[1], -2, 2));
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "Wall" || collisionInfo.collider.tag == "WallSegment") collisionTime = Time.time;
    }

    void OnCollisionExit(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "Wall" || collisionInfo.collider.tag == "WallSegment")
        {
            collided += Time.time - collisionTime;
        }
    }

    void Move(float moveX, float moveZ)
    {
        rBody.AddForce(new Vector3(moveX * multiplicator, 0f, moveZ * multiplicator));
    }
}

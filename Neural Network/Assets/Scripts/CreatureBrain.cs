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
    public Eye[] eyes;
    
    public NeuralNetwork brain;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Finish").transform;
    }

    void FixedUpdate()
    {
        Vector2 targetDirection = new Vector2(
            target.position.x - transform.position.x,
            target.position.z - transform.position.z
        );

        targetDirection.Normalize();
        float[] directions = brain.Compute(new float[]{
            targetDirection.x,
            targetDirection.y,
            eyes[0].activated,
            eyes[1].activated,
            eyes[2].activated,
            eyes[3].activated
        });
        
        Move(Mathf.Clamp(directions[0], -0.1f, 0.1f), Mathf.Clamp(directions[1], -0.1f, 0.1f));
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
        rBody.velocity = new Vector3(moveX * multiplicator, 0f, moveZ * multiplicator);
    }
}

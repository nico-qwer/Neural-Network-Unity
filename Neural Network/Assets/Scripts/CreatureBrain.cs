using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureBrain : MonoBehaviour
{
    Transform target;
    public Rigidbody rBody;
    public float fitness = 0f;
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
        Move(Mathf.Clamp(directions[0], float.NegativeInfinity, 20), Mathf.Clamp(directions[1], float.NegativeInfinity, 20));
    }

    void Move(float moveX, float moveZ)
    {
        rBody.AddForce(new Vector3(moveX * multiplicator, 0f, moveZ * multiplicator));
    }
}

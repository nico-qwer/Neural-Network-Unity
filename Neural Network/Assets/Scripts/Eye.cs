using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : MonoBehaviour
{
    public float activated = 0f;

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collider>().tag == "Wall" || other.GetComponent<Collider>().tag == "WallSegment")
        {
            activated = 1f;
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Collider>().tag == "Wall" || other.GetComponent<Collider>().tag == "WallSegment")
        {
            activated = 0f;
        }
    }
}

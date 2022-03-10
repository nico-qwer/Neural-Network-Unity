using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenFollower : MonoBehaviour
{
    public SpawnManager spawnManager;
    public Text text;
    int previous = 0;
    // Update is called once per frame
    void Update()
    {
        if (spawnManager.Generation != previous)
        {
            previous = spawnManager.Generation;
            text.text = spawnManager.Generation.ToString();
        }
    }
}

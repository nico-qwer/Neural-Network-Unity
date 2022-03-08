using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    int numAgents = 10;
    float wait = 30f;
    public GameObject AgentPrefab;
    public Transform spawn;

    // Start is called before the first frame update
    void Start()
    {
        spawn = GameObject.FindWithTag("Start").transform;
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        yield return new WaitForSeconds(5f);
        while (true)
        {
        //Spawns new agents
        for (int i = 0; i < numAgents; i++)
        {
            GameObject newAgent = Instantiate(
                    AgentPrefab,
                    new Vector3(
                        spawn.position.x,
                        spawn.position.y,
                        spawn.position.z
                    ),
                    Quaternion.identity
            );
            newAgent.transform.SetParent(spawn);
        }
        //Waits a bit
        yield return new WaitForSeconds(wait);
        }
    }
}

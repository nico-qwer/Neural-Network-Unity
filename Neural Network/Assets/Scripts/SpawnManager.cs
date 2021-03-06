using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [HideInInspector]
    public int Generation = 0;

    [Header("Parameters")]
    public int numAgents = 10;
    public float wait = 30f;
    public float timeMultiplier = 1f;

    [Header("Limits")]
    public float wallPunition = 2f;

    [Header("References")]
    public GameObject AgentPrefab;
    public SpawnMaze mazeGen;
    public Transform spawn;
    public Transform target;
    public Material normalMat;
    public Material bestMat;
    public int[] neuronsOnLayers;
    float[][][] bestWeights;

    // Start is called before the first frame update
    void Start()
    {
        bestWeights = PopulateTripleArray(neuronsOnLayers, 0f);
        StartCoroutine(SpawnLoop());
    }

    void Update()
    {
        Time.timeScale = timeMultiplier;

        if (Generation != 0)
        {
            Invoke("UpdateVisuals", 0.05f);
        }
    }

    IEnumerator SpawnLoop()
    {
        mazeGen.CreateMap();
        yield return new WaitForSeconds(2f);

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
            newAgent.name = "Agent " + i.ToString();
            CreatureBrain brain = (CreatureBrain)newAgent.GetComponent(typeof(CreatureBrain));
            brain.brain = new NeuralNetwork(neuronsOnLayers, bestWeights);
            brain.brain.Mutate();
            newAgent.transform.SetParent(spawn);
        }
        Generation = 1;

        //Resets agents with new brain
        while (true)
        {
            yield return new WaitForSeconds(wait);

            Generation++;

            GameObject[] agents = GameObject.FindGameObjectsWithTag("Agent");
            GameObject bestAgent = null;
            float bestAgentFitness = float.PositiveInfinity;

            for (int i = 0; i < agents.Length; i++)
            {
                //Computes fitness of current agent
                CreatureBrain currentBrain = (CreatureBrain)agents[i].GetComponent(typeof(CreatureBrain));
                currentBrain.fitness = ComputeFitness(agents[i]);

                //Gets best agent
                if (bestAgentFitness > currentBrain.fitness)
                {
                    bestAgent = agents[i];
                    bestAgentFitness = currentBrain.fitness;
                }
            }

            CreatureBrain brain = (CreatureBrain)bestAgent.GetComponent(typeof(CreatureBrain));
            bestWeights = brain.brain.GetWeights();
            Debug.Log("The best agent was " + bestAgent.name + " with his impressive " + bestAgentFitness + " fitness!!", bestAgent);

            mazeGen.CreateMap();
            
            for (int i = 0; i < agents.Length; i++)
            {
                CreatureBrain currentBrain = (CreatureBrain)agents[i].GetComponent(typeof(CreatureBrain));
                currentBrain.collided = 0f;
                if (agents[i] == bestAgent)
                {
                    agents[i].transform.position = spawn.position;
                    continue;
                }
                currentBrain.brain.SetWeights(bestWeights);
                currentBrain.brain.Generation = Generation;
                currentBrain.brain.Mutate();
                agents[i].transform.position = spawn.position;
            }
        }
    }

    //Calculates agent fitness
    float ComputeFitness(GameObject agent)
    {
        CreatureBrain currentBrain = (CreatureBrain)agent.GetComponent(typeof(CreatureBrain));
        
        float distX = target.position.x - agent.transform.position.x;
        float distZ = target.position.z - agent.transform.position.z;
        
        float agentFitness = Mathf.Pow(distX, 2) + Mathf.Pow(distZ, 2);

        agentFitness += currentBrain.collided * wallPunition;

        return Mathf.Abs(agentFitness);
    }

    public float[][][] PopulateTripleArray(int[] layers, float value)
    {
        List<float[][]> weightsList = new List<float[][]>(); //Creates list of all weights

        for (int i = 1; i < layers.Length; i++) //Loops over all layers excluding input layer
        {
            List<float[]> layerWeightsList = new List<float[]>(); //Creates list of all weights in layers
            int neuronsInPreviousLayer = layers[i - 1]; //Gets previous layer

            for (int j = 0; j < layers[i]; j++) //Loops over all neurons
            {
                float[] neuronWeights = new float[neuronsInPreviousLayer]; //Creates array of weights in one neuron

                for (int k = 0; k < neuronsInPreviousLayer; k++) //Loops over all weights of the current neuron
                {
                    neuronWeights[k] = value;
                }
                layerWeightsList.Add(neuronWeights); //Adds the weights of the current neuron to the layer
            }
            weightsList.Add(layerWeightsList.ToArray()); //Adds the weights of the layer to the weights
        }
        return weightsList.ToArray(); //Sets weight to new weights
    }
    void UpdateVisuals()
    {
        GameObject[] agents = GameObject.FindGameObjectsWithTag("Agent");
        GameObject bestAgent = null;
        float bestAgentFitness = float.PositiveInfinity;
        for (int i = 0; i < agents.Length; i++)
        {
            //Computes fitness of current agent          
            CreatureBrain currentBrain = (CreatureBrain)agents[i].GetComponent(typeof(CreatureBrain));
            float currentFitness = ComputeFitness(agents[i]);

            //Gets best agent
            if (bestAgentFitness > currentFitness)
            {
                bestAgent = agents[i];
                bestAgentFitness = currentFitness;
            }
            agents[i].GetComponent<Renderer>().material = normalMat;
        }
        bestAgent.GetComponent<Renderer>().material = bestMat;
    }
}

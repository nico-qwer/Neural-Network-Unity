using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public int Generation = 0;
    public int numAgents = 10;
    public float wait = 30f;
    public GameObject AgentPrefab;
    public Transform spawn;
    public Transform target;
    public int[] neuronsOnLayers;
    float[][][] bestWeights;

    // Start is called before the first frame update
    void Start()
    {
        bestWeights = PopulateTripleArray(neuronsOnLayers, 0f);
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
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
            CreatureBrain brain = (CreatureBrain)newAgent.GetComponent(typeof(CreatureBrain));
            brain.brain = new NeuralNetwork(neuronsOnLayers, bestWeights);
            brain.brain.Mutate();
            newAgent.transform.SetParent(spawn);
        }
        Generation = 1;

        while (true)
        {
            yield return new WaitForSeconds(wait);
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

            for (int i = 0; i < agents.Length; i++)
            {
                if (agents[i] == bestAgent) 
                {
                    agents[i].transform.position = spawn.position;
                    continue;
                }
                CreatureBrain currentBrain = (CreatureBrain)agents[i].GetComponent(typeof(CreatureBrain));
                currentBrain.brain.SetWeights(bestWeights);
                currentBrain.brain.Mutate();
                agents[i].transform.position = spawn.position;
                Generation++;
            }
        }
    }

    //Calculates agent fitness
    float ComputeFitness(GameObject agent)
    {
        float distX = target.position.x - agent.transform.position.x;
        float distZ = target.position.z - agent.transform.position.z;

        float agentFitness = Mathf.Pow(distX, 2) + Mathf.Pow(distZ, 2);

        return agentFitness;
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
}

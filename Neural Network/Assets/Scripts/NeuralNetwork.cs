using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork
{
    float defaultValue = 0f; //Default weight assigned at the beginning 
    int[] layers; //The layers and their amount of neurons
    float[][] neurons; //The values of all of the neurons
    float[][][] weights; //The weights of all of the connections
    public int Generation = 0;

    //-----------------------------------------CONSTRUCTOR FUNCTIONS-----------------------------------------//
    
    //Constructor (Will be called when the network is created, only once)
    public NeuralNetwork(int[] newLayers, float[][][] newWeights) //Takes in an int array of all the layers and amount of neurons in them
    {
        layers = new int[newLayers.Length]; //Assign an array with the good lenght to the layers array

        for (int i = 0; i < layers.Length; i++) //Loops over all the layers
        {
            layers[i] = newLayers[i]; //Give the layers the good amount of neurons
        }

        InitNeurons();
        SetWeights(newWeights);
    }

    //Initializes all the neurons
    void InitNeurons()
    {
        List<float[]> neuronsList = new List<float[]>(); //Creates list of all neurons

        for (int i = 0; i < layers.Length; i++) //Loops over all layers
        {
            float[] neuronValues = new float[layers[i]];

            for (int j = 0; j < neuronValues.Length; j++) //Loops over all neurons
            {
                neuronValues[j] = defaultValue; //Assigns a default value to each neuron
            }

            neuronsList.Add(new float[layers[i]]); //Adds layer to neurons list
        }

        neurons = neuronsList.ToArray(); //Converts list to array
    }

    //Initializes all the weights
    public void SetWeights(float[][][] newWeights)
    {
        List<float[][]> weightsList = new List<float[][]>(); //Creates list of all weights

        for (int i = 1; i < layers.Length; i++) //Loops over all layers excluding input layer
        {
            List<float[]> layerWeightsList = new List<float[]>(); //Creates list of all weights in layers
            int neuronsInPreviousLayer = layers[i - 1]; //Gets previous layer

            for (int j = 0; j < neurons[i].Length; j++) //Loops over all neurons
            {
                float[] neuronWeights = new float[neuronsInPreviousLayer]; //Creates array of weights in one neuron

                for (int k = 0; k < neuronsInPreviousLayer; k++) //Loops over all weights of the current neuron
                {
                    neuronWeights[k] = newWeights[i - 1][j][k]; //Assigns a value to the weight
                }
                layerWeightsList.Add(neuronWeights); //Adds the weights of the current neuron to the layer
            }
            weightsList.Add(layerWeightsList.ToArray()); //Adds the weights of the layer to the weights
        }
        weights = weightsList.ToArray(); //Sets weight to new weights
    }
    //-----------------------------------------CONSTRUCTOR FUNCTIONS END-----------------------------------------//

    //Mutates weights 
    public void Mutate()
    {
        float bias = Mathf.Clamp(((float)Generation / 20f), 0f, 0.5f);
        for (int i = 1; i < layers.Length; i++) //Loops over all layers excluding input layer
        {
            int neuronsInPreviousLayer = layers[i - 1]; //Gets previous layer
            for (int j = 0; j < neurons[i].Length; j++) //Loops over all neurons
            {
                for (int k = 0; k < neuronsInPreviousLayer; k++) //Loops over all weights of the current neuron
                {
                    float rng = Random.value; //Chooses a random value
                    rng = BiasFunction(rng, bias);
                    weights[i - 1][j][k] += (rng * 2 - 0.5f) / 8f;
                }
            }
        }
    }

    //Calculates output neurons based on inputs
    public float[] Compute(float[] inputs)
    {
        neurons[0] = inputs; //Sets input neurons values to input

        for (int i = 0; i < neurons[0].Length; i++) //Loops over all input neurons
        {
            neurons[0][i] = inputs[i]; //Sets neurons value to coresponding value in input
        }

        for (int i = 1; i < layers.Length; i++) //Loops over all layers excluding input layer
        {
            int neuronsInPreviousLayer = layers[i - 1]; //Gets previous layer
            for (int j = 0; j < layers[i]; j++) //Loops over all neurons
            {
                float neuronValue = 0f; //Sets neuron value to 0
                for (int k = 0; k < neuronsInPreviousLayer; k++) //Loops over all weights of the current neuron
                {
                    neuronValue += weights[i - 1][j][k] * neurons[i - 1][k]; //Computes value for current connection
                }
                neurons[i][j] = neuronValue; //Sets the value of the current neuron the the computed value
            }

        }
        return neurons[layers.Length - 1]; //Outputs the values of the last layer of neurons
    }

    public float[][][] GetWeights()
    {
        return weights;
    }

    public float[][][] PopulateTripleArray(float defaultValue = 0f)
    {
        List<float[][]> weightsList = new List<float[][]>(); //Creates list of all weights

        for (int i = 1; i < layers.Length; i++) //Loops over all layers excluding input layer
        {
            List<float[]> layerWeightsList = new List<float[]>(); //Creates list of all weights in layers
            int neuronsInPreviousLayer = layers[i - 1]; //Gets previous layer

            for (int j = 0; j < neurons[i].Length; j++) //Loops over all neurons
            {
                float[] neuronWeights = new float[neuronsInPreviousLayer]; //Creates array of weights in one neuron

                for (int k = 0; k < neuronsInPreviousLayer; k++) //Loops over all weights of the current neuron
                {
                    neuronWeights[k] = defaultValue; //Assigns a value to the weight
                }
                layerWeightsList.Add(neuronWeights); //Adds the weights of the current neuron to the layer
            }
            weightsList.Add(layerWeightsList.ToArray()); //Adds the weights of the layer to the weights
        }
        return weightsList.ToArray(); //Sets weight to new weights
    }

    float BiasFunction(float x, float bias)
    {
        float k = Mathf.Pow(1-bias, 3);
        return (x * k) / (x * k - x + 1);
    }
}

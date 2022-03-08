using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpawnMaze))]
public class CustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        SpawnMaze mazeGen = (SpawnMaze)target;
        if (GUILayout.Button("Generate Maze"))
        {
            mazeGen.CreateMap();
        }
        if (GUILayout.Button("Clear"))
        {
            mazeGen.DeleteAllSegements();
        }
    }    
}

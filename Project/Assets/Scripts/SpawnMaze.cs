using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SpawnMaze : MonoBehaviour
{
    public int width = 30;
    public int lenght = 30;
    [Range(0f, 1f)]
    public float filled = 0.5f;
    public Transform parent;
    public GameObject wallSegment;

    bool RandomBool(float chance)
    {
        return (Random.value < chance);
    }

    public void CreateMap()
    {
        DeleteAllSegements();

        for (int x = 0; x < width - 2; x++)
        {
            for (int z = 0; z < width - 2; z++)
            {
                if (RandomBool(filled) != true) continue;

                GameObject newSegment = Instantiate(
                    wallSegment,
                    new Vector3(
                        x - (width / 2 - 1) + parent.position.x + 0.5f,
                        parent.position.y + (wallSegment.transform.localScale.y / 2),
                        z - (lenght / 2 - 1) + parent.position.z + 0.5f
                    ),
                    Quaternion.identity
                );
                newSegment.transform.SetParent(parent);
            }
        }
    }

    public void DeleteAllSegements()
    {
        foreach (GameObject seg in GameObject.FindGameObjectsWithTag("WallSegment"))
        {
            if (seg.name == "Wall(Clone)")
            {
                DestroyImmediate(seg);
            }
        }
    }
}
